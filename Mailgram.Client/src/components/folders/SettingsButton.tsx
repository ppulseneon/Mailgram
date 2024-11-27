import { IoMenu } from "react-icons/io5";
import '../../assets/css/Folders.css'
import {ReactElement, useContext} from "react";
import {MenuContext} from "../../hooks/MenuProvider.tsx";

function SettingsButton(): ReactElement {
    const {setVisibility} = useContext(MenuContext);
    
    const handleSettingsClick = () => {
        setVisibility();
    };

    return <div className="folder" onClick={handleSettingsClick}>
            <span className="settings-icon"><IoMenu/></span>
    </div>
}

export default SettingsButton;