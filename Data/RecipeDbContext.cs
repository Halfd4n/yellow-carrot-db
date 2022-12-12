using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Models;

namespace YellowCarrotDb.Data;
public class RecipeDbContext : DbContext
{
	public RecipeDbContext()
	{

	}
	public RecipeDbContext(DbContextOptions options) : base(options)
	{

	}

	public DbSet<Ingredient> Ingredients { get; set; }
	public DbSet<Recipe> Recipes { get; set; }
	public DbSet<Tag> Tags { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=YellowCarrotRecipesDb;Trusted_Connection=True;");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		
	}

	// Vid migration flagga den med -RecipeDbContext!
}
