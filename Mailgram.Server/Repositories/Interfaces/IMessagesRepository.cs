using System.Net.Mail;
using Mailgram.Server.Models;

namespace Mailgram.Server.Repositories.Interfaces;

public interface IMessagesRepository
{
    Task SaveMessage(Guid userId, Message message);
    Task SaveMessages(Guid userId, List<Message> messages);
    Task SaveAttachment(Guid userId, uint messageId, string filename, byte[] attachment);
    Task<List<Message>> GetMessages(Guid userId);
    Task<Message?> GetMessage(Guid userId, uint messageId);
    Task<string> GetMessageAttachmentPath(Guid userId, uint messageId, string attachmentName);
}