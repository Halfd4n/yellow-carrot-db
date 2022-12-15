using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YellowCarrotDb.Models;

namespace YellowCarrotDb.Managers;
public class IngredientManager
{
    private string _ingredientName;
    private string _ingredientUnit;
    private int _ingredientQuantityDouble;
    private List<Ingredient> _currentIngredients;

    /// <summary>
    /// Creating a new ingredient.
    /// </summary>
    /// <param name="ingredientName"></param>
    /// <param name="ingredientUnit"></param>
    /// <param name="ingredientQuantity"></param>
    /// <returns></returns>
    public Ingredient CreateIngredient(string ingredientName, string ingredientUnit, double ingredientQuantity)
    {
        Ingredient newIngredient = new() { IngredientName = ingredientName, Unit = ingredientUnit, Quantity = ingredientQuantity };

        return newIngredient;
    }

    /// <summary>
    /// Checking if the ingredient is already in the list. Returning a false boolean if so.
    /// </summary>
    /// <param name="ingredientNameToCheck"></param>
    /// <param name="currentIngredients"></param>
    /// <returns></returns>
    public bool CheckIfValidIngredient(string ingredientNameToCheck, List<Ingredient> currentIngredients)
    {
        foreach(Ingredient ingredient in currentIngredients)
        {
            if (ingredient.IngredientName.Equals(ingredientNameToCheck))
            {
                MessageBox.Show("That ingredient is already in the list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }

        return true;
    }
}
