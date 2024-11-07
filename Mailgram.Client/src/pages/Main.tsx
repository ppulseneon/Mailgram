import {ReactElement} from "react";
import Chats from "../components/chats/Chats";
import Folders from "../components/folders/Folders";
import MenuPopUp from "../components/menu/MenuPopUp";
import Workspace from "../components/workspace/Workspace";

function MainPage(): ReactElement {
    return (
        <>  
            <div className="container">
                <MenuPopUp/>
                <Folders/>
                <Chats/>
                <Workspace/>
            </div>
        </>
    )
}

export default MainPage;