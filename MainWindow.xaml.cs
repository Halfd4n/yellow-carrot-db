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
using System.Windows.Navigation;
using System.Windows.Shapes;
using YellowCarrotDb.Data;
using YellowCarrotDb.Managers;
using YellowCarrotDb.Models;
using YellowCarrotDb.Repositories;

namespace YellowCarrotDb;

public partial class MainWindow : Window
{
    private UserManager _userManager = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Attempt to sign in user. First checking credentials. If approved, then opening RecipeWindow.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnSignIn_Click(object sender, RoutedEventArgs e)
    {
        AppUser? user = await _userManager.CheckUserCredentials(txtUsername.Text, pswPassword.Password);

        if (user is null)
        {
            MessageBox.Show("Username or password is incorrect!", "Error", MessageBoxButton.OK);
        }
        else
        {
            RecipeWindow recipeWindow = new(user.UserId);

            recipeWindow.Show();
            this.Close();
        }
    }

    /// <summary>
    /// Open window to register new user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        RegisterWindow registerWindow = new();

        registerWindow.Show();
        this.Close();
    }

    /// <summary>
    /// Operation enabled pressing return key to click Sign in button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pswPassword_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key.Equals(Key.Return))
        {
            btnSignIn_Click(sender, e);
        }
    }
}
