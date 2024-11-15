namespace Mailgram.Server.Models.Responses;

public class MessagesResponse(List<Message> messages)
{
    public IList<MessageResponse> Messages { get; set; } = messages.Select(x => new MessageResponse(x)).ToList();
    public int Count { get; set; } = messages.Count;
}