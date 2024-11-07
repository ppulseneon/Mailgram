import {UUID} from "node:crypto";

class AccountResponse {
    status?: boolean;
    errorMessage?: string;
    id?: UUID;
    login?: string;
    platform?: string;

    constructor(params?: {
        status?: boolean;
        errorMessage?: string;
        id?: UUID;
        login?: string;
        platform?: string;
    }) {
        this.status = params?.status;
        this.errorMessage = params?.errorMessage;
        this.id = params?.id;
        this.login = params?.login;
        this.platform = params?.platform;
    }
}

export default AccountResponse;