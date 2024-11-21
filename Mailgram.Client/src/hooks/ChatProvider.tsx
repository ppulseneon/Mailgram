import React, {createContext, Dispatch, ReactNode, SetStateAction, useState} from "react";
import MessageResponse from "../models/Response/MessageResponse.tsx";

interface ChatContextType {
    openChat: number | null;
    chats: MessageResponse[];
    setChats: Dispatch<React.SetStateAction<MessageResponse[]>>
    setOpenChat: Dispatch<React.SetStateAction<number | null>>
}

const ChatContext = createContext<ChatContextType>({
    openChat: null,
    chats: [],
    setChats: () => { },
    setOpenChat: () => { },
});

const ChatProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [openChat, setOpenChat] = useState<number | null>(null);
    const [chats, setChats] = useState<MessageResponse[]>([]);

    return (
        <ChatContext.Provider value={{ openChat, chats, setChats, setOpenChat }}>
            {children}
        </ChatContext.Provider>
    );
};

export { ChatProvider, ChatContext };