import {ReactElement} from "react";
import '../assets/css/Accounts.css'
import Auth from "../components/accounts/Auth.tsx";
import Accounts from "../components/accounts/Accounts.tsx";


function AccountsPage(): ReactElement {
    return (
        <>
            <div className="accounts-container">
                <Auth/>
                <Accounts/>
            </div>
        </>
    )
}

export default AccountsPage;