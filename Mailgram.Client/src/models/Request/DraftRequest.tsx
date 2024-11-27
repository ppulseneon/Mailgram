class DraftRequest {
    userId: string;
    to: string;
    subject: string;
    message: string;
    constructor(userId: string, to: string, subject: string, message: string) {
        this.userId = userId
        this.to = to;
        this.subject = subject;
        this.message = message;
    }
}

export default DraftRequest;