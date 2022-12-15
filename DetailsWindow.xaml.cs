using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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

public partial class DetailsWindow : Window
{
    private int _userId;
    AppUser _signedInUser;

    private List<Ingredient> _ingredientsToAdd = new();
    private List<Ingredient> _ingredientsToRemove = new();

    private List<Tag> _tagsToAdd = new();
    private List<Tag> _tagsToRemove = new();

    private Recipe _recipeToEdit;

    private IngredientManager _ingredientManager = new();
    private RecipeManager _recipeManager = new();
    private UserManager _userManager = new();

    public DetailsWindow(int signedInUserId, int recipeId)
    {
        InitializeComponent();

        _userId = signedInUserId;

        SeedDataFields(recipeId, signedInUserId);
    }

    /// <summary>
    /// Seeding of all the data field of DetailsWindow, including currently signed in user and chosen recipe.
    /// </summary>
    /// <param name="recipeId"></param>
    /// <param name="signedInUserId"></param>
    /// <returns></returns>
    private async Task SeedDataFields(int recipeId, int signedInUserId)
    {
        using (RecipeDbContext context = new())
        {
            UnitOfWork unitOfWork = new(context);

            _recipeToEdit = await unitOfWork.RecipeRepository.GetRecipeByIdAsync(recipeId);

            _ingredientsToAdd = _recipeToEdit.Ingredients;
            _tagsToAdd = _recipeToEdit.Tags;

            txtRecipeName.Text = _recipeToEdit.Name;
            lvIngredients.ItemsSource = _ingredientsToAdd;

            cmbTags.ItemsSource = await unitOfWork.TagRepository.GetAllTagsAsync();

            lvTags.ItemsSource = await unitOfWork.TagRepository.GetTagsByRecipeId(_recipeToEdit.RecipeId);
        }

        using(UserDbContext userContext = new())
        {
            UserRepository userRepository = new(userContext);

            _signedInUser = await userRepository.GetUserByIdAsync(signedInUserId);
        }
    }

    /// <summary>
    /// Checking so that user is admin or the author of the recipe.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnEditRecipe_Click(object sender, RoutedEventArgs e)
    {
        if(_signedInUser.IsAdmin || _signedInUser.Username.Equals(_recipeToEdit.Username))
        {
            UnlockDataFields();
        }
        else
        {
            MessageBox.Show("You are only allowed to modify your own recipes!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    /// <summary>
    /// Unlocking data fields to allow for modification of the recipe.
    /// </summary>
    private void UnlockDataFields()
    {
        txtRecipeName.IsEnabled = true;
        txtIngredientName.IsEnabled = true;
        txtIngredientUnit.IsEnabled = true;
        txtIngredientQuantity.IsEnabled = true;
        lvIngredients.IsEnabled = true;
        lvTags.IsEnabled = true;
        btnAddIngredient.IsEnabled = true;
        btnRemoveIngredient.IsEnabled = true;
        btnAddTag.IsEnabled = true;
        btnRemoveTag.IsEnabled = true;
        btnSaveRecipe.IsEnabled = true;
        cmbTags.IsEnabled = true;

        btnEditRecipe.IsEnabled = false;
    }

    /// <summary>
    /// Adding a new ingredient to the recipe. First checking for faulty input into the data fields.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
    {
        bool isDouble = double.TryParse(txtIngredientQuantity.Text, out double ingredientQuantity);

        try
        {
            if (String.IsNullOrWhiteSpace(txtIngredientName.Text))
            {
                throw new FormatException("An ingredient must have a name.");
            }
            else if (String.IsNullOrWhiteSpace(txtIngredientUnit.Text))
            {
                throw new FormatException("You haven't assigned a measurement unit.");
            }
            else if (!isDouble)
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
        catch (FormatException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Updating UI after changes made to the data fields.
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
    private void ClearIngredientField()
    {
        txtIngredientName.Clear();
        txtIngredientUnit.Clear();
        txtIngredientQuantity.Clear();
    }

    /// <summary>
    /// Removing an existing ingredient from the ingredients list.
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
            _ingredientsToRemove.Add((Ingredient)lvIngredients.SelectedItem);
            _ingredientsToAdd.Remove((Ingredient)lvIngredients.SelectedItem);

            UpdateUI();
        }
    }

    /// <summary>
    /// Adding a new tag to the recipe.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddTag_Click(object sender, RoutedEventArgs e)
    {
        if (cmbTags.SelectedItem is null)
        {
            MessageBox.Show("Please choose a tag from the combo box!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            Tag selectedTag = (Tag)cmbTags.SelectedItem;

            try
            {
                foreach (Tag tag in _tagsToAdd)
                {
                    if (tag.Name.Equals(selectedTag.Name))
                    {
                        throw new FormatException("Tag is already added to recipe!");
                    }
                }

                _tagsToAdd.Add(selectedTag);

                UpdateUI();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //if (_tagsToAdd.(selectedTag))
            //{
            //    MessageBox.Show("The given tag is already added to the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }
    }

    /// <summary>
    /// Removing an existing tag from the recipe.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRemoveTag_Click(object sender, RoutedEventArgs e)
    {
        if (lvTags.SelectedItem is null)
        {
            MessageBox.Show("Please choose a tag from the list!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            _tagsToRemove.Add((Tag)lvTags.SelectedItem);
            _tagsToAdd.Remove((Tag)lvTags.SelectedItem);

            UpdateUI();
        }
    }

    /// <summary>
    /// Saving the changes made to the recipe. Also checking so that the data fields have correct input.
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
            else if (_ingredientsToAdd.Count.Equals(0))
            {
                throw new FormatException("A recipe must contain ingredients!");
            }
            else if (_tagsToAdd.Count.Equals(0))
            {
                throw new FormatException("A recipe must have at least one tag!");
            }
            else
            {
                Recipe editetRecipe = await _recipeManager.UpdateRecipeAsync(_userId, _recipeToEdit.RecipeId, txtRecipeName.Text, _ingredientsToAdd, _tagsToAdd, _ingredientsToRemove, _tagsToRemove);

                if(editetRecipe is null)
                {
                    MessageBox.Show("Something went wrong, please try again!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"{editetRecipe.Name} was successfully updated!", "Success", MessageBoxButton.OK);

                    RecipeWindow recipeWindow = new(_userId);

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
    /// Cancel editing and returning to RecipeWindow.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        RecipeWindow recipeWindow = new(_userId);

        recipeWindow.Show();
        this.Close();
    }

}
