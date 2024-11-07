import '../../assets/css/chats/Chats.css';

import ChatImage from "./ChatImage.tsx";
import ChatContent from "./ChatContent.tsx";
import ChatInfo from "./ChatInfo.tsx";

function Chat(): JSX.Element {
    return <div className="chat">
        <ChatImage chatName={"Qilana SYablonko"}/>
        <ChatContent/>
        <ChatInfo/>
    </div>
}

export default Chat;