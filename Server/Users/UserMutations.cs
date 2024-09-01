using HotChocolate.Execution;
using Server.People;
using System.Security.Cryptography;
using System.Text;

namespace Server.Users
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutations
    {
        public async Task<CreateUserPayload> CreateUser(
            CreateUserInput input,
            [Service] ChatDbContext dbContext,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The name cannot be empty.")
                        .SetCode("USERNAME_EMPTY")
                        .Build());
            }

            if (string.IsNullOrEmpty(input.Email))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The email cannot be empty.")
                        .SetCode("EMAIL_EMPTY")
                        .Build());
            }

            if (string.IsNullOrEmpty(input.Password))
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The password cannot be empty.")
                        .SetCode("PASSWORD_EMPTY")
                        .Build());
            }

            string salt = Guid.NewGuid().ToString("N");

            using var sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input.Password + salt));

            Guid personId = Guid.NewGuid();

            var user = new User
            {
                Id = Guid.NewGuid(),
                PersonId = personId,
                Email = input.Email,
                PasswordHash = Convert.ToBase64String(hash),
                Salt = salt
            };

            var person = new Person
            {
                Id = personId,
                UserId = user.Id,
                Name = input.Name,
                Email = input.Email,
                LastSeen = DateTime.UtcNow,
                ImageUri = input.Image
            };

            dbContext.Users.Add(user);
            dbContext.People.Add(person);

            await dbContext.SaveChangesAsync();

            return new CreateUserPayload(user, input.ClientMutationId);
        }
    }
}
