using Mailgram.Server.Models;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interfaces;
using Mailgram.Server.Utility;
using Newtonsoft.Json;

namespace Mailgram.Server.Repositories;

public class ContactsRepository(IEncryptService encryptService): IContactsRepository
{
    public async Task SaveContact(Guid userId, Contact contact)
    {
        // Получаем путь к папке контактов
        var userContactsDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "contacts");
        
        // Получаем путь к контакту
        var contactDirectory = Path.Combine(userContactsDirectory, Guid.NewGuid().ToString());
        
        // Получаем путь к сообщению
        var contactFilePath = Path.Combine(contactDirectory, ".contact");
        
        // Создаем путь к сообщению
        Directory.CreateDirectory(contactFilePath);
        
        // Сериализируем сообщение
        var jsonMessage = JsonConvert.SerializeObject(contact);
            
        // Сохраняем сообщение с системными ключами
        await AppData.SaveEncryptedFile(jsonMessage, contactFilePath);
    }
    
    public async Task<List<Contact>> GetContactsAsync(Guid userId)
    {
        var contacts = new List<Contact>();
        
        var userContactsDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "contacts");
        
        var contactsFolders = Directory.GetDirectories(userContactsDirectory);

        foreach (var contactFolder in contactsFolders)
        {
            var contactFile = Path.Combine(contactFolder, ".contact");
            contacts.Add(await AppData.ReadEncryptedFile<Contact>(contactFile));
        }

        return contacts;
    }

    public async Task<(string, string)> GenerateContactKeys(Guid userId)
    {
        // Получаем путь к папке контактов
        var userContactsDirectory = Path.Combine(AppData.GetAppDataDirectory(), userId.ToString(), "contacts");
        
        // Получаем путь к контакту
        var contactDirectory = Path.Combine(userContactsDirectory, Guid.NewGuid().ToString());
        
        var publicRsa = await encryptService.GenerateRsa(contactDirectory, ".prsa");
        var publicEcp = await encryptService.GenerateRsa(contactDirectory, ".pecp");
        
        return (publicRsa, publicEcp);
    }

    public async Task ImportContactKeys()
    {
        
    }
}