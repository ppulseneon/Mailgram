import ConnectCredentials from "../ConnectCredentials.tsx";

class AccountRequest {
    login: string;
    password: string;
    platform: string;
    smtpCredentials: ConnectCredentials | null;
    imapCredentials: ConnectCredentials | null;
    constructor(login: string, password: string, platform: string, imapCredentials: ConnectCredentials | null, smtpCredentials: ConnectCredentials | null) {
        this.login = login;
        this.password = password;
        this.platform = platform;
        this.imapCredentials = imapCredentials;
        this.smtpCredentials = smtpCredentials;
    }
}

export default AccountRequest; 