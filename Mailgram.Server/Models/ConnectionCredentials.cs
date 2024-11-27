namespace Mailgram.Server.Models;

public class ConnectionCredentials
{
    /// <summary>
    /// Имя хоста
    /// </summary>
    public required string Hostname { get; init; }
    
    /// <summary>
    /// Порт подключения
    /// </summary>
    public int Port { get; init; } = 456;
}

