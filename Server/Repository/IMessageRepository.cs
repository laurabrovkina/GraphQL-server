using Server.Messages;

namespace Server.Repository;

public interface IMessageRepository
{
    IQueryable<Message> GetMessages(
    Guid senderId,
    Guid recipientId);

    Task AddMessageAsync(
        Message message,
        CancellationToken cancellationToken);
}
