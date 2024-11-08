using System.Net.Mail;
using Mailgram.Server.Models;
using Mailgram.Server.Services.Interface;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace Mailgram.Server.Services;

public class EmailReaderService : IEmailReaderService
{
    public async Task LoadEmailsAsync(Account account)
    {
        // Получаем все письма из базы данных
        // var uploadedEmails;

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

            // Создаем список для хранения всех писем
            var allMessages = new List<Message>();

            // Получаем идентификаторы (UID) всех сообщений в папке. SearchQuery.All - все письма.
            var uids = await inbox.SearchAsync(SearchQuery.All).ConfigureAwait(false);

            // Получаем письма пачками. Размер пачки - 100 писем
            const int batchSize = 100;

            // Цикл по всем UID с шагом batchSize
            for (int i = 0; i < uids.Count; i += batchSize)
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

                    var message = new Message
                    {
                        Id = summary.UniqueId.Id,
                        MimeMessage = mimeMessage,
                        AttachmentFiles = []
                    };
                    
                    allMessages.Add(message);
                }
            }

            // Отключаемся от сервера
            await client.DisconnectAsync(true).ConfigureAwait(false);

            // Проверяем, какие письма есть в хранилище
            
            // Получаем выборку незагруженных писем в хранилище
            
            // Сохраняем письма и файлы
            
            // Save attacments https://stackoverflow.com/questions/43331004/mailkit-how-to-download-all-attachments-locally-from-a-mimemessage 
        }
        catch (Exception ex)
        {
            Console.WriteLine("LoadEmails catch exception: " + ex.Message);
        }
    }
}