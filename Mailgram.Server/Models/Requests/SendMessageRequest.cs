using System.Text.Json.Serialization;

namespace Mailgram.Server.Models.Requests;

public class SendMessageRequest
{
    public Guid UserId { get; set; }
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public required string Message { get; set; } = string.Empty;
    public bool IsEncrypt { get; set; } = false;
    public bool IsSign { get; set; } = false;
    public List<IFormFile>? Attachments { get; set; } 
    
    [JsonIgnore]
    public List<string> EncryptedAttachments { get; set; } = [];
}