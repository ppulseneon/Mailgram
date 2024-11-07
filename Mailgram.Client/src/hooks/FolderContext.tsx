import { createContext, useState } from "react";
import { FoldersList } from '../enums/FoldersList';

interface FolderContextType {
  folder: FoldersList;
  setFolder: (folder: FoldersList) => void;
}

const FolderContext = createContext<FolderContextType>({
  folder: FoldersList.Received,
  setFolder: () => { }
});


const FolderProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [folder, setFolder] = useState(FoldersList.Received);

  return (
    <FolderContext.Provider value={{ folder, setFolder }}>
      {children}
    </FolderContext.Provider>
  );
};

export { FolderProvider, FolderContext };