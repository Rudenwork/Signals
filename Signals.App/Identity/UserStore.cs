using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Signals.App.Database;
using Signals.App.Database.Entities;

namespace Signals.App.Identity
{
    public partial class UserStore : IUserStore<UserEntity>, IUserPasswordStore<UserEntity>
    {
        private SignalsContext SignalsContext { get; set; }

        public UserStore(SignalsContext context)
        {
            SignalsContext = context;
        }

        public async Task<UserEntity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var entity = await SignalsContext.Users
                .Where(u => u.Username == normalizedUserName)
                .FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public async Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.Id.ToString();

        public async Task<string> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.Username;

        public async Task<string> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.PasswordHash;
    }

    //Not Used Members
    public partial class UserStore
    {
        public void Dispose() { }

        public Task SetPasswordHashAsync(UserEntity user, string passwordHash, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public Task SetNormalizedUserNameAsync(UserEntity user, string normalizedName, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken) =>
            IdentityResult.Success;

        public async Task<UserEntity> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public async Task<string> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public Task SetUserNameAsync(UserEntity user, string userName, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public async Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken) =>
            throw new NotImplementedException();
    }
}
