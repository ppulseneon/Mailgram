using Mailgram.Server.Models;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Utility;
using Newtonsoft.Json;

namespace Mailgram.Server.Repositories;

public class MessagesRepository: IMessagesRepository
{
    public async Task SaveMessage(Guid userId, Message message)
    {
        // Получаем путь к папке пользовательских сообщений
        var userMessagesDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "messages");
        
        // Получаем путь к сообщению
        var messageDirectory = Path.Combine(userMessagesDirectory, message.Id.ToString());
        
        // Получаем путь к сообщению
        var messageFilePath = Path.Combine(messageDirectory, ".message");
        
        // Создаем путь к сообщению
        Directory.CreateDirectory(messageDirectory);
        
        // Сериализируем сообщение
        var jsonMessage = JsonConvert.SerializeObject(message);
            
        // Сохраняем сообщение с системными ключами
        await AppData.SaveEncryptedFile(jsonMessage, messageFilePath);
    }
    
    public async Task SaveMessages(Guid userId, List<Message> messages)
    {
        foreach (var message in messages)
        {
            await SaveMessage(userId, message);
        }
    }

    public async Task<List<Message>> GetMessages(Guid userId)
    {
        var messages = new List<Message>();
        
        var userMessagesDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "messages");
        
        var messagesFolders = Directory.GetDirectories(userMessagesDirectory);

        foreach (var messageFolder in messagesFolders)
        {
            var messageFile = Path.Combine(messageFolder, ".message");
            messages.Add(await AppData.ReadEncryptedFile<Message>(messageFile));
        }

        return messages;
    }

    public async Task<Message?> GetMessage(Guid userId, uint messageId)
    {
        var userMessagesDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "messages");
        var messagesFolders = Directory.GetDirectories(userMessagesDirectory);
        var findedMessageFolder = Path.Combine(userMessagesDirectory, messageId.ToString());
        
        foreach (var messageFolder in messagesFolders)
        {
            if (messageFolder != findedMessageFolder) continue;
            
            var messageFile = Path.Combine(messageFolder, ".message");
            var message = await AppData.ReadEncryptedFile<Message>(messageFile);
            return message;
        }

        return null;
    }
    
    public async Task SaveAttachment(Guid userId, uint messageId, string filename, byte[] attachment)
    {
        // Получаем путь к папке пользовательских сообщений
        var userMessagesDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "messages");
        
        // Получаем путь к сообщению
        var messageDirectory = Path.Combine(userMessagesDirectory, messageId.ToString());
        
        // Получаем путь к файлу
        var messageFilePath = $"{messageDirectory}\\{filename}.enc";
        
        // Сохраняем файл
        await AppData.SaveEncryptedFile(attachment, messageFilePath);
    }
    
    public async Task<string> GetMessageAttachmentPath(Guid userId, uint messageId, string attachmentName)
    {
        try
        {
            // Получаем путь к папке пользовательских сообщений
            var userMessagesDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "messages");
            
            // Получаем путь к сообщению
            var messageDirectory = Path.Combine(userMessagesDirectory, messageId.ToString());
            
            // Получаем путь к файлу
            var messageFilePath = $"{messageDirectory}\\{attachmentName}.enc";
            
            // Получаем содержимое файла
            var attachmentData = await AppData.ReadEncryptedFileData(messageFilePath);
            
            // Получаем название файла
            var fileName = attachmentName.Replace(".enc", "");
            
            // Папка для загрузки
            var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            
            // Формируем путь файла
            var decryptedFilePath = Path.Combine(downloadsPath, fileName);
            
            // Записываем в файл расшифрованный файл
            await File.WriteAllBytesAsync(decryptedFilePath, attachmentData);
            
            return decryptedFilePath;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }
}