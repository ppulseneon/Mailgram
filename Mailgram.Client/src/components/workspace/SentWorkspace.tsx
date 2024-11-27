import '../../assets/css/workspaces/Sent.css'
import {convertFromRaw, convertToRaw, Editor, EditorState, RichUtils} from 'draft-js';
import {useContext, useEffect, useState} from "react";
import {stateToHTML} from 'draft-js-export-html';
import {PageContext} from "../../hooks/PageContext.tsx";
import ContactResponse from "../../models/Response/ContactResponse.tsx";
import ContactsService from "../../services/ContactsService.tsx";
import {ContactsStatuses} from "../../models/Enums/ContactsStatuses.tsx";
import MessagesService from "../../services/MessagesService.tsx";
import DraftRequest from "../../models/Request/DraftRequest.tsx";
import SendMessageRequest from "../../models/Request/SendMessageRequest.tsx";

function SentWorkspace(): JSX.Element {
    const [editorState, setEditorState] = useState(EditorState.createEmpty());
    const [isEncryptChecked, setEncryptChecked] = useState(false);
    const [isEcpChecked, setEcpChecked] = useState(false);
    const [files, setFiles] = useState([]);
    const {page} = useContext(PageContext);
    const [contacts, setContacts] = useState<ContactResponse[] | undefined>(undefined);
    const [isCheckboxDisabled, setIsCheckboxDisabled] = useState<boolean>(true);
    const [emailInputValue, setEmailInputValue] = useState<string>('');
    const [subjectInputValue, setSubjectInputValue] = useState<string>('');
    
    const emailService = new MessagesService();
    
    const handleEncryptChange = (event: any) => {
        setEncryptChecked(event.target.checked);
    };

    const handleEcpChange = (event: any) => {
        setEcpChecked(event.target.checked);
    };

    useEffect(() => {
        const accountId = localStorage.getItem('accountId');

        const contactsService = new ContactsService();
        
        const fetchContacts = async () => {
            try {
                const requestedContacts = await contactsService.getContacts(accountId!);
                setContacts(requestedContacts);
                console.log(requestedContacts)
            } catch (error) {
                console.error('Error fetching contacts:', error);
            }
        };

        fetchContacts().then();

    }, [page, localStorage.getItem('accountId')]);
    
    const handleKeyCommand = (command: any) => {
        const newState = RichUtils.handleKeyCommand(editorState, command);
        if (newState) {
            setEditorState(newState);
            return 'handled';
        }
        return 'not-handled';
    };
    
    const onItalicClick = () => {
        setEditorState(RichUtils.toggleInlineStyle(editorState, 'ITALIC'));
    };

    const onUnderlineClick = () => {
        setEditorState(RichUtils.toggleInlineStyle(editorState, 'UNDERLINE'));
    };

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newFiles = Array.from(event.target.files as FileList);
        setFiles([...files, ...newFiles]);
    };
    
    const handleDeleteFile = (index: number) => {
        const newFiles = files.filter((_, i) => i !== index);
        setFiles(newFiles);
    };
    
    const getBodyHtml = () => {
        const contentState = editorState.getCurrentContent();
        const rawContentState = convertToRaw(contentState);
        const contentStateFromRaw = convertFromRaw(rawContentState);
        return stateToHTML(contentStateFromRaw);
    }
    
    const sendMessage = () => {
        const accountId = localStorage.getItem('accountId')!;
        const sendMessageRequest: SendMessageRequest = {
            userId: accountId,
            to: emailInputValue,
            subject: subjectInputValue,
            message: getBodyHtml(),
            attachments: files,
            isSign: isEcpChecked,
            isEncrypt: isEncryptChecked,
        };

        const sendRequest = async () => {
            try {
                await emailService.sendMessageRequest(sendMessageRequest);
                console.log('send draft')
            } catch (error) {
                console.error('Error fetching contacts:', error);
            }
        };

        sendRequest().then();
    }

    const saveDraft = () => {
        const accountId = localStorage.getItem('accountId')!;
        const draftRequest = new DraftRequest(accountId, emailInputValue, subjectInputValue, getBodyHtml());

        const sendRequest = async () => {
            try {
                await emailService.sendDraft(draftRequest);
                console.log('send draft')
            } catch (error) {
                console.error('Error fetching contacts:', error);
            }
        };

        sendRequest().then();
    }

    const handleEmailInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        setEmailInputValue(value);
        setIsCheckboxDisabled(!contacts!.some(contact => contact.email === value && contact.status === ContactsStatuses.Accept));
    };

    const handleSubjectInputValue = (event: React.ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;
        setSubjectInputValue(value);
    };
    
    
    return <div className="workspace-container">
        <div className="workspace-sent-container">
            <p className="workspace-title">Отправка письма</p>
            <div className="input-container">
                <input className="subject-input" type="text" id="email-input" placeholder="Введите получателя"
                       value={emailInputValue}
                       onChange={handleEmailInputChange}/>
            </div>
            <div className="input-container">
                <input className="subject-input" type="text" id="subject-input" placeholder="Введите тему письма"
                       value={subjectInputValue}
                       onChange={handleSubjectInputValue}/>
            </div>
            <div className="control-container">
                <div className="control-element">
                    <button onClick={onItalicClick}>Italic</button>
                </div>
                <div className="control-element">
                    <button onClick={onUnderlineClick}>Underline</button>
                </div>
            </div>
            <div className="input-message">
                <Editor
                    editorState={editorState}
                    handleKeyCommand={handleKeyCommand}
                    onChange={setEditorState}
                />
            </div>
            <ul>
                {files.map((file, index) => (
                    <li key={index}>
                        {file.name}
                        <button onClick={() => handleDeleteFile(index)}>Delete</button>
                    </li>
                ))}
            </ul>
            <div className="sent-container">
                <div className="control-element">
                    <button onClick={sendMessage}>Отправить</button>
                </div>
                <div className="control-element">
                    <label>
                        <input
                            type="checkbox"
                            checked={isEncryptChecked}
                            onChange={handleEncryptChange}
                            disabled={isCheckboxDisabled}
                        />
                        Зашифровать
                    </label>
                </div>
                <div className="control-element">
                    <label>
                        <input
                            type="checkbox"
                            checked={isEcpChecked}
                            onChange={handleEcpChange}
                            disabled={isCheckboxDisabled}
                        />
                        Подписать
                    </label>
                </div>
                <div className="control-element">
                    <input
                        type="file"
                        multiple
                        onChange={handleFileChange}
                    />
                </div>
                <div className="control-element">
                    <button onClick={saveDraft}>Сохранить черновик</button>
                </div>
            </div>
        </div>
    </div>
}

export default SentWorkspace;