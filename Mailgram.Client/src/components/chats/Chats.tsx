import Chat from './Chat';
import '../../assets/css/chats/Chats.css';
import {useContext, useEffect, useState} from "react";
import {ChatContext} from "../../hooks/ChatProvider.tsx";
import {FolderContext} from "../../hooks/FolderContext.tsx";
import {FoldersList} from "../../enums/FoldersList.tsx";
import {Folders} from "../../models/Enums/Folders.tsx";

function Chats(): JSX.Element {
    const [lastFolder, setLastFolder] = useState(1)
    const {chats} = useContext(ChatContext);
    const {folder} = useContext(FolderContext);
    
    useEffect(() => {
        if (folder != FoldersList.SendButton && folder != FoldersList.Contacts){
            setLastFolder(folder);   
        }
    }, [folder]);
    
    return <div className="chats-container">
        {chats.length > 0 && chats.map((chat, index) => {
            if (folder !== FoldersList.SendButton && folder !== FoldersList.Contacts) {
                if (folder === FoldersList.Received && (chat.folder === Folders.Incoming || chat.folder === Folders.Favorites)) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (folder === FoldersList.Sent && chat.folder === Folders.Sent) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (folder === FoldersList.Favorite && chat.folder === Folders.Favorites) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (folder === FoldersList.Deleted && chat.folder === Folders.Deleted) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (folder === FoldersList.Drafted && chat.folder === Folders.Drafts) {
                    return <Chat key={index} id={chat.id!} />;
                }
            } else {
                if (lastFolder === FoldersList.Received && (chat.folder === Folders.Incoming || chat.folder === Folders.Favorites)) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (lastFolder === FoldersList.Sent && chat.folder === Folders.Sent) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (lastFolder === FoldersList.Favorite && chat.folder === Folders.Favorites) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (lastFolder === FoldersList.Deleted && chat.folder === Folders.Deleted) {
                    return <Chat key={index} id={chat.id!} />;
                }

                if (lastFolder === FoldersList.Drafted && chat.folder === Folders.Drafts) {
                    return <Chat key={index} id={chat.id!} />;
                }
            }
            return null;
        })}
    </div>
}

export default Chats;