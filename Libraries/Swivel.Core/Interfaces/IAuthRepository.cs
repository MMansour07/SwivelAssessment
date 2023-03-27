using Swivel.Core.Model;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using Swivel.Core.Dtos.UserInfo;
using System.Linq.Expressions;
using System;

namespace Swivel.Core.Interfaces
{
    public interface IAuthRepository
    {
        #region UsersInRoles
        Task<IdentityResult> AddUserToRoleAsync(string userId, string rolename);
        Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string rolename);
        #endregion

        #region Users

        IQueryable<User> Filter(Expression<Func<User, bool>> filter = null, Sort srt = null, Query qry = null,
            Expression<Func<User, bool>> searchfilter = null, string includeProperties = "", bool anotherLevel = false);
        Task<User> FindUserAsync(string id);
        Task<UserInfoDto> FindUserinfoByIdAsync(string id);
        Task<int> UpdateUserActivityAsync(string email);
        Task<IdentityResult> RegisterUserAsync(User user, string password);
        Task  UpdateAsync(User user);
        Task<IdentityResult> DeleteUserAsync(User user);
        bool UserInRole(string UserName, string RoleId);
        #endregion
    }
}
