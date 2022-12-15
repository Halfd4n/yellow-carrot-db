using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YellowCarrotDb.Data;
using YellowCarrotDb.Managers;
using YellowCarrotDb.Models;
using YellowCarrotDb.Repositories;

namespace YellowCarrotDb;

public partial class AddRecipeWindow : Window
{
    private int _signedInUserId;
    private RecipeManager _recipeManager = new();
    private IngredientManager _ingredientManager = new();
    private List<Ingredient> _ingredientsToAdd = new();
    private List<Tag> _tagsToAdd = new();

    // Make quantity to integers.

    public AddRecipeWindow(int signedInUserId)
    {
        InitializeComponent();

        _signedInUserId = signedInUserId;

        Task awaitComboBox = SeedComboBox();
    }

    /// <summary>
    /// Seeding combobox from the database.
    /// </summary>
    /// <returns></returns>
    private async Task SeedComboBox()
    {
        using(RecipeDbContext context = new())
        {
            UnitOfWork unitOfWork = new(context);

            cmbTags.ItemsSource = await unitOfWork.TagRepository.GetAllTagsAsync();
        }
    }

    /// <summary>
    /// Add new ingredient to lvIngredients.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
    {
        bool isDouble = double.TryParse(txtIngredientQuantity.Text, out double ingredientQuantity);

        try
        {
            if(String.IsNullOrWhiteSpace(txtIngredientName.Text))
            {
                throw new FormatException("An ingredient must have a name.");
            }
            else if(String.IsNullOrWhiteSpace(txtIngredientUnit.Text))
            {
                throw new FormatException("You haven't assigned a measurement unit.");
            }
            else if(!isDouble)
            {
                throw new FormatException("The quantity wasn't assigned correctly, only input numbers in this box");
            }
            else
            {
                bool isValidIngredient = _ingredientManager.CheckIfValidIngredient(txtIngredientName.Text, _ingredientsToAdd);

                if (isValidIngredient)
                {
                    Ingredient newIngredient = _ingredientManager.CreateIngredient(txtIngredientName.Text, txtIngredientUnit.Text, ingredientQuantity);

                    _ingredientsToAdd.Add(newIngredient);

                    UpdateUI();

                    ClearIngredientField();
                }
            }
        }
        catch(FormatException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Updating UI.
    /// </summary>
    private void UpdateUI()
    {
        lvIngredients.SelectedItem = null;
        lvIngredients.ItemsSource = null;
        
        cmbTags.SelectedItem = null;
        lvTags.ItemsSource = null;

        lvIngredients.ItemsSource = _ingredientsToAdd;
        lvTags.ItemsSource = _tagsToAdd;
    }

    /// <summary>
    /// Clearing data fields associated to creating a new ingredient.
    /// </summary>
    private void ClearIngredientField()
    {
        txtIngredientName.Clear();
        txtIngredientUnit.Clear();
        txtIngredientQuantity.Clear();
    }

    /// <summary>
    /// Remove ingredient from lvIngredients.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRemoveIngredient_Click(object sender, RoutedEventArgs e)
    {
        if(lvIngredients.SelectedItem is null)
        {
            MessageBox.Show("Please choose an ingredient from the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            _ingredientsToAdd.Remove((Ingredient)lvIngredients.SelectedItem);

            UpdateUI();
        }
    }

    /// <summary>
    /// Adding new tag to lvTags.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddTag_Click(object sender, RoutedEventArgs e)
    {
        if(cmbTags.SelectedItem is null)
        {
            MessageBox.Show("Please choose a tag from the combo box!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            if (_tagsToAdd.Contains((Tag)cmbTags.SelectedItem))
            {
                MessageBox.Show("The given tag is already added to the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _tagsToAdd.Add((Tag)cmbTags.SelectedItem);

                UpdateUI();
            }
        }
    }

    /// <summary>
    /// Remove tag from lvTags.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRemoveTag_Click(object sender, RoutedEventArgs e)
    {
        if(lvTags.SelectedItem is null)
        {
            MessageBox.Show("Please choose a tag from the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            _tagsToAdd.Remove((Tag)lvTags.SelectedItem);

            UpdateUI();
        }
    }

    /// <summary>
    /// Save new recipe. Collecting data from the data fields and sending the to RecipeManager.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnSaveRecipe_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtRecipeName.Text))
            {
                throw new FormatException("A recipe must have a name!");
            }
            else if(_ingredientsToAdd.Count.Equals(0))
            {
                throw new FormatException("A recipe must contain ingredients!");
            }
            else if(_tagsToAdd.Count.Equals(0))
            {
                throw new FormatException("A recipe must have at least one tag!");
            }
            else
            {
                Recipe recipeToAdd = await _recipeManager.AddRecipe(_signedInUserId, txtRecipeName.Text, _ingredientsToAdd, _tagsToAdd);

                if(recipeToAdd is null)
                {
                    MessageBox.Show("Something went wrong, please try again!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"{recipeToAdd.Name} was added to Yellow Carrot!", "Success", MessageBoxButton.OK);

                    RecipeWindow recipeWindow = new(_signedInUserId);

                    recipeWindow.Show();
                    this.Close();
                }
            }
        }
        catch (FormatException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Cancel and return to RecipeWindow.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        RecipeWindow recipeWindow = new(_signedInUserId);

        recipeWindow.Show();
        this.Close();
    }
}
