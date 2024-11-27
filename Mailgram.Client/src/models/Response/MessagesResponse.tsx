import MessageResponse from "./MessageResponse.tsx";

interface MessagesResponse {
    status: boolean,
    errorMessage?: string,
    messages?: MessageResponse[];
    count?: number;
}

export default MessagesResponse;