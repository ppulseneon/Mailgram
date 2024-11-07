import {ReactElement, useContext} from "react";
import Account from "../../models/Account.tsx";
import {UUID} from "node:crypto";
import '../../assets/css/Accounts.css'
import {PagesList} from "../../enums/PagesList.tsx";
import {PageContext} from "../../hooks/PageContext.tsx";

function AccountElement(account: Account): ReactElement {
    const {setPage} = useContext(PageContext);
    
    const handleAccountClick = (accountId: UUID) => {
        // Помечаем активный аккаунт
        localStorage.setItem('accountId', accountId);
        
        // Открываем приложение
        setPage(PagesList.Main);
    };
    
    return (
        <div className="account-container"
             onClick={() => handleAccountClick(account.id)}>
            <div className="account-info" style={{ display: 'flex', alignItems: 'center' }}>
                <h1>Войти как <b style={{fontWeight: "bold"}}>{account.login}</b></h1>
                <p className="platform-pill">{account.platform}</p>
            </div>

        </div>
    );
}

export default AccountElement;