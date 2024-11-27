import '../../assets/css/workspaces/Empty.css'
import {useContext, useEffect, useState} from "react";
import ContactsService from "../../services/ContactsService.tsx";
import {PageContext} from "../../hooks/PageContext.tsx";
import ContactResponse from "../../models/Response/ContactResponse.tsx";
import {ContactsStatuses} from "../../models/Enums/ContactsStatuses.tsx";

function ContactsWorkspace(): JSX.Element {
    const {page} = useContext(PageContext);
    const [contacts, setContacts] = useState<ContactResponse[] | undefined>(undefined);

    const contactsService = new ContactsService();

    const [emailInput, setEmailInput] = useState<string>('');
    
    const getStatusLabel = (status: ContactsStatuses): string => {
        switch (status) {
            case ContactsStatuses.Sent:
                return 'ожидаем ответа';
            case ContactsStatuses.Received:
                return 'пришла заявка на добавление';
            case ContactsStatuses.Accept:
                return 'контакт добавлен';
            default:
                return 'Неизвестный статус';
        }
    };
    
    useEffect(() => {
        const accountId = localStorage.getItem('accountId');
        
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
    
    
    function AddContact(){
        const accountId = localStorage.getItem('accountId');
        
        const fetchContact = async () => {
            try {
                await contactsService.addContact(accountId!, emailInput);
                const requestedContacts = await contactsService.getContacts(accountId!);
                setContacts(requestedContacts);
            } catch (error) {
                console.error('Error fetching contacts:', error);
            }
        };

        fetchContact().then();
    }

    function AcceptContact(email: string){
        const accountId = localStorage.getItem('accountId');

        const fetchContact = async () => {
            try {
                await contactsService.acceptContact(accountId!, email);
                const requestedContacts = await contactsService.getContacts(accountId!);
                setContacts(requestedContacts);
            } catch (error) {
                console.error('Error fetching contacts:', error);
            }
        };

        fetchContact().then();
    }
    
    return <div className="workspace-container">
        <div className="workspace-sent-container">
            <p className="workspace-title">Контакты</p>

            <div className="send-contact-invite">
                <div className="left-block">
                    <div className="input-container">
                        <input className="subject-input" type="text" id="email-input" placeholder="Введите получателя" value={emailInput}
                               onChange={(e) => setEmailInput(e.target.value)}/>
                    </div>
                </div>
                <div className="right-block">
                    <div className="send-invite-button" onClick={AddContact}>
                        <p>Добавить</p>
                    </div>
                </div>
            </div>

            {contacts ? (
            <div className="contacts-list">
                {contacts.map((contact) => (
                    <div key={contact.email} className="contact">
                        <div className="contact-info">
                            <p className="contact-email">{contact.email}</p>

                            {
                                contact.status == ContactsStatuses.Received ? (
                                        <p className="received-email">Статус: {getStatusLabel(contact.status)}</p>
                                    ):
                                    <p>Статус: {getStatusLabel(contact.status)}</p>
                            }
                        </div>
                        {
                            contact.status == ContactsStatuses.Received ? (
                                <button className="action-button" onClick={() => AcceptContact(contact.email)}>
                                    Принять
                                </button>
                            ) : null
                        }
                    </div>
                ))}
            </div>
            ) : (
                <div></div>
            )}
        </div>
    </div>
}

export default ContactsWorkspace;