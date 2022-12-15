using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Data;
using YellowCarrotDb.Models;

namespace YellowCarrotDb.Repositories;
public class RecipeRepository
{
    private readonly RecipeDbContext _context;

    public RecipeRepository(RecipeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all recipes from the database.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Tags)
            .ToListAsync();
    }

    /// <summary>
    /// Get a specific recipe from the database using recipe id.
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
    {
        return await _context.Recipes.FindAsync(recipeId);
    }

    /// <summary>
    /// Get a specific recipe including the tags by recipe id.
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public async Task<Recipe> GetRecipeAndIncludeTagsByRecipeId(int recipeId)
    {
        return await _context.Recipes.Include(r => r.Tags).FirstOrDefaultAsync(r => r.RecipeId == recipeId);
    }

    /// <summary>
    /// Get a specific recipe from the database using recipe name.
    /// </summary>
    /// <param name="recipeName"></param>
    /// <returns></returns>
    public async Task<Recipe> GetRecipeByNameAsync(string recipeName)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Name.Equals(recipeName));
    }

    /// <summary>
    /// Get one or many specific recipes depending using search string.
    /// </summary>
    /// <param name="searchString"></param>
    /// <returns></returns>
    public async Task<List<Recipe>> GetRecipesByNameAsync(string searchString)
    {
        return await _context.Recipes
            .Include(r => r.Tags)
            .Where(r => r.Name.Equals(searchString))
            .ToListAsync();
    }    
    
    /// <summary>
    /// Get one or many specific recipes depending on search string.
    /// </summary>
    /// <param name="searchString"></param>
    /// <returns></returns>
    public async Task<List<Recipe>> GetRecipesByTagAsync(string searchString)
    {
        return await _context.Recipes
            .Include(r => r.Tags)
            .Where(r => r.Tags.Any(t => t.Name.Equals(searchString)))
            .ToListAsync();
    }

    /// <summary>
    /// Get latest added recipe from the database.
    /// </summary>
    /// <returns></returns>
    public async Task<Recipe> GetLatestRecipe()
    {
        return await _context.Recipes.OrderBy(r => r.RecipeId).LastAsync();
    }

    /// <summary>
    /// Add a recipe to the database.
    /// </summary>
    /// <param name="recipeToAdd"></param>
    /// <returns></returns>
    public async Task AddRecipeAsync(Recipe recipeToAdd)
    {
        await _context.Recipes.AddAsync(recipeToAdd);
    }

    /// <summary>
    /// Update a recipe in the database.
    /// </summary>
    /// <param name="recipeToUpdate"></param>
    public void UpdateRecipe(Recipe recipeToUpdate)
    {
        _context.Recipes.Update(recipeToUpdate);
    }

    /// <summary>
    /// Remove a recipe from the database.
    /// </summary>
    /// <param name="recipeToRemove"></param>
    public void RemoveRecipe(Recipe recipeToRemove)
    {
        _context.Recipes.Remove(recipeToRemove);
    }
}