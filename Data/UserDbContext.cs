using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Models;

namespace YellowCarrotDb.Data;
public class UserDbContext : DbContext
{
	private IEncryptionProvider _encryptionProvider;
	public UserDbContext()
	{
		_encryptionProvider = new GenerateEncryptionProvider("__yellow-carrot-encrypt_");
	}
	public UserDbContext(DbContextOptions options) : base (options)
	{

	}
	
	public DbSet<AppUser> Users { get; set; }

	/// <summary>
	/// Configuring database connection.
	/// </summary>
	/// <param name="optionsBuilder"></param>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=YellowCarrotUsersDb;Trusted_Connection=True;");
	}

	/// <summary>
	/// Seeding default users.
	/// </summary>
	/// <param name="modelBuilder"></param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.UseEncryption(_encryptionProvider);


		modelBuilder.Entity<AppUser>().HasData(new AppUser
		{
			UserId = 1,
			Username = "admin",
			Password = "password",
			IsAdmin = true
		}, new AppUser()
		{
			UserId = 2,
			Username = "user",
			Password = "password"
		});
	}
}
