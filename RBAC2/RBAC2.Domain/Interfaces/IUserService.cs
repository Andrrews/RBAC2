using Microsoft.AspNetCore.Identity;
using RBAC2.Domain.Models;

/// <summary>
/// Interface for user service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userModel">The user model.</param>
    /// <returns>The created user model.</returns>
    Task<UserModel> CreateUserAsync(UserModel userModel);

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The user model.</returns>
    Task<UserModel> GetUserByIdAsync(int userId);

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A collection of user models.</returns>
    Task<IEnumerable<UserModel>> GetAllUsersAsync();

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="userModel">The user model.</param>
    /// <returns>The updated user model.</returns>
    Task<UserModel> UpdateUserAsync(UserModel userModel);

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>True if the user was deleted, otherwise false.</returns>
    Task<bool> DeleteUserAsync(int userId);

    /// <summary>
    /// Finds an identity user by their login.
    /// </summary>
    /// <param name="login">The login.</param>
    /// <returns>The identity user.</returns>
    Task<IdentityUser> FindIdentityUserByLoginAsync(string login);
}
