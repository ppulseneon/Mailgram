import './assets/css/Main.css'

import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import {PageProvider} from './hooks/PageContext'
import {FolderProvider} from './hooks/FolderContext'
import {MenuProvider} from "./hooks/MenuProvider.tsx";

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <PageProvider>
            <MenuProvider>
                <FolderProvider>
                    {/*<ChatProvider>*/}
                        <App/>
                    {/*</ChatProvider>*/}
                </FolderProvider>
            </MenuProvider>
        </PageProvider>
    </React.StrictMode>
)