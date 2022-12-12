using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCarrotDb.Models;
public class Recipe
{
    public int RecipeId { get; set; }
    public required string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public User UserName { get; set; }

}
