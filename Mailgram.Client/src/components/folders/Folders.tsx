import FolderButton from './FolderButton'
import '../../assets/css/Folders.css'
import SettingsButton from './SettingsButton'

import { IoSend } from "react-icons/io5";
import { RiFolderSharedFill } from "react-icons/ri";
import { RiFolderReceivedFill } from "react-icons/ri";
import { IoBookmarkSharp } from "react-icons/io5";
import { FaDraftingCompass } from "react-icons/fa";
import { MdDelete } from "react-icons/md";
import { useContext } from 'react';
import { FolderContext } from '../../hooks/FolderContext';
import { FoldersList } from '../../enums/FoldersList';

function Folders(): JSX.Element {
    // todo: переписать под https://kaneru.me/react-scrollable-container/

    const { folder, setFolder } = useContext(FolderContext);

    const handleSendButtonClick = () => {
        setFolder(FoldersList.SendButton);
    };

    const handleReceivedFolderClick = () => {
        setFolder(FoldersList.Received);
    }

    const handleFavoriteFolderClick = () => {
        setFolder(FoldersList.Favorite);
    }

    const handleSendedFolderClick = () => {
        setFolder(FoldersList.Sended);
    }

    const handleDeletedFolderClick = () => {
        setFolder(FoldersList.Deleted);
    }

    const handleDraftedFolderClick = () => {
        setFolder(FoldersList.Drafted);
    }

    return <div className="folders-container">
        <div>
            <SettingsButton />
            <FolderButton icon={<IoSend
                className={folder === FoldersList.SendButton ? "folder-icon-active" : "folder-icon"}/>}
                text="Написать"
                onClick={handleSendButtonClick}
                isActive={folder === FoldersList.SendButton} />

            <FolderButton icon={<RiFolderReceivedFill 
                className={folder === FoldersList.Received ? "folder-icon-active" : "folder-icon"}/>}
                text="Входящие" onClick={handleReceivedFolderClick}
                isActive={folder === FoldersList.Received} />

            <FolderButton icon={<RiFolderSharedFill
                className={folder === FoldersList.Sended ? "folder-icon-active" : "folder-icon"}/>}
                text="Отправленные" onClick={handleSendedFolderClick}
                isActive={folder === FoldersList.Sended} />

            <FolderButton icon={<IoBookmarkSharp
                className={folder === FoldersList.Favorite ? "folder-icon-active" : "folder-icon"}/>}
                text="Избранные" onClick={handleFavoriteFolderClick}
                isActive={folder === FoldersList.Favorite} />

            <FolderButton icon={<MdDelete
                className={folder === FoldersList.Deleted ? "folder-icon-active" : "folder-icon"}/>}
                text="Удаленные" onClick={handleDeletedFolderClick}
                isActive={folder === FoldersList.Deleted} />

            <FolderButton icon={<FaDraftingCompass
                className={folder === FoldersList.Drafted ? "folder-icon-active" : "folder-icon"}/>}
                text="Черновики" onClick={handleDraftedFolderClick}
                isActive={folder === FoldersList.Drafted} />
        </div>
    </div>
}

export default Folders