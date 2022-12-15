using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Data;
using YellowCarrotDb.Models;
using YellowCarrotDb.Repositories;

namespace YellowCarrotDb.Managers;
public class RecipeManager
{
    private AppUser? _currentUser;

    private Recipe? _latestRecipe;
    private Recipe? _editedRecipe;



    /// <summary>
    /// Add new recipe to the database.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="recipeName"></param>
    /// <param name="ingredientsToAdd"></param>
    /// <param name="tagsToAdd"></param>
    /// <returns></returns>
    public async Task<Recipe> AddRecipe(int userId, string recipeName, List<Ingredient> ingredientsToAdd, List<Tag> tagsToAdd)
    {
        GetUser(userId);

        if (_currentUser != null)
        {
            using (RecipeDbContext context = new())
            {
                UnitOfWork unitOfWork = new(context);

                Recipe _newRecipe = new() { Name = recipeName, Username = _currentUser.Username };

                await unitOfWork.RecipeRepository.AddRecipeAsync(_newRecipe);

                await unitOfWork.SaveChangesAsync();
            }

            using (RecipeDbContext context = new())
            {
                UnitOfWork unitOfWork = new(context);

                _latestRecipe = await unitOfWork.RecipeRepository.GetLatestRecipe();

                ingredientsToAdd.ForEach(i => i.RecipeId = _latestRecipe.RecipeId);
                ingredientsToAdd.ForEach(async i => await unitOfWork.IngredientRepository.AddIngredientAsync(i));

                foreach (Tag tag in tagsToAdd)
                {
                    Tag? dbTag = context.Tags.FirstOrDefault(t => t.TagId == tag.TagId);

                    if (dbTag != null)
                    {
                        _latestRecipe.Tags.Add(dbTag);
                    }
                }

                unitOfWork.RecipeRepository.UpdateRecipe(_latestRecipe);

                await unitOfWork.SaveChangesAsync();
            }

            return _latestRecipe;
        }

        return null;
    }

    /// <summary>
    /// Update an already existing recipe in the database.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="updatedIngredientName"></param>
    /// <param name="ingredientsToAdd"></param>
    /// <param name="tagsToAdd"></param>
    /// <param name="ingredientsToRemove"></param>
    /// <param name="tagsToRemove"></param>
    /// <returns></returns>
    public async Task<Recipe> UpdateRecipeAsync(int userId, int recipeId, string updatedIngredientName, List<Ingredient> ingredientsToAdd, List<Tag> tagsToAdd, List<Ingredient> ingredientsToRemove, List<Tag> tagsToRemove)
    {
        GetUser(userId);

        if (_currentUser != null)
        {
            using (RecipeDbContext context = new())
            {
                UnitOfWork unitOfWork = new(context);

                if (ingredientsToRemove.Count > 0)
                {
                    foreach (Ingredient ingredientToRemove in ingredientsToRemove)
                    {
                        if (ingredientToRemove.IngredientId != 0)
                        {
                            unitOfWork.IngredientRepository.RemoveIngredient(ingredientToRemove);
                        }
                    }
                }

                if (ingredientsToAdd.Count > 0)
                {
                    foreach (Ingredient ingredientToAdd in ingredientsToAdd)
                    {
                        if (ingredientToAdd.RecipeId == 0)
                        {
                            ingredientToAdd.RecipeId = recipeId;

                            await unitOfWork.IngredientRepository.AddIngredientAsync(ingredientToAdd);
                        }
                    }
                }


                Recipe _editedRecipe = await unitOfWork.RecipeRepository.GetRecipeAndIncludeTagsByRecipeId(recipeId);


                if (tagsToRemove.Count > 0)
                {
                    foreach (Tag tag in tagsToRemove)
                    {
                        Tag? dbTag = context.Tags.FirstOrDefault(t => t.TagId == tag.TagId);

                        if (dbTag != null)
                        {
                            _editedRecipe.Tags.Remove(dbTag);
                        }
                    }
                }

                if (tagsToAdd.Count > 0)
                {
                    foreach (Tag tag in tagsToAdd)
                    {
                        Tag? dbTag = context.Tags.FirstOrDefault(t => t.TagId == tag.TagId);

                        if (dbTag != null)
                        {
                            _editedRecipe.Tags.Add(dbTag);
                        }
                    }

                }

                if (_editedRecipe.Name != updatedIngredientName)
                {
                    _editedRecipe.Name = updatedIngredientName;
                }

                unitOfWork.RecipeRepository.UpdateRecipe(_editedRecipe);

                await unitOfWork.SaveChangesAsync();

                return _editedRecipe;
            }
        }

        return null;

    }

    /// <summary>
    /// Get current user from the database async.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task GetUserAsync(int userId)
    {
        using (UserDbContext context = new())
        {
            UserRepository userRepository = new(context);

            _currentUser = await userRepository.GetUserByIdAsync(userId);
        }
    }

    /// <summary>
    /// Get current user from the database.
    /// </summary>
    /// <param name="userId"></param>
    private void GetUser(int userId)
    {
        using (UserDbContext context = new())
        {
            UserRepository userRepository = new(context);

            _currentUser = userRepository.GetUserById(userId);
        }
    }

    /// <summary>
    /// Delete recipe.
    /// </summary>
    /// <param name="recipeToRemove"></param>
    /// <returns></returns>
    public async Task DeleteRecipe(Recipe recipeToRemove)
    {
        using (RecipeDbContext recipeContext = new())
        {
            UnitOfWork unitOfWork = new(recipeContext);

            unitOfWork.RecipeRepository.RemoveRecipe(recipeToRemove);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
