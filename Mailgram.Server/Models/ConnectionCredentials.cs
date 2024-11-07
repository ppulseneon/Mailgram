namespace Mailgram.Server.Models;

public class ConnectionCredentials
{
    public required string Hostname { get; init; }
    public int Port { get; init; } = 456;
}