import '../../assets/css/chats/ChatImage.css'

// Список цветов градиетов
const gradients = [
    { start: '#FF3838', end: '#6C1A1A' },
    { start: '#21D63E', end: '#0F5816' },
    { start: '#4F94FC', end: '#173F7A' },
    { start: '#9262FA', end: '#360872' },
    { start: '#F545C4', end: '#67124F' },
];

interface ChatImageProps {
    chatName: string;
}

function getFirstLettersOrChars(inputString: string): string {
    // Разделить строку на слова
    const words = inputString.split(' ');

    // Проверить количество слов
    if (words.length >= 2) {
        // Если слов два или больше, взять первые буквы первого и второго слов
        return words[0][0] + words[1][0];
    } else {
        // Если слово одно, взять первые две буквы
        return words[0].substring(0, 2);
    }
}

function ChatImage({ chatName }: ChatImageProps) {
    
    // Получаем первый символ названия чата
    const firstLetter = chatName.charAt(0);

    // Считаем номер цвета
    const index = firstLetter.charCodeAt(0) % gradients.length;

    // Получаем цвет из коллекции
    const gradient = gradients[index];
    
    const displayName = getFirstLettersOrChars(chatName);
    
    return <div className="chat-image" style={{
            background: `linear-gradient(180deg, ${gradient.start} 0%, ${gradient.end} 100%)`,
        }}>
        <p>{displayName}</p>
    </div>
}

export default ChatImage;