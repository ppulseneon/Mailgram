namespace Mailgram.Server.Models.Responses;

public class ContactsResponse(List<Contact> contacts)
{
    public IList<ContactResponse> Contacts { get; set; } = contacts.Select(x => new ContactResponse()).ToList();
    public int Count { get; set; } = contacts.Count;
}