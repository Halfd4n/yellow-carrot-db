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

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.Include(t => t.Recipes).ToListAsync();
    }

    public async Task<Tag> GetTagByIdAsync(int tagId)
    {
        return await _context.Tags.FindAsync(tagId);
    }

    public async Task<Tag> GetTagByName(string tagName)
    {
        return await _context.Tags.Include(t => t.Recipes).FirstOrDefaultAsync(t => t.Name.Equals(tagName));
    }

    public async void AddTagAsync(Tag tagToAdd)
    {
        await _context.Tags.AddAsync(tagToAdd);
    }

    public void RemoveTag(Tag tagToRemove)
    {
        _context.Tags.Remove(tagToRemove);
    }
}
