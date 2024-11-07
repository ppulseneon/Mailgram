using Mailgram.Server.Models;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Utility;
using Newtonsoft.Json;

namespace Mailgram.Server.Repositories;

public class AccountRepository: IAccountRepository
{
    private readonly string _basePath;
    
    public AccountRepository()
    {
        _basePath = AppData.GetAppDataDirectory();
    }
    
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
        
        Directory.CreateDirectory(accountDirectoryPath);
        
        var jsonString = JsonConvert.SerializeObject(account);
        await File.WriteAllTextAsync(accountFilePath, jsonString);
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
                var jsonString = await File.ReadAllTextAsync(filePath);
                var account = JsonConvert.DeserializeObject<Account>(jsonString);

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
}