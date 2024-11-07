import '../../assets/css/chats/ChatInfo.css'

// import { FaRegCircle } from "react-icons/fa6";
import { FaCircle } from "react-icons/fa";
import { FaStar } from "react-icons/fa";
// import { FaRegCircle } from "react-icons/fa";
// import { FaRegStar } from "react-icons/fa";

function ChatInfo() {
    return <div className="chat-info">
        <div className="chat-info-date">
            20 сен
        </div>
        <div className="chat-info-buttons">
            <div className="chat-info-button-active">
                <FaCircle size={16}/>
            </div>

            <div className="chat-info-button">
                <FaStar/>
            </div>
        </div>
    </div>
}

export default ChatInfo;