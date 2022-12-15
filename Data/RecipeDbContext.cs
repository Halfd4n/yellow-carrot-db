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

	/// <summary>
	/// Configuring database connection.
	/// </summary>
	/// <param name="optionsBuilder"></param>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=YellowCarrotRecipesDb;Trusted_Connection=True;");
	}

	/// <summary>
	/// Seeding default tags, ingredients and recipes to the database.
	/// </summary>
	/// <param name="modelBuilder"></param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        modelBuilder.Entity<Tag>().HasData(new Tag()
        {
            TagId = 1,
            Name = "Vegetarian"
        }, new Tag()
        {
            TagId = 2,
            Name = "Fast-food"
        }, new Tag()
        {
            TagId = 3,
            Name = "Meat"
        }, new Tag()
        {
            TagId = 4,
            Name = "Fish"
        }, new Tag()
        {
            TagId = 5,
            Name = "Vegan"
        }, new Tag()
        {
            TagId = 6,
            Name = "Egg-free"
        }, new Tag()
        {
            TagId = 7,
            Name = "Gluten-free"
        }, new Tag()
        {
            TagId = 8,
            Name = "Keto"
        }, new Tag()
        {
            TagId = 9,
            Name = "Healthy"
        });

        modelBuilder.Entity<Ingredient>().HasData(new Ingredient
		{
			IngredientId = 1,
			IngredientName = "Eggs",
			Unit = "pcs",
			Quantity = 2,
			RecipeId = 1

		}, new Ingredient()
		{
			IngredientId = 2,
			IngredientName = "Milk",
			Unit = "dl",
			Quantity = 4,
			RecipeId = 1

		}, new Ingredient()
		{
			IngredientId = 3,
			IngredientName = "Flour",
			Unit = "dl",
			Quantity = 2,
			RecipeId = 1

		}, new Ingredient()
		{
			IngredientId = 4,
			IngredientName = "Salt",
			Unit = "tbs",
			Quantity = 0.5,
			RecipeId = 1

		}, new Ingredient()
		{
			IngredientId = 5,
			IngredientName = "Sausage",
			Unit = "pcs",
			Quantity = 4,
			RecipeId = 2

		}, new Ingredient()
		{
			IngredientId = 6,
			IngredientName = "Buns",
			Unit = "pcs",
			Quantity = 4,
			RecipeId = 2
		});

		modelBuilder.Entity<Recipe>().HasData(new Recipe()
		{
			RecipeId = 1,
			Name = "Pancakes",
			Ingredients = new(),
			Username = "user",
		}, new Recipe()
		{
			RecipeId = 2,
			Name = "Hot dogs",
			Ingredients = new(),
			Username = "user"
		});

		modelBuilder.Entity<Recipe>()
			.HasMany(r => r.Tags)
			.WithMany(t => t.Recipes)
			.UsingEntity(j => j.HasData(new { RecipesRecipeId = 1, TagsTagId = 1 }));

		modelBuilder.Entity<Recipe>()
			.HasMany(r => r.Tags)
			.WithMany(t => t.Recipes)
			.UsingEntity(j => j.HasData(new { RecipesRecipeId = 1, TagsTagId = 2 }));

		modelBuilder.Entity<Recipe>()
			.HasMany(r => r.Tags)
			.WithMany(t => t.Recipes)
			.UsingEntity(j => j.HasData(new { RecipesRecipeId = 2, TagsTagId = 2 }));
		
		modelBuilder.Entity<Recipe>()
			.HasMany(r => r.Tags)
			.WithMany(t => t.Recipes)
			.UsingEntity(j => j.HasData(new { RecipesRecipeId = 2, TagsTagId = 3 }));
	}
}
