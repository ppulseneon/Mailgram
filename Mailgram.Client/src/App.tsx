import {ReactElement, useContext, useEffect, useState} from "react";
import {PageContext} from "./hooks/PageContext.tsx";
import {PagesList} from "./enums/PagesList.tsx";
import MainPage from "./pages/Main.tsx";
import AccountsPage from "./pages/Accounts.tsx";
import MessagesService from "./services/MessagesService.tsx";
import {ChatContext} from "./hooks/ChatProvider.tsx";

function App() {
    const {page} = useContext(PageContext);
    const [pageElement, setPageElement] = useState<ReactElement | null>(null);
    const {setChats} = useContext(ChatContext);
    
    let intervalId: string | number | NodeJS.Timeout | null | undefined;
    
    // Обработка загрузки приложения
    useEffect(() => {
        
        // Элемент страницы для рендера
        let element;
        
        // Если приложение только запустили
        if (page === PagesList.Initial){
            
            // Получаем текущий аккаунт
            const accountId = localStorage.getItem('accountId');
                
            // Устанавливаем страницу
            element = accountId ? <MainPage/> : <AccountsPage/>;
        }
        
        // Если пользователь меняет страницу
        else {
            switch (page) {
                case PagesList.Main:
                    element = <MainPage/>;
                    break;
                case PagesList.Accounts:
                    element = <AccountsPage/>;
                    break;
                default:
                    element = null;
            }
        }
        
        // Устанавливаем страницу
        setPageElement(element);
    }, [page]);

    // Обновления чатов
    useEffect(() => {
        const accountId = localStorage.getItem('accountId');

        if (accountId && (page === PagesList.Main || page === PagesList.Initial)) {
            const messagesService = new MessagesService();
            
            intervalId = setInterval(() => {
                const syncMail = async () => {
                    console.log("Синхронизируем почту");
                    await messagesService.syncMessages(accountId);
                };

                syncMail().then();
            }, 10000);

            intervalId = setInterval(() => {
                const syncMail = async () => {
                    console.log("Обновляем сообщения");
                    const messages = await messagesService.getMessages(accountId);

                    if (messages != undefined){
                        setChats(messages);
                    }
                };

                syncMail().then();
            }, 100);
        }

        return () => { // Cleanup
            if (intervalId) {
                clearInterval(intervalId);
                intervalId = null;
            }
        };
    }, [page, localStorage.getItem('accountId')]);
    

    return pageElement;
}

export default App;

