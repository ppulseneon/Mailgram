using Mailgram.Server.Models;
using Mailgram.Server.Services.Interfaces;

namespace Mailgram.Server.Services;

public class MessagesService: IMessagesService
{
    public async Task<List<Message>> GetMessages(Guid userId)
    {
        // await
        throw new NotImplementedException();
    }
}