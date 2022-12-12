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
    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    // Get a specific user from the database using user id:
    public async Task<AppUser> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    // Get a specific user from the database using user name:
    public async Task<AppUser> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(userName));
    }

    // Get a specific user from the database using user name and password:
    public async Task<AppUser> GetUserByUserNameAndPasswordAsync(string userName, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(userName) && u.Password.Equals(password));
    }

    // Add a user to the database:
    public async Task AddUserAsync(AppUser userToAdd)
    {
        await _context.Users.AddAsync(userToAdd);
    }
}
