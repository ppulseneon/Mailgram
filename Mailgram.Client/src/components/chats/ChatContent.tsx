import '../../assets/css/chats/ChatContent.css';

function ChatContent({chatname, chatcontent}: {chatname: string; chatcontent: string}) {
    return <div className="chat-content">
        <h1 className="chat-content-name" >
            {chatname}
        </h1>
        <p className="chat-content-topic">
            {chatcontent}
        </p>
    </div>    
}

export default ChatContent;