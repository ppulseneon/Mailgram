using Mailgram.Server.Enums;

namespace Mailgram.Server.Models;

public class Draft
{
    public string? EmailTo { get; set; }
    public string Content { get; set; } = string.Empty;
    public Folders Folder { get; set; } = Folders.Drafts;
    public List<string> AttachmentFiles { get; set; } = [];
}