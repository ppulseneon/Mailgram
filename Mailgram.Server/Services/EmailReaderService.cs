using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interface;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace Mailgram.Server.Services;

public class EmailReaderService(IMessagesRepository messagesRepository) : IEmailReaderService
{
    public async Task SyncAsync(Account account)
    {
        // Получаем все письма из базы данных
        var localSavedMessages = await messagesRepository.GetMessages(account.Id);
        
        // Создаем список для хранения всех писем
        var allMessages = new List<Message>();

        // Подключаемся через IMAP к серверу
        using var client = new ImapClient();
        try
        {
            // Подключаемся к серверу
            await client.ConnectAsync(account.ImapCredentials.Hostname, account.ImapCredentials.Port, true).ConfigureAwait(false);

            // Авторизуемся на нем
            await client.AuthenticateAsync(account.Login, account.Password).ConfigureAwait(false);

            // Получаем папку "Входящие"
            var inbox = client.Inbox;

            // Открываем папку "Входящие" в режиме только для чтения
            await inbox.OpenAsync(FolderAccess.ReadOnly).ConfigureAwait(false);

            // Получаем идентификаторы (UID) всех сообщений в папке. SearchQuery.All - все письма.
            var uids = await inbox.SearchAsync(SearchQuery.All).ConfigureAwait(false);

            // Получаем письма пачками. Размер пачки - 100 писем
            const int batchSize = 100;

            // Цикл по всем UID с шагом batchSize
            for (var i = 0; i < uids.Count; i += batchSize)
            {
                // Берем пачку UID
                var batchUids = uids.Skip(i).Take(batchSize).ToList();

                // Получаем краткую информацию о письмах в пачке (UID и заголовки)
                var messages = await inbox
                    .FetchAsync(batchUids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope)
                    .ConfigureAwait(false);

                // Цикл по письмам в пачке
                foreach (var summary in messages)
                {
                    // Получаем полное письмо по его UID
                    var mimeMessage = await inbox.GetMessageAsync(summary.UniqueId)
                        .ConfigureAwait(false);

                    var message = mimeMessage.ToMessage(summary.UniqueId.Id);
                    allMessages.Add(message);
                }
            }

            // Отключаемся от сервера
            await client.DisconnectAsync(true).ConfigureAwait(false);

            // Проверяем, какие письма есть в хранилище
            
            // Обработка есть ли зашифрованные письма
            
            // Получаем выборку незагруженных писем в хранилище
            var newEmails = allMessages.Where(m => localSavedMessages.All(ue => ue.Id != m.Id)).ToList();;

            // Сохраняем письма и файлы
            await messagesRepository.SaveMessages(account.Id, newEmails);
            
            // Save attacments https://stackoverflow.com/questions/43331004/mailkit-how-to-download-all-attachments-locally-from-a-mimemessage 
        }
        catch (Exception ex)
        {
            Console.WriteLine("LoadEmails catch exception: " + ex.Message);
        }
    }

    public async Task<List<Message>> GetAll(Guid userId)
    {
        var messages = await messagesRepository.GetMessages(userId);
        return messages.OrderByDescending(message => message.Date).ToList();
    }
}