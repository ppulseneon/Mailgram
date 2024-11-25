import '../../assets/css/workspaces/Sent.css'
import {useEffect, useRef, useState} from "react";

function SentWorkspace(): JSX.Element {
    const [htmlContent, setHtmlContent] = useState('<p>Ваше сообщение</p>');
    const editorRef = useRef(null);

    useEffect(() => {
        if (editorRef.current) {
            (editorRef.current as HTMLElement).innerHTML = htmlContent;
        }
    }, [htmlContent]);

    const handleBoldClick = () => {
        document.execCommand('bold', false, null);
        updateHtmlContent();
    };

    const handleItalicClick = () => {
        document.execCommand('italic', false, null);
        updateHtmlContent();
    };

    const handleUnderlineClick = () => {
        document.execCommand('underline', false, null);
        updateHtmlContent();
    };

    const updateHtmlContent = () => {
        if (editorRef.current) {
            setHtmlContent((editorRef.current as HTMLElement).innerHTML);
        }
    };

    const handleInputChange = (e: any) => {
        setHtmlContent(e.target.innerHTML)
    }
    
    return <div className="workspace-container">
        <div className="workspace-sent-container">
            <p className="workspace-title">Отправка письма</p>
            <div className="input-email">

            </div>
            <div className="input-container">
                <input className="subject-input" type="text" id="email-input" placeholder="Введите получателя"/>
            </div>
            <div className="input-container">
                <input className="subject-input" type="text" id="subject-input" placeholder="Введите тему письма"/>
            </div>
            <div className="control-container">
                <div className="control-element">
                    <button onClick={handleBoldClick}><b>B</b></button>
                </div>
                <div className="control-element">
                    <button onClick={handleItalicClick}><i>I</i></button>
                </div>
                <div className="control-element">
                    <button onClick={handleUnderlineClick}><u>U</u></button>
                </div>
            </div>
            <div className="input-message">
                <div
                    contentEditable
                    // suppressContentEditableWarning
                    ref={editorRef}
                    onInput={handleInputChange}
                    // dangerouslySetInnerHTML={{__html: htmlContent}}
                />
            </div>
            <div className="sent-container">

            </div>
        </div>
    </div>
}

export default SentWorkspace;