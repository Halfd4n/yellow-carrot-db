using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Data;
using YellowCarrotDb.Models;

namespace YellowCarrotDb.Repositories;
public class UserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
	{
        _context = context;
    }

    /// <summary>
    /// Get all users from the database.
    /// </summary>
    /// <returns></returns>
    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <summary>
    /// Get a specific user from the database using user id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<AppUser> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    /// <summary>
    /// Get a specific user from the database using user id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AppUser? GetUserById(int userId)
    {
        return _context.Users.Find(userId);
    }

    /// <summary>
    /// Get a specific user from the database using user name.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<AppUser> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(userName));
    }

    /// <summary>
    /// Get a specific user from the database using user name and password.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<AppUser> GetUserByUserNameAndPasswordAsync(string userName, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(userName) && u.Password.Equals(password));
    }

    /// <summary>
    /// Add a user to the database.
    /// </summary>
    /// <param name="userToAdd"></param>
    /// <returns></returns>
    public async Task AddUserAsync(AppUser userToAdd)
    {
        await _context.Users.AddAsync(userToAdd);
    }
}
