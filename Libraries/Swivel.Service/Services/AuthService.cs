using Swivel.Core.Dtos.User;
using Swivel.Core.Dtos.UserInfo;
using Swivel.Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Swivel.Core.Interfaces;
using Swivel.Core.Model;
using Microsoft.AspNet.Identity;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using Swivel.Data.Identity;
using Swivel.Core.Dtos.General;
using System;
using Swivel.Core.Helper;

namespace Swivel.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _identityRepository;
        private IMapper _autoMapper;
        private ApplicationSignInManager _signInManager;
        public AuthService(IAuthRepository identityRepository, IMapper autoMapper, ApplicationSignInManager signInManager)
        {
            _identityRepository = identityRepository;
            _autoMapper = autoMapper;
            _signInManager = signInManager;
        }

        #region Users
        public async Task<ResponseModel<TableModel<UserDto>>> GetUsers(RequestModel<string> obj)
        {
            try
            {
                var users = _identityRepository.Filter(i => i.Roles.Any(r => r.RoleId != ERole.SuperAdmin.ToString()) , obj.sort, obj.query,
                    i => i.UserName.ToLower().Contains(obj.query.generalSearch), "Jobs,Roles");

                return new ResponseModel<TableModel<UserDto>>()
                {
                    Success = true,
                    Data = _autoMapper.Map<PagedList<User>, TableModel<UserDto>>(await PagedList<User>.Create(users, obj.pagination.page, obj.pagination.perpage, users.Count()))
                };
            }
            catch (Exception ex)
            {
                //logs + roll back
                return new ResponseModel<TableModel<UserDto>>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<UserEditDto>> FindUserAsync(string id)
        {
            try
            {
                return new ResponseModel<UserEditDto>() { Data = _autoMapper.Map<User, UserEditDto>(await _identityRepository.FindUserAsync(id)) ?? null, Success = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel<UserEditDto>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<IdentityResult>> RegisterUserAsync(RegisterUserDto userModel)
        {
            try
            {
                var user = _autoMapper.Map<RegisterUserDto, User>(userModel);

                var response = await _identityRepository.RegisterUserAsync(user, userModel.Password);

                if (response.Succeeded)
                {
                    var result  = await _identityRepository.AddUserToRoleAsync(user.Id, ERole.Employer.ToString());

                    if(result.Succeeded)
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    await _identityRepository.UpdateUserActivityAsync(userModel.Email);

                    return new ResponseModel<IdentityResult>() { Data = response, Success = true};
                }
                else
                    return new ResponseModel<IdentityResult>() { Data = response, Success = true };
            }
            catch (Exception ex)
            {
                //logs + roll back if possible
                return new ResponseModel<IdentityResult>() { Ex = ex, Message = ex.Message, Success = false };
            }

        }

        public async Task<ResponseModel<IdentityResult>> AddUserToRoleAsync(string UserId)
        {
            try
            {
                return new ResponseModel<IdentityResult>() { Data = await _identityRepository.AddUserToRoleAsync(UserId, ERole.Admin), Success = true };
            }
            catch (Exception ex)
            {
                //logs + roll back if possible
                return new ResponseModel<IdentityResult>() { Ex = ex, Message = ex.Message, Success = false };
            }

        }

        public async Task<ResponseModel<IdentityResult>> RemoveUserFromRoleAsync(string UserId)
        {
            try
            {
                return new ResponseModel<IdentityResult>() { Data = await _identityRepository.RemoveUserFromRoleAsync(UserId, ERole.Admin), Success = true };
            }
            catch (Exception ex)
            {
                //logs + roll back if possible
                return new ResponseModel<IdentityResult>() { Ex = ex, Message = ex.Message, Success = false };
            }

        }

        public async Task<ResponseModel<SignInStatus>> PasswordSignInAsync(SigninUserDto userModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password,
                    userModel.RememberMe, shouldLockout: false);

                // in case of success or failure of signing as trial
                await _identityRepository.UpdateUserActivityAsync(userModel.Email);

                return new ResponseModel<SignInStatus>()
                {
                    Data = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, shouldLockout: false),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<SignInStatus>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }
        public ResponseModel<bool> UserInRole(string UserName, string RoleId)
        {
            try
            {
                return new ResponseModel<bool>()
                {
                    Data = _identityRepository.UserInRole(UserName, RoleId),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<bool>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<object>> UpdateUserAsync(UserEditDto userModel)
        {
            try
            {
                await _identityRepository.UpdateAsync(_autoMapper.Map<UserEditDto, User>(userModel));
                return new ResponseModel<object>() { Data = null, Success = true };
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<object>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<IdentityResult>> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _identityRepository.FindUserAsync(userId);
                if (user != null)
                {
                    return new ResponseModel<IdentityResult>() { Data = await _identityRepository.DeleteUserAsync(user), Success = true };
                }
                return new ResponseModel<IdentityResult>() { Data = IdentityResult.Failed("User is not available"), Success = true };
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<IdentityResult>() { Ex = ex, Message = ex.Message, Success = false };
            }
           
        }

        #endregion

        #region UsersInfos
        public async Task<ResponseModel<UserInfoDto>> GetUserInfoAsync(string userId)
        {
            try
            {
                return new ResponseModel<UserInfoDto>() { Data = await _identityRepository.FindUserinfoByIdAsync(userId), Success = true};
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<UserInfoDto>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }
        #endregion
    }
}
