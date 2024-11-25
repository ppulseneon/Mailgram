import {ReactElement, useContext} from "react";
import Account from "../../models/Account.tsx";
import {UUID} from "node:crypto";
import '../../assets/css/Accounts.css'
import {PagesList} from "../../enums/PagesList.tsx";
import {PageContext} from "../../hooks/PageContext.tsx";
import MessagesService from "../../services/MessagesService.tsx";
import {ChatContext} from "../../hooks/ChatProvider.tsx";

function AccountElement(account: Account): ReactElement {
    const {setPage} = useContext(PageContext);
    const {setChats} = useContext(ChatContext);
    const handleAccountClick = (accountId: UUID, accountName: string) => {
        
        // Помечаем активный аккаунт
        localStorage.setItem('accountId', accountId);
        
        // Помечаем активный аккаунт
        localStorage.setItem('accountName', accountName);

        const syncMail = async () => {
            const messagesService = new MessagesService();

            console.log("Синхронизируем почту");
            await messagesService.syncMessages(accountId);

            console.log("Обновляем сообщения");
            const messages = await messagesService.getMessages(accountId);

            if (messages != undefined){
                setChats(messages);
            }
        };

        syncMail().then(() => setPage(PagesList.Main))
    };
    
    return (
        <div className="account-container"
             onClick={() => handleAccountClick(account.id, account.login)}>
            <div className="account-info" style={{ display: 'flex', alignItems: 'center' }}>
                <h1>Войти как <b style={{fontWeight: "bold"}}>{account.login}</b></h1>
                <p className="platform-pill">{account.platform}</p>
            </div>
        </div>
    );
}

export default AccountElement;