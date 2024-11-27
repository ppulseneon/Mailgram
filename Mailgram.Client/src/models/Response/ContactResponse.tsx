import {ContactsStatuses} from "../Enums/ContactsStatuses.tsx";

class ContactResponse {
    email: string
    status: ContactsStatuses

    constructor(params?: {
        email: string;
        status: ContactsStatuses;
    }) {
        this.email = params!.email;
        this.status = params!.status;
    }
}

export default ContactResponse;