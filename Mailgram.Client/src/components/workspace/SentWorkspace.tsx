import '../../assets/css/workspaces/Empty.css'

function Empty(): JSX.Element {
    return <div className="empty-message-container">
        <div className="empty-message">
            <p className="empty-text">
                Выберите или напишите новое письмо
            </p>
        </div>
    </div>
}

export default Empty;