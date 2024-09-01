﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Server.Users;

namespace Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoCollection<User> users)
        {
            _users = users;

            _users.Indexes.CreateOne(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(x => x.Email),
                    new CreateIndexOptions { Unique = true }));

            _users.Indexes.CreateOne(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(x => x.PersonId),
                    new CreateIndexOptions { Unique = true }));
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
        {
            await _users.InsertOneAsync(user, options: default, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _users.AsQueryable()
                .FirstOrDefaultAsync(t => t.Email == email, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdatePasswordAsync(string email, string newPasswordHash, string salt, CancellationToken cancellationToken = default)
        {
            await _users.UpdateOneAsync(
                Builders<User>.Filter.Eq(t => t.Email, email),
                Builders<User>.Update.Combine(
                    Builders<User>.Update.Set(t => t.PasswordHash, newPasswordHash),
                    Builders<User>.Update.Set(t => t.Salt, salt)),
                options: default,
                cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
