using Server.Users;

namespace Server.Repository;

public interface IUserRepository
{
    Task<User> GetUserAsync(
    string email,
    CancellationToken cancellationToken = default);

    Task AddUserAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task UpdatePasswordAsync(
        string email,
        string newPasswordHash,
        string salt,
        CancellationToken cancellationToken = default);
}
