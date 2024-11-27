using Mailgram.Server.Enums;

namespace Mailgram.Server.Models;

public class ContactSwap
{
    public string From { get; set; }
    public SwapStatus SwapStatus { get; set; }
    public string PublicRsa  { get; set; }
    public string PublicEcp  { get; set; }
}