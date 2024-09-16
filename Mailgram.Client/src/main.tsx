import './assets/css/Main.css'

import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import { FolderProvider } from './hooks/FolderContext'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <FolderProvider>
      <App />

    </FolderProvider>

  </React.StrictMode>
)
