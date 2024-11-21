import '../../assets/css/chats/ChatInfo.css'

// import { FaRegCircle } from "react-icons/fa6";
import { FaStar } from "react-icons/fa";
// import { FaRegCircle } from "react-icons/fa";
// import { FaRegStar } from "react-icons/fa";
function ChatInfo({date, isStar}: {date: Date, isStar: boolean}) {
    function formatDate(date: Date): string {
        if (!(date instanceof Date)) {
            return "Invalid date"; // Or throw an error, depending on your needs
        }

        const day = date.getDate();
        const monthNames = [
            "янв", "фев", "мар", "апр", "мая", "июн",
            "июл", "авг", "сен", "окт", "ноя", "дек"
        ];
        const month = monthNames[date.getMonth()];

        return `${day} ${month}`;
    }
    
    return <div className="chat-info">
        <div className="chat-info-date">
            {formatDate(new Date(date))}
        </div>
        <div className="chat-info-buttons">
            <div className="chat-info-button-active">
                {/*<FaCircle size={16}/>*/}
            </div>
            {isStar ? (
                <div className="chat-info-button-active">
                    <FaStar />
                </div>
            ) : null}
        </div>
    </div>
}

export default ChatInfo;