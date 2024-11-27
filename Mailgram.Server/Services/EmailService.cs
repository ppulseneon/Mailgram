using System.Text;
using Mailgram.Server.Enums;
using Mailgram.Server.Extensions.Mappers;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Requests;
using Mailgram.Server.Repositories.Interfaces;
using Mailgram.Server.Services.Interfaces;
using Mailgram.Server.Tools;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Newtonsoft.Json;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Mailgram.Server.Services;

public class EmailService(IMessagesRepository messagesRepository, IContactsRepository contactsRepository,
    IEncryptService encryptService) : IEmailService
{
    public async Task SyncAsync(Account account)
    {
        // Получаем все письма из базы данных
        var localSavedMessages = await messagesRepository.GetMessages(account.Id);
        
        var contacts = await contactsRepository.GetContactsAsync(account.Id);
        
        // Создаем список для хранения всех писем
        var allMessages = new List<Message>();

        // Подключаемся через IMAP к серверу
        using var client = new ImapClient();
        try
        {
            // Подключаемся к серверу
            await client.ConnectAsync(account.ImapCredentials.Hostname, account.ImapCredentials.Port, true)
                .ConfigureAwait(false);

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
                    .FetchAsync(batchUids,
                        MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.Flags)
                    .ConfigureAwait(false);

                // Цикл по письмам в пачке
                foreach (var summary in messages)
                {
                    // Получаем полное письмо по его UID
                    var mimeMessage = await inbox.GetMessageAsync(summary.UniqueId)
                        .ConfigureAwait(false);

                    var folder = Folders.Incoming;
                    var seen = false;

                    // Обработка реквеста
                    if (mimeMessage.Headers.Contains("X-Swap-Flag"))
                    {
                        var contact = JsonConvert.DeserializeObject<ContactSwap>(mimeMessage.HtmlBody);

                        if (contacts.FirstOrDefault(x => x.Email == contact.From &&
                                                         x.Status == ExchangeStatus.Sent && contact.SwapStatus == SwapStatus.Response) != null)
                        {

                            var importedContact = new Contact
                            {
                                Email = contact!.From,
                                Status = ExchangeStatus.Received
                            };

                            if (contact.SwapStatus == SwapStatus.Request)
                            {
                                await contactsRepository.SaveContact(account.Id, importedContact);
                            }

                            if (contact.SwapStatus == SwapStatus.Response)
                            {
                                importedContact.Status = ExchangeStatus.Accept;
                                await contactsRepository.SaveContact(account.Id, importedContact);
                            }

                            await contactsRepository.ImportContactKeys(account.Id, contact.From, contact.PublicRsa,
                                contact.PublicEcp);
                        }

                        continue;
                    }
                    
                    if (summary.Flags.HasValue && summary.Flags.Value.HasFlag(MessageFlags.Flagged))
                    {
                        folder = Folders.Favorites;
                    }

                    if (summary.Flags.HasValue && summary.Flags.Value.HasFlag(MessageFlags.Seen))
                    {
                        seen = true;
                    }
                    
                    var attachments = new List<string>();
                    
                    foreach (var attachment in mimeMessage.Attachments)
                    {
                        if (attachment is MimePart mimePart)
                        {
                            attachments.Add(mimePart.FileName);
                        }
                    }
                    
                    // Обработка зашифрованного сообщения
                    if (mimeMessage.Headers.Contains("X-Encrypt-Flag") || mimeMessage.Headers.Contains("X-Signed-Flag"))
                    {
                        if (localSavedMessages.FirstOrDefault(x => x.Id == summary.UniqueId.Id) == null)
                        {
                            await SaveContactMessage(account, mimeMessage.ToMessage((int)summary.UniqueId.Id, folder, attachments), mimeMessage.Attachments,
                                mimeMessage.Headers.Contains("X-Encrypt-Flag"), mimeMessage.Headers.Contains("X-Signed-Flag"));
                        }
                        continue;
                    }

                    var message = mimeMessage.ToMessage((int)summary.UniqueId.Id, folder, attachments);

                    allMessages.Add(message);
                }
            }

            // Получаем выборку незагруженных писем в хранилище
            var newEmails = allMessages.Where(m => localSavedMessages.All(ue => ue.Id != m.Id)).ToList();
            
            // Сохраняем письма и файлы
            await messagesRepository.SaveMessages(account.Id, newEmails);
            
            // Скачивание файлов
            foreach (var email in newEmails)
            {
                try 
                {
                    var messageUid = new UniqueId((uint)email.Id);
                    var message = await client.Inbox.GetMessageAsync(messageUid, CancellationToken.None);
                    
                    foreach (var attachment in message.Attachments)
                    {
                        var fileName = "";
                        byte[] content = null;

                        if (attachment is MimePart mimePart)
                        {
                            fileName = mimePart.FileName;
                            using var memoryStream = new MemoryStream();
                            await mimePart.Content.DecodeToAsync(memoryStream, CancellationToken.None);
                            content = memoryStream.ToArray();
                        }
                        
                        if(content != null && !string.IsNullOrEmpty(fileName)) {
                            await messagesRepository.SaveAttachment(account.Id, email.Id, fileName, content);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading attachments for email {email.Id}: {ex.Message}");
                }
            }
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

    public async Task<Message?> ChangeStarred(Account account, int messageId)
    {
        var message = await messagesRepository.GetMessage(account.Id, messageId);

        if (message == null)
        {
            return null;
        }

        using (var client = new ImapClient())
        {
            try
            {
                // Подключаемся к серверу
                await client.ConnectAsync(account.ImapCredentials.Hostname, account.ImapCredentials.Port, true)
                    .ConfigureAwait(false);

                // Авторизуемся на нем
                await client.AuthenticateAsync(account.Login, account.Password).ConfigureAwait(false);

                var inbox = client.Inbox;

                // Открываем папку "Входящие" с режимом записи
                await inbox.OpenAsync(FolderAccess.ReadWrite);

                // Ид письма на почте
                var messageUid = new UniqueId((uint)messageId);

                var inboxMessage = await inbox.GetMessageAsync(messageUid);

                if (inboxMessage != null)
                {
                    switch (message)
                    {
                        case { Folder: Folders.Incoming }:
                            message.Folder = Folders.Favorites;
                            await inbox.SetFlagsAsync(messageUid, MessageFlags.Flagged, true);
                            break;
                        case { Folder: Folders.Favorites }:
                            message.Folder = Folders.Incoming;
                            await inbox.RemoveFlagsAsync(messageUid, MessageFlags.Flagged, true);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadEmails catch exception: " + ex.Message);
            }
        }

        await messagesRepository.SaveMessage(account.Id, message);
        return message;
    }

    public async Task<Message?> DeleteEmail(Account account, int messageId)
    {
        var message = await messagesRepository.GetMessage(account.Id, messageId);

        if (message == null)
        {
            return null;
        }

        message.Folder = message switch
        {
            { Folder: Folders.Incoming } => Folders.Deleted,
            { Folder: Folders.Favorites } => Folders.Deleted,
            { Folder: Folders.Deleted } => Folders.Incoming,
            _ => message.Folder
        };

        // Кейс, если восстановили удаленное отправленное сообщение
        if (message.From.Contains(account.Login) && message.Folder == Folders.Incoming)
        {
            message.Folder = Folders.Sent;
        }

        await messagesRepository.SaveMessage(account.Id, message);
        return message;
    }

    public async Task<Message?> SendMessage(Account account, SendMessageRequest request, bool isSwap = false)
    {
        var domain = account.Platform;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(account.Login, account.Login.AppendDomain(domain)));
        message.To.Add(MailboxAddress.Parse(request.To));
        message.Subject = request.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = request.Message
        };

        if (request.Attachments != null && request.Attachments.Count != 0)
        {
            foreach (var file in request.Attachments.Where(file => file.Length > 0))
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                bodyBuilder.Attachments.Add(file.FileName, memoryStream.ToArray(),
                    ContentType.Parse(file.ContentType));
            }
        }
        
        if (request.EncryptedAttachments.Count != 0)
        {
            foreach (var file in request.EncryptedAttachments)
            {
                await bodyBuilder.Attachments.AddAsync(file);
            }
        }

        message.Body = bodyBuilder.ToMessageBody();

        if (isSwap)
        {
            message.Headers.Add("X-Swap-Flag", "true");
        }
        
        if (request.IsEncrypt)
        {
            message.Headers.Add("X-Encrypt-Flag", "true");
        }
        
        if (request.IsSign)
        {
            message.Headers.Add("X-Signed-Flag", "true");
        }
        
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(account.SmtpCredentials.Hostname, account.SmtpCredentials.Port, true);
            await client.AuthenticateAsync(account.Login, account.Password).ConfigureAwait(false);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error sending email: {ex.Message}");
            throw;
        }

        // Синхронизация отправленного письма через IMAP
        using var imapClient = new ImapClient();
        try
        {
            // Подключаемся к серверу
            await imapClient.ConnectAsync(account.ImapCredentials.Hostname, account.ImapCredentials.Port, true)
                .ConfigureAwait(false);

            // Авторизуемся на нем
            await imapClient.AuthenticateAsync(account.Login, account.Password).ConfigureAwait(false);

            var sentFolder = imapClient.GetFolder(SpecialFolder.Sent);
            await sentFolder.OpenAsync(FolderAccess.ReadWrite);
            
            await sentFolder.AppendAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("LoadEmails catch exception: " + ex.Message);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }

        return message.ToMessage(await messagesRepository.GetLastSentMessageId(account.Id)-1, Folders.Sent);
    }

    public async Task<Message> SendMessageRequestToMessage(Account account, SendMessageRequest request)
    {
        var domain = account.Platform;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(account.Login, account.Login.AppendDomain(domain)));
        message.To.Add(MailboxAddress.Parse(request.To));
        message.Subject = request.Subject;
        message.Date = DateTime.Now;
        
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = request.Message
        };
        
        message.Body = bodyBuilder.ToMessageBody();
        
        return message.ToMessage(await messagesRepository.GetLastSentMessageId(account.Id)-1, Folders.Sent);
    }

    public async Task<SendMessageRequest> CreateContactMessage(Account account, SendMessageRequest request)
    {
        var result = new SendMessageRequest
        {
            UserId = request.UserId,
            Subject = request.Subject,
            To = request.To,
            Message = request.Message,
            IsEncrypt = request.IsEncrypt,
            IsSign = request.IsSign,
            Attachments = [],
            EncryptedAttachments = []
        };

        var (publicRsa, privateEcp) = contactsRepository.GetEncryptKeysPaths(account.Id, request.To);
        var tempFolder = AppData.CreateSubFolder();
        
        if (request.IsEncrypt)
        {
            var (dsaKey, iv) = await encryptService.GenerateTripleDes();
            
            result.Message = await encryptService.EncryptMessage(request.Message, dsaKey, iv);

            var tempEncryptedDsaPath = await encryptService.EncryptKey(dsaKey, tempFolder, publicRsa);
            result.EncryptedAttachments.Add(tempEncryptedDsaPath);

            var tempIvPath = await encryptService.SaveIv(iv, tempFolder);
            result.EncryptedAttachments.Add(tempIvPath);

            if (request.Attachments != null)
            {
                foreach (var attachment in request.Attachments)
                {
                    var encryptedAttachment = await encryptService.EncryptFile(attachment, tempFolder, dsaKey, iv);
                    result.EncryptedAttachments.Add(encryptedAttachment);
                }
            }
        }

        if (request.IsSign)
        {
            var hash = encryptService.ComputeSha256Hash(request.Message);
            var signature = await encryptService.CreateMessageSign(hash, privateEcp); 
            var tempSign = await encryptService.SaveSign(signature, tempFolder);
            result.EncryptedAttachments.Add(tempSign);
        }
        
        return result;
    }

    private async Task SaveContactMessage(Account account, Message message, IEnumerable<MimeEntity> attachmentsEntity, bool isEncrypted, bool isSigned)
    {
        var resultMessage = new Message
        {
            Id = message.Id,
            Subject = message.Subject,
            From = message.From,
            To = message.To,
            HtmlContent = message.HtmlContent,
            Folder = Folders.Incoming,
            AttachmentFiles = message.AttachmentFiles,
            Date = message.Date,
            IsEncrypted = message.IsEncrypted,
            IsSigned = message.IsSigned,
            IsSeen = message.IsSeen,
            IsEncryptedRight = message.IsEncryptedRight,
            IsSignedRight = message.IsSignedRight,
        };
        
        var (privateRsa, publicEcp) = contactsRepository.GetDecryptKeysPaths(account.Id, message.From);
        
        if (isEncrypted)
        {
            resultMessage.IsEncrypted = true;
            resultMessage.IsEncryptedRight = true;
            
            byte[] key = null;
            byte[] iv = null;

            foreach (var attachment in attachmentsEntity)
            {
                if (attachment is not MimePart part) continue;
                
                var fileName = part.FileName ?? part.ContentDisposition?.FileName;

                if (fileName == null) continue;
                using var stream = new MemoryStream();
                await part.Content.DecodeToAsync(stream);
                var data = stream.ToArray();

                if (fileName.EndsWith(".key", StringComparison.OrdinalIgnoreCase))
                {
                    key = data;
                }
                else if (fileName.EndsWith(".iv", StringComparison.OrdinalIgnoreCase))
                {
                    iv = data;
                }
            }
            
            var decryptedKey = await encryptService.DecryptKey(key!, privateRsa);
            
            resultMessage.HtmlContent = await encryptService.DecryptMessage(message.HtmlContent, decryptedKey, iv!);
            
            // Обрабатываем все прикрепленные файлы
            var newAttachments = (from attachment in resultMessage.AttachmentFiles where attachment != ".key" && attachment != ".iv" && attachment != ".sign" select attachment[..^4]).ToList();
            resultMessage.AttachmentFiles = newAttachments;
            
            await messagesRepository.SaveMessage(account.Id, resultMessage);
            
            if (message.AttachmentFiles != null)
            {
                foreach (var attachment in attachmentsEntity)
                {
                    if (attachment is not MimePart part) continue;
                    
                    var fileName = part.FileName ?? part.ContentDisposition?.FileName;
                    
                    if (!fileName.EndsWith(".enc", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("The file is not encrypted (.enc).");
                        continue;
                    }
                    
                    using var stream = new MemoryStream();
                    await part.Content.DecodeToAsync(stream);
                    var data = stream.ToArray();
                    
                    var encryptedAttachment = await encryptService.DecryptFile(data, decryptedKey, iv!);
                    
                    var decryptedFilePath = fileName[..^4]; 
                    await messagesRepository.SaveAttachment(account.Id, message.Id, decryptedFilePath, encryptedAttachment);
                }
            }
        }
        
        if (isSigned)
        {
            resultMessage.IsSigned = true;
            byte[] sign = null;
            
            if (message.AttachmentFiles != null)
            {
                foreach (var attachment in attachmentsEntity)
                {
                    if (attachment is not MimePart part) continue;

                    var fileName = part.FileName ?? part.ContentDisposition?.FileName;

                    if (fileName.EndsWith(".sign", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = new MemoryStream();
                        await part.Content.DecodeToAsync(stream);
                        sign = stream.ToArray();
                    }
                }
            }

            if (sign == null)
            {
                resultMessage.IsSignedRight = false;
                await messagesRepository.SaveMessage(account.Id, resultMessage);
                return;
            }

            var hash = encryptService.ComputeSha256Hash(resultMessage.HtmlContent);
            var signedResult = await encryptService.VerifySign(hash, sign, publicEcp); 
            resultMessage.IsSignedRight = signedResult;
            await messagesRepository.SaveMessage(account.Id, resultMessage);
        }
    }

    public async Task<string> GetDecryptAttachment(Guid userId, int messageId, string attachmentName)
    {
        return await messagesRepository.GetMessageAttachmentPath(userId, messageId, attachmentName);
    }
}