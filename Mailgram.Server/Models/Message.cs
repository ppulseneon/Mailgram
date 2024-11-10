using Mailgram.Server.Enums;
using MimeKit;

namespace Mailgram.Server.Models;

public class Message
{
    public uint Id { get; set; }
    public bool IsEncrypted { get; set; } = false;
    public bool IsSigned { get; set; } = false;
    public bool IsSeen { get; set; } = false;
    public required string From { get; set; } 
    public required string To { get; set; }
    public required string Subject { get; set; }
    public required string HtmlContent { get; set; }
    public DateTime Date { get; set; }
    public Folders Folder { get; set; }
    public List<string> AttachmentFiles { get; set; } = [];
}