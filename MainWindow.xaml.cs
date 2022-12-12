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
using YellowCarrotDb.Models;
using YellowCarrotDb.Repositories;

namespace YellowCarrotDb;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void btnSignIn_Click(object sender, RoutedEventArgs e)
    {
        using (UserDbContext context = new())
        {
            UserRepository userRepo = new(context);

            AppUser user = await userRepo.GetUserByUserNameAndPasswordAsync(txtUsername.Text, pswPassword.Password);

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
    }

    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        RegisterWindow registerWindow = new();

        registerWindow.Show();
        this.Close();
    }

    private void pswPassword_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key.Equals(Key.Return))
        {
            btnSignIn_Click(sender, e);
        }
    }
}
