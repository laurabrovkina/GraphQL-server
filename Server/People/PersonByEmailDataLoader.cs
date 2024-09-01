using Microsoft.EntityFrameworkCore;

namespace Server.People;

public class PersonByEmailDataLoader
    : BatchDataLoader<string, Person>
{
    private readonly ChatDbContext _dbContext;

    public PersonByEmailDataLoader(IBatchScheduler batchScheduler, ChatDbContext dbContext)
        : base(batchScheduler)
    {
        _dbContext = dbContext;
    }

    protected override async Task<IReadOnlyDictionary<string, Person>> LoadBatchAsync(
        IReadOnlyList<string> keys, CancellationToken cancellationToken)
    {
        return await _dbContext.People
            .Where(t => keys.Contains(t.Email))
            .ToDictionaryAsync(t => t.Email);
    }
}
