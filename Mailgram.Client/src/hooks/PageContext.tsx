import React, {createContext, ReactNode, useState} from "react";
import { PagesList } from '../enums/PagesList';

interface PageContextType {
  page: PagesList;
  setPage: (folder: PagesList) => void;
}

const PageContext = createContext<PageContextType>({
  page: PagesList.Initial,
  setPage: () => { }
});

const PageProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [page, setPage] = useState(PagesList.Initial);

  return (
      <PageContext.Provider value={{ page, setPage }}>
        {children}
      </PageContext.Provider>
  );
};

export { PageProvider, PageContext };