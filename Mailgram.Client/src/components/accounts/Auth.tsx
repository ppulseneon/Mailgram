import {ReactElement, useContext, useState} from "react";
import '../../assets/css/Auth.css'
import ConnectCredentials from "../../models/ConnectCredentials.tsx";
import emailOptions from "../../models/Constants/EmailOptions.tsx";
import AccountsService from "../../services/AccountsService.tsx";
import AccountRequest from "../../models/Request/AccountRequest.tsx";
import {PagesList} from "../../enums/PagesList.tsx";
import {PageContext} from "../../hooks/PageContext.tsx";
import MessagesService from "../../services/MessagesService.tsx";
import {ChatContext} from "../../hooks/ChatProvider.tsx";

function Auth(): ReactElement {
    const {setPage} = useContext(PageContext);
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [selectedEmailName, setSelectedEmailName] = useState<string | undefined>("");
    const [selectedEmailImap, setSelectedEmailImap] = useState<ConnectCredentials | null>(null);
    const [selectedEmailSmtp, setSelectedEmailSmtp] = useState<ConnectCredentials| null>(null);
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false); // Добавлено состояние загрузки
    const {setChats} = useContext(ChatContext)
    
    const handleEmailChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedName = event.target.value;
        const selectedOption = emailOptions.find((option) => option.name === selectedName);

        setSelectedEmailName(selectedOption?.name);
        setSelectedEmailSmtp(selectedOption?.smtpCredentials!);
        setSelectedEmailImap(selectedOption?.imapCredentials!);
    };

    const handleSubmit = async () => {
        setError("");
        setIsLoading(true); // Начало загрузки

        try { // try...catch для обработки ошибок
            if (!selectedEmailName) {
                setError("Ошибка. Выберите почтовый сервер");
                return;
            }

            if (login === ""){
                setError("Ошибка. Введите логин");
                return;
            }

            if (password === "") {
                setError("Ошибка. Введите пароль");
                return;
            }

            const request = new AccountRequest(login, password, selectedEmailName, selectedEmailImap, selectedEmailSmtp);

            const accountsService = new AccountsService();
            const accountResponse = await accountsService.addAccount(request);

            if (!accountResponse.status) {
                setError(accountResponse.errorMessage!);
                console.log(accountResponse.errorMessage!);
                return;
            }

            localStorage.setItem('accountId', accountResponse!.id!);
            localStorage.setItem('email', selectedEmailName);

            const syncMail = async () => {
                const messagesService = new MessagesService();

                console.log("Синхронизируем почту");
                await messagesService.syncMessages(accountResponse!.id!);

                console.log("Обновляем сообщения");
                const messages = await messagesService.getMessages(accountResponse!.id!);

                if (messages != undefined){
                    setChats(messages);
                }
            };
            
            await syncMail()
            
            setPage(PagesList.Main);
        } finally { 
            setIsLoading(false); 
        }
    }
    
    return (
        <div className="auth-container">
            <h1>Авторизация</h1>

            <div className="input-container">
                <input
                    placeholder="Логин"
                    className="input-field"
                    type="text"
                    value={login}
                    onChange={(e) => setLogin(e.target.value)}/>
                <label htmlFor="input-field" className="input-label">Логин</label>
                <span className="input-highlight"></span>
            </div>

            <div className="input-container">
                <input
                    placeholder="Пароль"
                    className="input-field"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}/>
                <label htmlFor="input-field" className="input-label">Пароль</label>
                <span className="input-highlight"></span>
            </div>

            <div className="email-select">
                <select onChange={handleEmailChange}
                        value={selectedEmailName ? emailOptions.find(opt => opt.name === selectedEmailName)?.name || "" : ""}>
                    <option value="" disabled>Выберите email</option>
                    {emailOptions.map((option) => (
                        <option key={option.name} value={option.name}>
                            {option.name}
                        </option>
                    ))}
                </select>
            </div>
            
            {error && <div className="error-message">{error}</div>}

            <div className="button-container">
                <button type="submit" className="login-button" onClick={handleSubmit} disabled={isLoading}>
                    {isLoading ? "Загрузка..." : "Войти"}
                </button>
            </div>
        </div>
    );
}

export default Auth;