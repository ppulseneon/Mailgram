import {UUID} from "node:crypto";

interface Account {
    id: UUID;   
    login: string;
    platform: string;
}

export default Account;