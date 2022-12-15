using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YellowCarrotDb.Data;
using YellowCarrotDb.Models;

namespace YellowCarrotDb.Repositories;
public class IngredientRepository
{
    private readonly RecipeDbContext _context;

    public IngredientRepository(RecipeDbContext context)
	{
        _context = context;
    }

    /// <summary>
    /// Get all ingredients in the database.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Ingredient>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }

    /// <summary>
    /// Get a specific ingredient from the database using ingredient id.
    /// </summary>
    /// <param name="ingredientId"></param>
    /// <returns></returns>
    public async Task<Ingredient> GetIngredientByIdAsync(int ingredientId)
    {
        return await _context.Ingredients.FindAsync(ingredientId);
    } 

    /// <summary>
    /// Get a specific ingredient from the database using ingredient name.
    /// </summary>
    /// <param name="ingredientName"></param>
    /// <returns></returns>
    public async Task<Ingredient> GetIngredientByNameAsync(string ingredientName)
    {
        return await _context.Ingredients.FirstOrDefaultAsync(i => i.IngredientName.Equals(ingredientName));
    }

    /// <summary>
    /// Add ingredient to the database.
    /// </summary>
    /// <param name="ingredientToAdd"></param>
    /// <returns></returns>
    public async Task AddIngredientAsync(Ingredient ingredientToAdd)
    {
        await _context.Ingredients.AddAsync(ingredientToAdd);
    }

    /// <summary>
    /// Update an ingredient in the database.
    /// </summary>
    /// <param name="ingredientToUpdate"></param>
    public void UpdateIngredient(Ingredient ingredientToUpdate)
    {
        _context.Ingredients.Update(ingredientToUpdate);
    }

    /// <summary>
    /// Remove an ingredient from the database.
    /// </summary>
    /// <param name="ingredientToRemove"></param>
    public void RemoveIngredient(Ingredient ingredientToRemove)
    {
        _context.Remove(ingredientToRemove);
    }
}
