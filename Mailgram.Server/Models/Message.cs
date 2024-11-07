using Mailgram.Server.Enums;
using MimeKit;

namespace Mailgram.Server.Models;

public class Message
{
    public MimeMessage MimeMessage { get; set; }
    public List<Folders> Folders { get; set; }
    public List<string> AttachmentFiles { get; set; } 
}