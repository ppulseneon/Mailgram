using Mailgram.Server.Enums;
using MimeKit;

namespace Mailgram.Server.Models;

public class Message
{
    public uint Id { get; set; }
    public bool IsEncrypted { get; set; } = false;
    public bool IsSigned { get; set; } = false;
    public MimeMessage MimeMessage { get; set; }
    public Folders Folder { get; set; }
    public List<string> AttachmentFiles { get; set; } = [];
}