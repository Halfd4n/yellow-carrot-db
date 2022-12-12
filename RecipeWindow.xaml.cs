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
using YellowCarrotDb.Models;
using YellowCarrotDb.Repositories;

namespace YellowCarrotDb;
/// <summary>
/// Interaction logic for RecipeWindow.xaml
/// </summary>
public partial class RecipeWindow : Window
{
    private List<Recipe> _allRecipes;
    private List<Tag> _allTags;
    private int _signedInUserId;
    public RecipeWindow(int signedInUserId)
    {
        InitializeComponent();

        _signedInUserId = signedInUserId;

        UpdateUI();
    }

    private async void UpdateUI()
    {
        using (RecipeDbContext context = new())
        {
            UnitOfWork unitOfWork = new(context);

            lvRecipeList.ItemsSource = await unitOfWork.RecipeRepository.GetAllRecipesAsync();
            cmbTags.ItemsSource = await unitOfWork.TagRepository.GetAllTagsAsync();
        }
    }
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
    private void btnSignOut_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new();

        mainWindow.Show();
        this.Close();
    }

    private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
    {
        AddRecipeWindow addRecipeWindow = new(_signedInUserId);

        addRecipeWindow.Show();
        this.Close();
    }
    private void btnDetails_Click(object sender, RoutedEventArgs e)
    {
        Recipe recipeDetails = (Recipe)lvRecipeList.SelectedItem;

        DetailsWindow detailsWindow = new(_signedInUserId, recipeDetails);

        detailsWindow.Show();
        this.Close();
    }
    private async void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        using(UserDbContext context = new())
        {
            UserRepository userRepository = new(context);

            AppUser currentUser = await userRepository.GetUserByIdAsync(_signedInUserId);

            if (currentUser.Username.Equals("admin"))
            {
                Recipe recipeToRemove = (Recipe)lvRecipeList.SelectedItem;
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {recipeToRemove.Name}? This action can't be reversed.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageBoxResult.Equals(MessageBoxResult.Yes))
                {
                    using (RecipeDbContext recipeContext = new())
                    {
                        UnitOfWork unitOfWork = new(recipeContext);

                        unitOfWork.RecipeRepository.RemoveRecipe(recipeToRemove);
                        await unitOfWork.SaveChangesAsync();

                        UpdateUI();
                    }
                }
            }
        }


        //if(messageBoxResult.Equals())
    }
}
