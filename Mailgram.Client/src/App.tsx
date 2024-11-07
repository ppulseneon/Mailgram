import {ReactElement, useContext, useEffect, useState} from "react";
import {PageContext} from "./hooks/PageContext.tsx";
import {PagesList} from "./enums/PagesList.tsx";
import MainPage from "./pages/Main.tsx";
import AccountsPage from "./pages/Accounts.tsx";

// import AccountResponse from "./models/Response/AccountResponse";

function App() {
    // const [setAccounts] = useState<AccountResponse[]>([]);
    const {page} = useContext(PageContext);
    const [pageElement, setPageElement] = useState<ReactElement | null>(null);

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

    // useEffect(() => {
    //     const fetchAccounts = async () => {
    //         const accountsService = new AccountsService();
    //         const accounts = await accountsService.getAccounts();
    //         // setAccounts(accounts);
    //         console.log(accounts);
    //     };
    //
    //     fetchAccounts();
    // }, []);

    return pageElement;
}

export default App;

