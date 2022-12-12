using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCarrotDb.Models;
public class Tag
{
    public int TagId { get; set; }
    public required string Name { get; set; }
    public List<Recipe> Recipes { get; set; } = new();
}
