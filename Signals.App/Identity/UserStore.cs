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

        public async Task<UserEntity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var id = Guid.Parse(userId);

            var entity = await SignalsContext.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public async Task<UserEntity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var entity = await SignalsContext.Users
                .Where(u => u.Username == normalizedUserName)
                .FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public async Task<string> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.Username.ToLower();

        public async Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.Id.ToString();

        public async Task<string> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.Username;

        public async Task SetPasswordHashAsync(UserEntity user, string passwordHash, CancellationToken cancellationToken) =>
            user.PasswordHash = passwordHash;

        public async Task<string> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.PasswordHash;

        public async Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken) =>
            user.PasswordHash != null;
    }

    //Not Used Members
    public partial class UserStore
    {
        public async Task SetNormalizedUserNameAsync(UserEntity user, string normalizedName, CancellationToken cancellationToken) { }

        public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken) =>
            IdentityResult.Success;

        public Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(UserEntity user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }
    }
}
