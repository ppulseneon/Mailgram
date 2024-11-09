using Mailgram.Server.Models;

namespace Mailgram.Server.Services.Interface;

public interface IEmailReaderService
{
    Task SyncAsync(Account account);
    Task<List<Message>> GetAll(Guid userId);
}