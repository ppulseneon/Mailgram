import AppSettings from "../models/Constants/AppSettings.tsx";
import MessagesResponse from "../models/Response/MessagesResponse.tsx";

class MessagesService {
    private baseUrl = AppSettings.ApiHost + '/api/email';

    public async syncMessages(userId: string) {
        const url = `${this.baseUrl}/sync?id=${userId}`;

        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        };
        
        await fetch(url, options);
        return
    }
    
    public async getMessages(userId: string) {
        const url = `${this.baseUrl}?id=${userId}`;
        const response = await fetch(url);
        const jsonData: MessagesResponse = await response.json();
        return jsonData.messages;
    }
    
    public async deleteMessage(userId: string, messageId: number) {
        const url = `${this.baseUrl}?userId=${userId}&messageId=${messageId}`;

        const options = {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        };
        
        await fetch(url, options);
    }
    
    public async changeStar(userId: string, messageId: number) {
        const url = `${this.baseUrl}/changeStarred?userId=${userId}&messageId=${messageId}`;

        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        };

        await fetch(url, options);
    }
}

export default MessagesService