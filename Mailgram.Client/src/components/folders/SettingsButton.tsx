import { IoMenu } from "react-icons/io5";
import '../../assets/css/Folders.css'

function SettingsButton(): JSX.Element {
    const handleSettingsClick = () => {
        alert('Settings button clicked');
      };

    return <div className="folder" onClick={handleSettingsClick}>
            <span className="settings-icon"><IoMenu/></span>
    </div>
}

export default SettingsButton;