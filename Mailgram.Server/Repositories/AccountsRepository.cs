using Mailgram.Server.Constants;
using Mailgram.Server.Models;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Utility;
using Newtonsoft.Json;

namespace Mailgram.Server.Repositories;

public class AccountsRepository: IAccountsRepository
{
    private readonly string _basePath = AppData.GetAppDataDirectory();

    public async Task CreateAccountAsync(Account account)
    {
        account.Id = Guid.NewGuid();
        
        var isCreated = await GetAccountAsync(account.Login);

        if (isCreated != null)
        {
            return;
        }
        
        var accountDirectoryPath = Path.Combine(_basePath, account.Id.ToString());
        var accountFilePath = Path.Combine(accountDirectoryPath, account.Id.ToString());
        
        // Инициализируем базовые папки пользователя
        CreateBaseUserDirectories(accountDirectoryPath);

        var jsonString = JsonConvert.SerializeObject(account);
        await AppData.SaveEncryptedFile(jsonString, accountFilePath);
    }

    public async Task<List<Account>> GetAccountsAsync()
    {
        var accounts = new List<Account>();
        var accountDirectories = Directory.GetDirectories(_basePath);

        foreach (var directory in accountDirectories)
        {
            var fileName = new DirectoryInfo(directory).Name;
            var filePath = Path.Combine(directory, fileName);

            if (!File.Exists(filePath)) continue;
            try
            {
                var account = await AppData.ReadEncryptedFile<Account>(filePath);

                if (account != null)
                {
                    accounts.Add(account);
                }
            }
            catch (JsonException)
            {
                Console.WriteLine($"Error parsing JSON in file: {filePath}");
            }
        }

        return accounts;
    }

    /// <summary>
    /// Получение аккаунта в базе данных по емейлу
    /// </summary>
    /// <param name="email">Адрес почты</param>
    /// <returns>Аккаунт</returns>
    public async Task<Account?> GetAccountAsync(string email)
    {
        // Получаем все аккаунты
        var accounts = await GetAccountsAsync();
        
        // Возвращаем аккаунт с совпадающим логином
        return accounts.FirstOrDefault(a => a.Login == email);
    }
    
    /// <summary>
    /// Получение аккаунта в базе данных по Id
    /// </summary>
    /// <param name="id">Уникальный идентификатор</param>
    /// <returns>Аккаунт</returns>
    public async Task<Account?> GetAccountAsync(Guid id)
    {
        // Получаем все аккаунты
        var accounts = await GetAccountsAsync();
        
        // Возвращаем аккаунт с совпадающим логином
        return accounts.FirstOrDefault(a => a.Id == id);
    }

    private static void CreateBaseUserDirectories(string accountDirectoryPath)
    {
        // Создаем папку пользователя 
        Directory.CreateDirectory(accountDirectoryPath);

        // Получаем путь к папке сообщений пользователя
        var messagesFolder = Path.Combine(accountDirectoryPath, SystemFoldersNames.Messages);
        
        // Создаем папку сообщений пользователя
        Directory.CreateDirectory(messagesFolder);
        
        // Получаем путь к папке контактов пользователя
        var contactsFolder = Path.Combine(accountDirectoryPath, SystemFoldersNames.Contacts);
        
        // Создаем папку контактов пользователя
        Directory.CreateDirectory(contactsFolder);
    }
}