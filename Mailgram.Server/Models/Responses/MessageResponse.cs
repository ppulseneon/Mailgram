using Mailgram.Server.Enums;

namespace Mailgram.Server.Models.Responses;

public class MessageResponse(Message message)
{
    public uint Id { get; set; } = message.Id;
    public string From { get; set; } = message.From;
    public string To { get; set; } = message.To;
    public string Subject { get; set; } = message.Subject;
    public string HtmlContent { get; set; } = message.HtmlContent;
    public DateTime Date { get; set; } = message.Date;
    public Folders Folder { get; set; } = message.Folder;
    public List<string> Attachments { get; set; } = message.AttachmentFiles;
}