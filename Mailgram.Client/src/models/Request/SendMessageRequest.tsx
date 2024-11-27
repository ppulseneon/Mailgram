interface SendMessageRequest {
    userId: string;
    to: string;
    subject: string;
    message: string;
    isEncrypt: boolean;
    isSign: boolean;
    attachments?: File[];
}

export default SendMessageRequest