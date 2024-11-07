import {ReactElement} from "react";
import MenuPointProps from "../../props/components/MenuPointProps.tsx";

function MenuPoint({ icon, text, onClick }: MenuPointProps): ReactElement {
    return <div className="menu-pop-up-container-point" onClick={onClick}>
        <span className="menu-pop-up-container-point-icon">{icon}</span>
        <span className="menu-pop-up-container-point-text">{text}</span>
    </div>
}

export default MenuPoint;