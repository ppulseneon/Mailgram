namespace Mailgram.Server.Models;

public class Account
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public required string Login { get; init; }
    
    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; init; }
    
    /// <summary>
    /// Название платформы (Yandex, Rambler и т.д.) 
    /// </summary>
    public required string Platform { get; init; }
    
    /// <summary>
    /// Данные авторизации для SMTP сервера
    /// </summary>
    public ConnectionCredentials SmtpCredentials { get; set; }
    
    /// <summary>
    /// Данные авторизации для IMAP сервера
    /// </summary>
    public ConnectionCredentials ImapCredentials { get; set; }
}
