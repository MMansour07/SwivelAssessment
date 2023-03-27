using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Swivel.Core.Dtos.User;
using Swivel.Core.Dtos.UserInfo;
using System.Threading.Tasks;
using Swivel.Core.Dtos.General;

namespace Swivel.Service.Interfaces
{
    public interface IAuthService
    {

        #region UsersInRoles
       
        Task<ResponseModel<IdentityResult>> AddUserToRoleAsync(string userId);
        Task<ResponseModel<IdentityResult>> RemoveUserFromRoleAsync(string UserId);
        #endregion

        #region Users
        Task<ResponseModel<TableModel<UserDto>>> GetUsers(RequestModel<string> obj);
        Task<ResponseModel<UserEditDto>> FindUserAsync(string Id);
        Task<ResponseModel<IdentityResult>> RegisterUserAsync(RegisterUserDto registerDto);
        Task<ResponseModel<SignInStatus>> PasswordSignInAsync(SigninUserDto registerDto);
        Task<ResponseModel<object>> UpdateUserAsync(UserEditDto userModel);
        Task<ResponseModel<IdentityResult>> DeleteUserAsync(string userId);
        ResponseModel<bool> UserInRole(string UserName, string RoleId);
        #endregion


        #region UsersInfos
        Task<ResponseModel<UserInfoDto>> GetUserInfoAsync(string userId);
        #endregion
    }
}
