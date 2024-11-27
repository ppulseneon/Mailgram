import AppSettings from "../models/Constants/AppSettings.tsx";
import MessagesResponse from "../models/Response/MessagesResponse.tsx";
import DraftRequest from "../models/Request/DraftRequest.tsx";
import SendMessageRequest from "../models/Request/SendMessageRequest.tsx";

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
    
    public async getAttachment(userId: string, messageId: number, attachment: string): Promise<string>{
        const url = `${this.baseUrl}/attachment?userId=${userId}&messageId=${messageId}&attachmentName=${attachment}`;

        const options = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        };

        const response = await fetch(url, options);
        return response.text();
    }
    
    public async sendDraft(draftRequest: DraftRequest){
        const url = `${this.baseUrl}/draft`;

        const options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(draftRequest)
        };

        await fetch(url, options);
    }

    public async sendMessageRequest(request: SendMessageRequest){
        const url = new URL(this.baseUrl);

        // Добавляем параметры запроса в URL
        url.searchParams.append('UserId', request.userId);
        url.searchParams.append('To', request.to);
        url.searchParams.append('Subject', request.subject);
        url.searchParams.append('Message', request.message);
        url.searchParams.append('IsEncrypt', String(request.isEncrypt));
        url.searchParams.append('IsSign', String(request.isSign));

        const formData = new FormData();

        // Добавляем вложения в FormData
        if (request.attachments) {
            request.attachments.forEach((file) => {
                formData.append(`Attachments`, file, file.name);
            });
        }

        const options = {
            method: 'POST',
            headers: {
                'accept': 'text/plain',
            },
            body: formData
        };

        try {
            const response = await fetch(url.toString(), options);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            console.log('Success:', data);
        } catch (error) {
            console.error('Error:', error);
        }
    }
}

export default MessagesService