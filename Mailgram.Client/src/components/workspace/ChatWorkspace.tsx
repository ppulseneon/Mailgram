import {useContext} from "react";
import {ChatContext} from "../../hooks/ChatProvider.tsx";
import '../../assets/css/workspaces/Chat.css'
import {FaStar, FaTrash} from "react-icons/fa";
import {Folders} from "../../models/Enums/Folders.tsx";
import MessagesService from "../../services/MessagesService.tsx";

function ChatWorkspace(): JSX.Element {
    const {chats, openChat} = useContext(ChatContext);

    const openChatObject = chats.find(x => x.id === openChat)!;
    const messages = new MessagesService();
    const accountId = localStorage.getItem('accountId')!;
    
    const deleteMessage = async () => {
        await messages.deleteMessage(accountId!, openChatObject!.id!);
    };

    const changeStar = async () => {
        await messages.changeStar(accountId, openChatObject!.id!);
    };
    
    const handleOpenFile = async (attachment: string) => {
        await messages.getAttachment(accountId, openChatObject.id!, attachment);
    };
    
    function formatDate(date: Date): string {
        if (!(date instanceof Date)) {
            return "Invalid date";
        }

        const hours = date.getHours().toString().padStart(2, '0');
        const minutes = date.getMinutes().toString().padStart(2, '0');
        const day = date.getDate();
        const monthNames = [
            "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня",
            "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря"
        ];
        const month = monthNames[date.getMonth()];
        const year = date.getFullYear();

        return `${day} ${month} ${year} в ${hours}:${minutes}`;
    }

    return <div className="workspace-container">
        <div className="workspace-chat-container">
            <div className="subject-container">
                <h1>{openChatObject.subject == "" || openChatObject.subject == null ? "[Письмо без темы]" : openChatObject.subject}</h1>
            </div>
            <div className="address-container">
                <p>От: {openChatObject.from}</p>
            </div>
            <div className="address-container">
                <p><b>Кому: {openChatObject.to}</b></p>
            </div>
            <div className="control-container">
                <div className="control-element">
                    <p>{formatDate(new Date(openChatObject.date!))}</p>
                </div>


                {
                    openChatObject.folder != Folders.Deleted ? (<div className="control-element" onClick={changeStar}>
                            <div
                                className={openChatObject.folder == Folders.Favorites ? "isFavourite" : "notFavourite"}>
                                <FaStar/></div>
                        </div>
                    ) : null
                }

                {
                    openChatObject.folder != Folders.Deleted ? (<div className="control-element">
                            <div className="trash-bukkit" onClick={deleteMessage}>
                                <FaTrash/></div>
                        </div>
                    ) : null
                }

                {
                    openChatObject.isEncryptedRight ? (
                        <div className="control-element">
                            <div className="encrypt">
                                Зашифровано
                            </div>
                        </div>
                    ) : null
                }

                {
                    openChatObject.isSigned ? (
                        <div className="control-element">
                            <div className="ecp">
                                Подписано
                            </div>
                        </div>
                    ) : null
                }
            </div>

            <div className="html-content" dangerouslySetInnerHTML={{__html: openChatObject!.htmlContent}}>
            </div>


            {openChatObject.attachments!.length > 0 ? (
                <p className="attachment-name">Вложения:</p>
            ) : null}
            
            <div className="attachments">
                {openChatObject?.attachments?.map((attachment: string, index: number) => (
                    <div key={index} className="attachment"
                         onClick={() => handleOpenFile(attachment)}>
                        <p>{attachment}</p>
                    </div>
                ))}
            </div>
        </div>
    </div>
}

export default ChatWorkspace;