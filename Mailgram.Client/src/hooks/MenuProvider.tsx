import React, {createContext, ReactNode, useState} from "react";

interface MenuContextType {
    isVisible: boolean;
    setVisibility: () => void;
}

const MenuContext = createContext<MenuContextType>({
    isVisible: false,
    setVisibility: () => {}
});

const MenuProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [isVisible, setIsVisible] = useState(false);

    const toggleVisibility = () => {
        setIsVisible(prevIsVisible => !prevIsVisible);
    };
    
    return (
        <MenuContext.Provider value={{ isVisible, setVisibility: toggleVisibility }}>
            {children}
        </MenuContext.Provider>
    );
};

export { MenuProvider, MenuContext };