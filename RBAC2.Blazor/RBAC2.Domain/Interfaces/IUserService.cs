using RBAC2.Domain.Models;

public interface IUserService
{
    Task<UserModel> CreateUserAsync(UserModel userModel);
    Task<UserModel> GetUserByIdAsync(int userId);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<UserModel> UpdateUserAsync(UserModel userModel);
    Task<bool> DeleteUserAsync(int userId);
}
