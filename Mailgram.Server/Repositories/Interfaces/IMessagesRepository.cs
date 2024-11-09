using Mailgram.Server.Models;

namespace Mailgram.Server.Repositories.Interfaces;

public interface IMessagesRepository
{
    Task SaveMessage(Guid userId, Message message);
    Task SaveMessages(Guid userId, List<Message> messages);
    Task<List<Message>> GetMessages(Guid userId);
    string GetMessageAttachmentPath(Guid userId, Guid messageId); // return decrypted file path in user downloads
}