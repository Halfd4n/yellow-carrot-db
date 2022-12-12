using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCarrotDb.Models;
public class Ingredient
{
    public int IngredientId { get; set; }
    public required string IngredientName { get; set; }
    public required string Unit { get; set; }
    public required double Quantity { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}
