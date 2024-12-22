using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RBAC2.Database;
using RBAC2.Database.Entities;
using RBAC2.Domain.Models;

namespace RBAC2.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly RbacDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(RbacDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            // Sanitize the login to remove the domain part and invalid characters
            var sanitizedLogin = SanitizeLogin(userModel.Login);

            // Create IdentityUser
            var identityUser = new IdentityUser
            {
                UserName = sanitizedLogin
            };
            var result = await _userManager.CreateAsync(identityUser);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create IdentityUser");
            }

            // Create User entity
            var userEntity = _mapper.Map<User>(userModel);
            userEntity.IdentityUserId = identityUser.Id;
            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserModel> GetUserByIdAsync(int userId)
        {
            var userEntity = await _context.Users
                .Include(u => u.Tasks)
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            var userEntities = await _context.Users
                .Include(u => u.Tasks)
                .Include(u => u.IdentityUser)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserModel>>(userEntities);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel userModel)
        {
            var userEntity = await _context.Users.FindAsync(userModel.UserId);
            if (userEntity == null)
            {
                return null;
            }

            _mapper.Map(userModel, userEntity);
            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var userEntity = await _context.Users
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (userEntity == null)
            {
                return false;
            }

            // Delete IdentityUser
            if (userEntity.IdentityUser != null)
            {
                var identityResult = await _userManager.DeleteAsync(userEntity.IdentityUser);
                if (!identityResult.Succeeded)
                {
                    throw new Exception("Failed to delete IdentityUser");
                }
            }

            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IdentityUser> FindIdentityUserByLoginAsync(string login)
        {
            var userEntity = await _context.Users
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(u => u.Login == login);

            if (userEntity == null)
            {
                return null;
            }
            return userEntity.IdentityUser;
        }

        private string SanitizeLogin(string login)
        {
            // Remove the domain part
            var sanitizedLogin = login.Substring(login.IndexOf("\\") + 1);

            // Remove invalid characters
            sanitizedLogin = Regex.Replace(sanitizedLogin, @"[^a-zA-Z0-9\._]", "");

            return sanitizedLogin;
        }
    }
}
