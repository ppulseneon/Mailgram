import React from "react";

interface FolderButtonProps {
    icon: React.ReactNode;
    text: string;
    onClick?: () => void;
    isActive: boolean;
}

export default FolderButtonProps; 