namespace Mailgram.Server.Models.Requests;

public class AddDraftRequest
{
    public Guid UserId { get; set; }
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public required string Message { get; set; } = string.Empty;
}