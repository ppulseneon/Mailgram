import Chats from "./components/chats/Chats";
import Folders from "./components/folders/Folders";
import Workspace from "./components/workspace/Workspace";

function App() {
    return (
        <>
            <div className="container">
                <Folders />
                <Chats />
                <Workspace />
            </div>
        </>
    )
}

export default App;