import {ReactElement, useContext} from "react";
import '../../assets/css/MenuPopUp.css'
import {MenuContext} from "../../hooks/MenuProvider.tsx";
import MenuPoint from "./MenuPoint.tsx";
import {MdLogout} from "react-icons/md";
import {PageContext} from "../../hooks/PageContext.tsx";
import {PagesList} from "../../enums/PagesList.tsx";

function MenuPopUp(): ReactElement {
    const {setPage} = useContext(PageContext);
    const {isVisible, setVisibility} = useContext(MenuContext);

    const handleSwitchAccountClick = () => {
        localStorage.removeItem('accountId');
        setPage(PagesList.Accounts);
        setVisibility();
    };

    const handleMenuClick = (event: React.MouseEvent<HTMLDivElement>) => {
        if (event.target instanceof HTMLElement && event.target.classList.contains("menu-pop-up")) {
            setVisibility();
        }
    };
    
    return <div className="menu-pop-up" style={{visibility: isVisible ? 'visible' : 'hidden'}} onClick={handleMenuClick}>
        <div className="menu-pop-up-container">
            <div className="menu-pop-up-container-header">
                <h1>Настройки</h1>

                <h2>Текущая почта: </h2>
                <p>thebooom@yandex.ru</p>
            </div>

            <hr/>

            <div className="menu-pop-up-container-points">
                <MenuPoint text="Сменить аккаунт" icon={<MdLogout/>} onClick={handleSwitchAccountClick}/>
            </div>
        </div>
    </div>
}

export default MenuPopUp;