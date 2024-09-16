import FolderButtonProps from '../../props/components/FolderButtonProps'

function FolderButton({ icon, text, onClick, isActive }: FolderButtonProps): JSX.Element {
    return (
      <div className={isActive ? 'folder-active' : 'folder'} onClick={onClick}>
        {icon}
        <span className={isActive ? 'folder-name-active' : 'folder-name'}>{text}</span>
      </div>
    );
  }
  
export default FolderButton;