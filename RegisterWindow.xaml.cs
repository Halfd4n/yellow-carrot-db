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
/// Interaction logic for RegisterWindow.xaml
/// </summary>
public partial class RegisterWindow : Window
{
    UserManager _userManager = new();
    public RegisterWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btnRegister_Click(object sender, RoutedEventArgs e)
    {

        bool isUserNameAvailable = await _userManager.CheckUserNameAvailability(txtNewUsername.Text);
            
        try
        {
            if (String.IsNullOrWhiteSpace(txtNewUsername.Text) || txtNewUsername.Text.Length < 5)
            {
                throw new FormatException("Username must be at least 5 characters long!");
            }
            else if (!isUserNameAvailable)
            {
                throw new FormatException("Username is not available!");
            }
            else if (String.IsNullOrWhiteSpace(pswPassword.Password) || pswPassword.Password.Length < 6)
            {
                throw new FormatException("Password must be at least 6 characters long!");
            }
            else if(pswPassword.Password != pswRepeatPassword.Password)
            {
                throw new FormatException("The passwords don't match!");
            }
            else
            {
                txtErrorMessage.Clear();

                AppUser newUser = _userManager.CreateUser(txtNewUsername.Text, pswPassword.Password);

                await _userManager.AddUserToDb(newUser);

                MessageBox.Show($"{newUser.Username} was successfully registered!", "Success", MessageBoxButton.OK);

                MainWindow mainWindow = new();

                mainWindow.Show();
                this.Close();
            }
        }
        catch (FormatException ex)
        {
            txtErrorMessage.Text = ex.Message;
        }
    }

    /// <summary>
    /// Cancel and return to MainWindow.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new();

        mainWindow.Show();
        this.Close();
    }

}
