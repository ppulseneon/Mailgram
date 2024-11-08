using Mailgram.Server.Models;

namespace Mailgram.Server.Services.Interfaces;

public interface IMessagesService
{
    Task<List<Message>> GetMessages(Guid userId);
}