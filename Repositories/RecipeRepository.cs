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

    // Get all recipes from the database:
    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Tags)
            .ToListAsync();
    }

    // Get a specific recipe from the database using recipe id:
    public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
    {
        return await _context.Recipes.FindAsync(recipeId);
    }

    // Get a specific recipe from the database using recipe name:
    public async Task<Recipe> GetRecipeByNameAsync(string recipeName)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Name.Equals(recipeName));
    }

    // Get one or many specific recipes depending using search string:
    public async Task<List<Recipe>> GetRecipesByNameAsync(string searchString)
    {
        return await _context.Recipes
            .Include(r => r.Tags)
            .Where(r => r.Name.Equals(searchString))
            .ToListAsync();
    }    
    
    // Get one or many specific recipes depending on search string:
    public async Task<List<Recipe>> GetRecipesByTagAsync(string searchString)
    {
        return await _context.Recipes
            .Include(r => r.Tags)
            .Where(r => r.Tags.Any(t => t.Name.Equals(searchString)))
            .ToListAsync();
    }

    // Add a recipe to the database:
    public async Task AddRecipeAsycn(Recipe recipeToAdd)
    {
        await _context.Recipes.AddAsync(recipeToAdd);
    }

    // Update a recipe in the database:
    public void UpdateRecipe(Recipe recipeToUpdate)
    {
        _context.Recipes.Update(recipeToUpdate);
    }

    // Remove a recipe from the database:
    public void RemoveRecipe(Recipe recipeToRemove)
    {
        _context.Recipes.Remove(recipeToRemove);
    }
}