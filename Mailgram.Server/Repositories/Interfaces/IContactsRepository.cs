using Mailgram.Server.Models;

namespace Mailgram.Server.Repositories.Interfaces;

public interface IContactsRepository
{
    Task<List<Contact>> GetContactsAsync(Guid userId);
    Task SaveContact(Guid userId, Contact contact);
    Task<(string, string)> GenerateContactKeys(Guid id, string email);
    Task ImportContactKeys(Guid userId, string email, string publicRsaKey, string publicEcpKey);
    (string, string) GetEncryptKeysPaths(Guid userId, string email);
    (string, string) GetDecryptKeysPaths(Guid userId, string email);
}