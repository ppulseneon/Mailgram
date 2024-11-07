using Mailgram.Server.Models;

namespace Mailgram.Server.Services.Interface;

public interface IEmailReaderService
{
    Task LoadEmailsAsync(Account account);
}