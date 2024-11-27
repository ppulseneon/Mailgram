import Empty from "./Empty.tsx";
import {useContext} from "react";
import {ChatContext} from "../../hooks/ChatProvider.tsx";
import {FolderContext} from "../../hooks/FolderContext.tsx";
import {FoldersList} from "../../enums/FoldersList.tsx";
import ChatWorkspace from "./ChatWorkspace.tsx";
import SentWorkspace from "./SentWorkspace.tsx";
import ContactsWorkspace from "./ContactsWorkspace.tsx";

function Workspace(): JSX.Element {
    const {openChat} = useContext(ChatContext);
    const {folder} = useContext(FolderContext);

    // рендер письма
    if (folder == FoldersList.SendButton){
        return <SentWorkspace/>
    }

    if (folder == FoldersList.Contacts){
        return <ContactsWorkspace/>
    }
    
    // init case
    if(openChat == null) {
        return <Empty />
    }
    
    return <ChatWorkspace/>;
}

export default Workspace;