using Mailgram.Server.Models;

namespace Mailgram.Server.Repositories.Interfaces;

public interface IMessagesRepository
{
    Task AddMessageAsync(Guid userId, Message message);
    List<Message> GetMessages(Guid userId);
    string GetMessageAttachmentPath(Guid userId, Guid messageId); // return decrypted file path in user downloads
}