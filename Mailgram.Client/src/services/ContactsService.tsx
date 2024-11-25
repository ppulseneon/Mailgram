import AppSettings from "../models/Constants/AppSettings.tsx";
import ContactsResponse from "../models/Response/ContactsResponse.tsx";

class ContactsService {
    private baseUrl = AppSettings.ApiHost + '/api/contacts';

    public async getContacts(userId: string){
        const url = `${this.baseUrl}?userId=${userId}`;
        const response = await fetch(url);
        const jsonData: ContactsResponse = await response.json();
        
        return jsonData.contacts;
    }

    public async addContact(userId: string, email: string) {
        const url = `${this.baseUrl}?userId=${userId}`;

        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email })
        };

        await fetch(url, options);
    }

    public async acceptContact(userId: string, email: string) {
        const url = `${this.baseUrl}/accept?userId=${userId}&email=${email}`;

        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        };

        await fetch(url, options);
    }
}

export default ContactsService;