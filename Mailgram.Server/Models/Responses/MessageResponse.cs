using Mailgram.Server.Enums;

namespace Mailgram.Server.Models.Responses;

public class MessageResponse
{
    public uint Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; } = "No subject";
    public string HtmlContent { get; set; }
    public DateTime Date { get; set; }
    public Folders Folder { get; set; }
    public List<string> Attachments { get; set; }
}