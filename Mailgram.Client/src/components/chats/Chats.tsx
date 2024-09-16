import Chat from './Chat';
import '../../assets/css/Chats.css';

function Chats(): JSX.Element {
    return <div className="chats-container">
        <Chat/>
        <Chat/>
        <Chat/>
    </div>
}

export default Chats;