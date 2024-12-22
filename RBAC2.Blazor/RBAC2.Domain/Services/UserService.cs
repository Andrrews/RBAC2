using AutoMapper;
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

        public UserService(RbacDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            var userEntity = _mapper.Map<User>(userModel);
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
            var userEntity = await _context.Users.FindAsync(userId);
            if (userEntity == null)
            {
                return false;
            }

            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
