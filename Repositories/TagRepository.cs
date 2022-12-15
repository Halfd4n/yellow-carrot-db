using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowCarrotDb.Data;
using YellowCarrotDb.Models;

namespace YellowCarrotDb.Repositories;
public class TagRepository
{
    private readonly RecipeDbContext _context;

    public TagRepository(RecipeDbContext context)
	{
        _context = context;
    }

    /// <summary>
    /// Getting all tags from the database.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.Include(t => t.Recipes).ToListAsync();
    }

    /// <summary>
    /// Get a specific tag by tag id.
    /// </summary>
    /// <param name="tagId"></param>
    /// <returns></returns>
    public async Task<Tag> GetTagByIdAsync(int tagId)
    {
        return await _context.Tags.FindAsync(tagId);
    }

    /// <summary>
    /// Get a specific tag by name.
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public async Task<Tag> GetTagByName(string tagName)
    {
        return await _context.Tags.Include(t => t.Recipes).FirstOrDefaultAsync(t => t.Name.Equals(tagName));
    }

    /// <summary>
    /// Get one or many specific tags by recipe id.
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    public async Task<List<Tag>> GetTagsByRecipeId(int recipeId)
    {
        return await _context.Tags.Include(t => t.Recipes).Where(t => t.Recipes.Any(r => r.RecipeId.Equals(recipeId))).ToListAsync();

    }

    /// <summary>
    /// Add tag to the database.
    /// </summary>
    /// <param name="tagToAdd"></param>
    /// <returns></returns>
    public async Task AddTagAsync(Tag tagToAdd)
    {
        await _context.Tags.AddAsync(tagToAdd);
    }

    /// <summary>
    /// Update a specific tag in the database.
    /// </summary>
    /// <param name="tagToUpdate"></param>
    public void UpdateTag(Tag tagToUpdate)
    {
        _context.Tags.Update(tagToUpdate);
    }

    /// <summary>
    /// Remove a tag from the database.
    /// </summary>
    /// <param name="tagToRemove"></param>
    public void RemoveTag(Tag tagToRemove)
    {
        _context.Tags.Remove(tagToRemove);
    }
}
