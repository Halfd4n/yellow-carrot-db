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

    // Get all users from the database:
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    // Get a specific user from the database using user id:
    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    // Get a specific user from the database using user name:
    public async Task<User> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(userName));
    }

    // Get a specific user from the database using user name and password:
    public async Task<User> GetUserByUserNameAndPasswordAsync(string userName, string password)
    {
        return await _context.Users
            .Where(u => u.UserName.Equals(userName) && u.Password.Equals(password))
            .FirstOrDefaultAsync();
    }

    // Add a user to the database:
    public async void AddUserAsync(User userToAdd)
    {
        await _context.Users.AddAsync(userToAdd);
    }
}
