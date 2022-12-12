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

    // Get all ingredients in the database:
    public async Task<List<Ingredient>> GetAllIngredientsAsync()
    {
        return await _context.Ingredients.ToListAsync();
    }

    // Get a specific ingredient from the database using ingredient id:
    public async Task<Ingredient> GetIngredientByIdAsync(int ingredientId)
    {
        return await _context.Ingredients.FindAsync(ingredientId);
    } 

    // Get a specific ingredient from the database using ingredient name:
    public async Task<Ingredient> GetIngredientByNameAsync(string ingredientName)
    {
        return await _context.Ingredients.FirstOrDefaultAsync(i => i.IngredientName.Equals(ingredientName));
    }

    // Add ingredient to the database:
    public async Task AddIngredientAsync(Ingredient ingredientToAdd)
    {
        await _context.Ingredients.AddAsync(ingredientToAdd);
    }

    // Update an ingredient in the database:
    public void UpdateIngredient(Ingredient ingredientToUpdate)
    {
        _context.Ingredients.Update(ingredientToUpdate);
    }

    // Remove an ingredient from the database:
    public void RemoveIngredient(Ingredient ingredientToRemove)
    {
        _context.Remove(ingredientToRemove);
    }
}
