import AccountsResponse from "../models/Response/AccountsResponse.tsx";
import AppSettings from "../models/Constants/AppSettings.tsx";
import AccountRequest from "../models/Request/AccountRequest.tsx";
import AccountResponse from "../models/Response/AccountResponse.tsx";

class AccountsService {
    private baseUrl = AppSettings.ApiHost + '/api/accounts';

    // Получить все подключенные аккаунты
    public async getAccounts(){
        const url = this.baseUrl;
        const response = await fetch(url);
        const jsonData: AccountsResponse = await response.json();
        
        return jsonData.accounts;
    }

    // Подключить аккаунт
    public async addAccount(request: AccountRequest ) {
        const url = this.baseUrl;

        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        };

        try {
            const response = await fetch(url, options);
            
            if (!response.ok) {
                const errorText = await response.text();
                console.log(errorText);
                return new AccountResponse({status: false, errorMessage: errorText}); 
            }

            const data = await response.json();
            return new AccountResponse({status: true, id: data.id, login: data.login, platform: data.platform});

        } catch (error) {
            return new AccountResponse({status: false, errorMessage: "Ошибка подключения к серверам"});
        }
    }
}

export default AccountsService