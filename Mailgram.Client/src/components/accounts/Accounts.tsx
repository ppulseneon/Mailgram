import {ReactElement, useEffect, useState} from "react";
import '../../assets/css/Auth.css'
import AccountsService from "../../services/AccountsService.tsx";
import Account from "../../models/Account.tsx";
import AccountElement from "./Account.tsx";

function Accounts(): ReactElement {
    const [accounts, setAccounts] = useState<Account[] | null>(null);

    useEffect(() => {
        const fetchAccounts = async () => {
            const accountsService = new AccountsService();
            const accounts = await accountsService.getAccounts();
            setAccounts(accounts!);
        };

        fetchAccounts().then();
    }, []);
    
    return (
        <div className="accounts-container">
            {accounts && (
                <div className="accounts-list"> {/* Добавляем обертку с прокруткой */}
                    {accounts.map((account) => (
                        <AccountElement
                            key={account.id} // Добавляем key для элементов списка
                            id={account.id}
                            login={account.login}
                            platform={account.platform}
                        />
                    ))}
                </div>
            )}
        </div>
    );
}

export default Accounts;