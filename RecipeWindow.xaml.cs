using Microsoft.Identity.Client;
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
/// <summary>
/// Interaction logic for RecipeWindow.xaml
/// </summary>
public partial class RecipeWindow : Window
{
    private List<Recipe> _allRecipes;
    private int _signedInUserId;

    private UserManager _userManager = new();
    private RecipeManager _recipeManager = new();

    public RecipeWindow(int signedInUserId)
    {
        InitializeComponent();

        _signedInUserId = signedInUserId;

        UpdateUI();
    }

    /// <summary>
    /// Updating the UI.
    /// </summary>
    private async void UpdateUI()
    {
        using (RecipeDbContext context = new())
        {
            UnitOfWork unitOfWork = new(context);

            lvRecipeList.ItemsSource = await unitOfWork.RecipeRepository.GetAllRecipesAsync();
            cmbTags.ItemsSource = await unitOfWork.TagRepository.GetAllTagsAsync();
        }
    }

    /// <summary>
    /// Noticing selection changes in the lvRecipeList.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void lvRecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(lvRecipeList.SelectedItem is not null)
        {
            btnDelete.IsEnabled = true;
        }
        else if(lvRecipeList.SelectedItem is null)
        {
            btnDelete.IsEnabled = false;
        }
    }

    /// <summary>
    /// Commencing a search for a given recipe name or a specific recipe tag.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnSearch_Click(object sender, RoutedEventArgs e)
    {
        if (btnSwitchSearch.Content.ToString()!.Equals("Name"))
        {
            using(RecipeDbContext context = new())
            {
                UnitOfWork unitOfWork = new(context);

                lvRecipeList.ItemsSource = await unitOfWork.RecipeRepository.GetRecipesByNameAsync(txtSearchString.Text);
            }
        }
        else
        {
            using (RecipeDbContext context = new())
            {
                UnitOfWork unitOfWork = new(context);

                lvRecipeList.ItemsSource = await unitOfWork.RecipeRepository.GetRecipesByTagAsync(cmbTags.Text);
            }
        }
    }

    /// <summary>
    /// Operation to switch between searching for recipes by name or tag.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSwitchSearch_Click(object sender, RoutedEventArgs e)
    {
        if(btnSwitchSearch.Content.ToString()!.Equals("Name"))
        {
            btnSwitchSearch.Content = "Tag";

            cmbTags.Visibility = Visibility.Visible;
            txtSearchString.Visibility = Visibility.Hidden;
        }
        else
        {
            btnSwitchSearch.Content = "Name";

            cmbTags.Visibility = Visibility.Hidden;
            txtSearchString.Visibility = Visibility.Visible;
        }
    }

    /// <summary>
    /// Sign out signed in user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSignOut_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new();

        mainWindow.Show();
        this.Close();
    }

    /// <summary>
    /// Opening AddRecipeWindow.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
    {
        AddRecipeWindow addRecipeWindow = new(_signedInUserId);

        addRecipeWindow.Show();
        this.Close();
    }

    /// <summary>
    /// Opening DetailsWindow, first checking so that a recipe is chosen from the lvRecipeList.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnDetails_Click(object sender, RoutedEventArgs e)
    {
        if (lvRecipeList.SelectedItem is null)
        {
            MessageBox.Show("Please choose a recipe from the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            Recipe recipeDetails = (Recipe)lvRecipeList.SelectedItem;

            DetailsWindow detailsWindow = new(_signedInUserId, recipeDetails.RecipeId);

            detailsWindow.Show();
            this.Close();
        }

    }

    /// <summary>
    /// Deleting a recipe from the database. First checking if the user is an admin or author of the recipe about to be deleted.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        AppUser signedInUser = await _userManager.GetSignedInUserAsync(_signedInUserId);

        Recipe recipeToRemove = (Recipe)lvRecipeList.SelectedItem;

        if (signedInUser.IsAdmin)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {recipeToRemove.Name}? This action can't be reversed.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult.Equals(MessageBoxResult.Yes))
            {
                _recipeManager.DeleteRecipe(recipeToRemove);

                UpdateUI();
                lvRecipeList.SelectedItem = null;
            }
            else
            {
                lvRecipeList.SelectedItem = null;
            }
        }
        else
        {
            if (signedInUser.Username.Equals(recipeToRemove.Username))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {recipeToRemove.Name}? This action can't be reversed.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageBoxResult.Equals(MessageBoxResult.Yes))
                {
                    _recipeManager.DeleteRecipe(recipeToRemove);

                    UpdateUI();
                    lvRecipeList.SelectedItem = null;
                }
            }
            else
            {
                MessageBox.Show("Not possible to delete other authors recipes from the database!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                lvRecipeList.SelectedItem = null;
            }
        }
    }
}
