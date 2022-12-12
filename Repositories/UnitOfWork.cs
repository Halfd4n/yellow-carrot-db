using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Data;

namespace YellowCarrotDb.Repositories;
public class UnitOfWork
{
    private readonly RecipeDbContext _context;
    private RecipeRepository _recipeRepository;
    private IngredientRepository _ingredientRepository;

    public UnitOfWork(RecipeDbContext context)
    {
        _context = context;
    }

    public RecipeRepository RecipeRepository
    {
        get
        {
            if(RecipeRepository is null)
            {
                _recipeRepository = new RecipeRepository(_context);
            }

            return _recipeRepository;
        }
    }

    public IngredientRepository IngredientRepository
    {
        get
        {
            if(IngredientRepository is null)
            {
                _ingredientRepository = new IngredientRepository(_context);
            }

            return _ingredientRepository;
        }
    }

    public async void SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
