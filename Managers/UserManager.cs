using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Data;
using YellowCarrotDb.Models;
using YellowCarrotDb.Repositories;

namespace YellowCarrotDb.Managers;
public class UserManager
{
    /// <summary>
    /// Getting a specific user by user Id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<AppUser> GetSignedInUserAsync(int userId)
    {
        using(UserDbContext context = new())
        {
            UserRepository userRepository = new(context);

            return await userRepository.GetUserByIdAsync(userId);
        }
    }

    /// <summary>
    /// Checking if new username is available.
    /// </summary>
    /// <param name="userNameToCheck"></param>
    /// <returns></returns>
    public async Task<bool> CheckUserNameAvailability(string userNameToCheck)
    {
        using(UserDbContext context = new())
        {
            UserRepository userRepository = new(context);

            bool isAvailableUserName = (bool)(await userRepository.GetUserByUserNameAsync(userNameToCheck) == null);

            return isAvailableUserName;
        }
    }

    /// <summary>
    /// Creating a new user locally.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public AppUser CreateUser(string userName, string password)
    {
        AppUser newUser = new() { Username = userName, Password = password };

        return newUser;
    }

    /// <summary>
    /// Adding new user to the database.
    /// </summary>
    /// <param name="newUser"></param>
    public async Task AddUserToDb(AppUser newUser)
    {
        using(UserDbContext context = new())
        {
            UserRepository userRepository = new(context);

            await userRepository.AddUserAsync(newUser);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<AppUser> CheckUserCredentials(string username, string password)
    {
        using (UserDbContext context = new())
        {
            UserRepository userRepo = new(context);

            AppUser user = await userRepo.GetUserByUserNameAndPasswordAsync(username, password);

            if (user is null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }
    }    
}

