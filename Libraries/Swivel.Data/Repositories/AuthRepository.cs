using Swivel.Core.Interfaces;
using Swivel.Core.Model;
using Swivel.Data.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Swivel.Core.Dtos.UserInfo;
using System.Linq.Expressions;
using Swivel.Core.Helper;

namespace Swivel.Data.Repositories
{
    public class AuthRepository : IAuthRepository, IDisposable
    {
        private DataContext _context;
        private ApplicationUserManager _userManager;
        
        public AuthRepository(DataContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region UsersInRoles
        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string rolename)
        {
            return await _userManager.AddToRoleAsync(userId, rolename);
        }
        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string rolename)
        {
            return await _userManager.RemoveFromRoleAsync(userId, rolename);
        }
        #endregion

        #region Users
        public async Task<User> FindUserAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<UserInfoDto> FindUserinfoByIdAsync(string id)
        {
            return await _context
                .Users
                .Select(u => new UserInfoDto()
                {
                    Id = u.Id,
                    CompanyName = u.CompanyName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Phone = u.Phone,
                    MediaCount = u.Jobs.SelectMany(j => j.Medias).Count()
                }).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IdentityResult> RegisterUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<int> UpdateUserActivityAsync(string email)
        {
            var user = _context.Users.SingleOrDefault(r => r.Email == email);
            if (user != null)
            {
                user.LastActive = DateTime.Now;

                _context.Entry(user).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                _context.Users.Attach(user);

                _context.Entry(user).State = EntityState.Modified;

                _context.Entry(user).Property(x => x.PasswordHash).IsModified = false;
                _context.Entry(user).Property(x => x.SecurityStamp).IsModified = false;

                //await _context.Users.upd(user);
                //_context.Entry(user).Property(x => x.PasswordHash).IsModified = false;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public bool UserInRole(string UserName, string RoleId)
        {
           return  _context.Users.Where(u => u.UserName == UserName).Any(i => i.Roles.Any(r => r.RoleId == RoleId));
        }

        public IQueryable<User> Filter(Expression<Func<User, bool>> filter = null, Sort srt = null, 
            Query qry = null, Expression<Func<User, bool>> searchfilter = null, string includeProperties = "", bool anotherLevel = false)
        {
            IQueryable<User> query = _context.Set<User>();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(qry?.generalSearch))
                {
                    var parameter = Expression.Parameter(typeof(User));

                    var leftVisitor = new ReplaceExpressionVisitor(filter.Parameters[0], parameter);
                    var left = leftVisitor.Visit(filter.Body);

                    var rightVisitor = new ReplaceExpressionVisitor(searchfilter.Parameters[0], parameter);
                    var right = rightVisitor.Visit(searchfilter.Body);

                    query = query.Where(Expression.Lambda<Func<User, bool>>(Expression.AndAlso(left, right), parameter));
                }
                else
                    query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (!string.IsNullOrEmpty(srt?.field))
            {
                var param = Expression.Parameter(typeof(User), string.Empty);
                var property = Expression.PropertyOrField(param, srt.field);
                var sort = Expression.Lambda(property, param);

                var call = Expression.Call(
                    typeof(Queryable),
                    (!anotherLevel ? "OrderBy" : "ThenBy") + ("desc" == srt.sort ? "Descending" : string.Empty),
                    new[] { typeof(User), property.Type },
                    query.Expression,
                    Expression.Quote(sort));

                return query.Provider.CreateQuery<User>(call);
            }

            return query.AsNoTracking();
        }
        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }
        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _userManager.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}