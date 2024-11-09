namespace Mailgram.Server.Models.Responses;

public class MessagesResponse
{
    public MessagesResponse(List<Message> messages)
    {   
        Count = messages.Count;
        Messages = messages.Select(x => new MessageResponse(x)).ToList();
    }
    
    public IList<MessageResponse> Messages { get; set; }
    public int Count { get; set; }
}