import {Folders} from "../Enums/Folders.tsx";

class MessageResponse {
    id?: number;
    from?: string;
    to?: string;
    subject?: string;
    htmlContent?: string;
    date?: Date;
    folder?: Folders;
    attachments?: string[];
    isSigned?: boolean;
    isEnrypted?: boolean;
    isSignedRight?: boolean;
    isEncryptedRight?: boolean;
    status?: boolean;    
    errorMessage?: string;    
    
    constructor(message?: {
        status?: boolean;
        errorMessage?: string;
        id?: number;
        from?: string;
        to?: string;
        subject?: string;
        htmlContent?: string;
        date?: Date;
        folder?: Folders;
        attachments?: string[];
        isSigned?: boolean;
        isEncrypted?: boolean;
        isSignedRight?: boolean;
        isEncryptedRight?: boolean;
    }) {
            this.status = message?.status;
            this.errorMessage = message?.errorMessage;
            this.id = message?.id;
            this.from = message?.from;
            this.to = message?.to;
            this.subject = message?.subject;
            this.htmlContent = message?.htmlContent;
            this.date = message?.date;
            this.folder = message?.folder;
            this.attachments = message?.attachments;
            this.isSigned = message?.isSigned;
            this.isEncrypted = message?.isEncrypted;
            this.isSignedRight = message?.isSignedRight;
            this.isEncryptedRight = message?.isEncryptedRight;
    }
}

export default MessageResponse;