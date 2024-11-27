using Mailgram.Server.Enums;

namespace Mailgram.Server.Models;

public class Contact
{
    /// <summary>
    /// Почтовый адрес
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Текущий статус (принят, отклонен и т.д.)
    /// </summary>
    public ExchangeStatus Status { get; set; }
}

