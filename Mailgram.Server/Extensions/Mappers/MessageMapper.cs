using Mailgram.Server.Enums;
using Mailgram.Server.Models;
using Mailgram.Server.Models.Responses;
using MimeKit;

namespace Mailgram.Server.Extensions.Mappers;

public static class MessageMapper
{
    public static MessageResponse ToResponse(this Message message)
    {
        return new MessageResponse(message);
    }
    
    public static MessagesResponse ToResponse(this List<Message> messages)
    {
        return new MessagesResponse(messages);
    }
    
    public static Message ToMessage(this MimeMessage mimeMessage, int messageUniqueId, Folders folder = Folders.Incoming, List<string>? attachments = null)
    {
        var message = new Message
        {
            Id = messageUniqueId,
            From = string.Join(", ", mimeMessage.From.Select(mailbox => mailbox.ToString())),
            To = string.Join(", ", mimeMessage.To.Select(a => a.ToString())),
            Subject = mimeMessage.Subject,
            Date = mimeMessage.Date.DateTime,
            HtmlContent = string.Empty,
            Folder = folder,
            AttachmentFiles = attachments ?? []
        };

        switch (mimeMessage.Body)
        {
            case TextPart { IsHtml: true } textPart:
                message.HtmlContent = textPart.Text;
                break;
            
            case Multipart multipart:
            {
                foreach (var part in multipart.OfType<TextPart>())
                {
                    if (part.IsHtml)
                    {
                        message.HtmlContent = part.Text;
                        break; 
                    }

                    if(part.IsPlain)
                    {
                        message.HtmlContent = part.Text; 
                    }
                }

                break;
            }
        }

        return message;
    }
}