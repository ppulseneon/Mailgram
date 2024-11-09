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
        await AppData.SaveEncryptedSystemFile(jsonMessage, messageFilePath);
            
        // todo: сохранить и удалить файлы сообщения
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
            messages.Add(await AppData.ReadEncryptedSystemFile<Message>(messageFile));
        }

        return messages;
    }

    public string GetMessageAttachmentPath(Guid userId, Guid messageId)
    {
        throw new NotImplementedException();
    }
}