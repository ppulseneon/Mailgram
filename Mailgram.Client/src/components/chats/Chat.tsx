import '../../assets/css/chats/Chats.css';

import ChatImage from "./ChatImage.tsx";
import ChatContent from "./ChatContent.tsx";
import ChatInfo from "./ChatInfo.tsx";
import {useContext} from "react";
import {ChatContext} from "../../hooks/ChatProvider.tsx";
import {FolderContext} from "../../hooks/FolderContext.tsx";
import {FoldersList} from "../../enums/FoldersList.tsx";
import {Folders} from "../../models/Enums/Folders.tsx";

interface ParsedEmail {
    name: string | null;
    email: string;
}

function parseEmail(input: string): ParsedEmail {
    const emailRegex = /<(.*?)>/;
    const emailMatch = input.match(emailRegex);

    if (!emailMatch) {
        // Если email не найден в формате <...>, то вся строка - email, имя - null
        return { name: null, email: input.trim() };
    }

    const email = emailMatch[1].trim();
    
    let namePart = input.substring(0, input.indexOf('<')).trim();

    // Удаляем кавычки из имени
    namePart = namePart.replace(/"/g, '');

    const name = namePart.length > 0 ? namePart : null;
    
    return { name, email };
}

function extractTextFromHTML(html: string): string {
    if (typeof window !== 'undefined') { 
        const parser = new DOMParser();
        const doc = parser.parseFromString(html, 'text/html');
        return doc.body.textContent || '';
    } else {
        return html.replace(/<[^>]*>/g, '');
    }
}

function Chat({id}: {id: number}): JSX.Element {
    const {chats, setOpenChat, openChat} = useContext(ChatContext);
    const {folder, setFolder} = useContext(FolderContext);
    
    const chat = chats.find(i => i.id === id);
    
    const isSelect = chat!.id! == openChat;
    
    const subjectIfHtmlText = extractTextFromHTML(chat!.htmlContent!) == "" ? "<Без сообщения>" : extractTextFromHTML(chat!.htmlContent!)
    
    let parsedChatname = chat!.folder != Folders.Sent ? parseEmail(chat!.from!) : parseEmail(chat!.to!);
    const chatname = parsedChatname!.name == "" || parsedChatname!.name == null ? parsedChatname!.email : parsedChatname!.name;
    
    return <div className={isSelect ? "chat-active" : "chat"} onClick={()=>{
        setOpenChat(chat!.id!)
        
        if (folder == FoldersList.SendButton) {
            if (chat!.folder == Folders.Incoming) {
                setFolder(FoldersList.Received);
            }
        }
    }}>
        <ChatImage chatName={parseEmail(chat!.from!).name} email={parseEmail(chat!.from!).email}/>
        <ChatContent chatname={chatname} chatcontent={chat!.subject == "" || chat!.subject == null ? subjectIfHtmlText : chat!.subject}/>
        <ChatInfo date={chat!.date!} isStar={chat!.folder == Folders.Favorites}/>
    </div>
}

export default Chat;