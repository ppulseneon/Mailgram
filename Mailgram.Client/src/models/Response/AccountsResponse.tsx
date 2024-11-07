import Account from "../Account.tsx";

interface AccountsResponse {
    status: boolean,
    errorMessage?: string,
    accounts?: Account[];
    count?: number;
}

export default AccountsResponse;