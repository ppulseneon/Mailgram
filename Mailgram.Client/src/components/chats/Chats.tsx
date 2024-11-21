import Chat from './Chat';
import '../../assets/css/chats/Chats.css';
import {useContext, useEffect, useState} from "react";
import {ChatContext} from "../../hooks/ChatProvider.tsx";
import MessageResponse from "../../models/Response/MessageResponse.tsx";
import {FolderContext} from "../../hooks/FolderContext.tsx";
import {FoldersList} from "../../enums/FoldersList.tsx";
import {Folders} from "../../models/Enums/Folders.tsx";

function Chats(): JSX.Element {
    const [lastFolder, setLastFolder] = useState(1)
    const {chats} = useContext(ChatContext);
    const {folder} = useContext(FolderContext);
    
    useEffect(() => {
        if (folder != FoldersList.SendButton){
            setLastFolder(folder);   
        }
    }, [folder]);
    
    return <div className="chats-container">
        {
            chats.map((chat: MessageResponse, index) => {
                if (folder != FoldersList.SendButton) {
                    if (folder == FoldersList.Received) {
                        if (chat.folder == Folders.Incoming || chat.folder == Folders.Favorites) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (folder == FoldersList.Sent) {
                        if (chat.folder == Folders.Sent) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (folder == FoldersList.Sent) {
                        if (chat.folder == Folders.Sent) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (folder == FoldersList.Favorite) {
                        if (chat.folder == Folders.Favorites) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (folder == FoldersList.Deleted) {
                        if (chat.folder == Folders.Deleted) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (folder == FoldersList.Drafted) {
                        if (chat.folder == Folders.Drafts) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }
                }
                else{
                    if (lastFolder == FoldersList.Received) {
                        if (chat.folder == Folders.Incoming || chat.folder == Folders.Favorites) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (lastFolder == FoldersList.Sent) {
                        if (chat.folder == Folders.Sent) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (lastFolder == FoldersList.Favorite) {
                        if (chat.folder == Folders.Favorites) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (lastFolder == FoldersList.Deleted) {
                        if (chat.folder == Folders.Deleted) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }

                    if (lastFolder == FoldersList.Drafted) {
                        if (chat.folder == Folders.Drafts) {
                            return <Chat key={index} id={chat.id!}/>
                        }
                    }
                }
            })
        }
    </div>
}

export default Chats;