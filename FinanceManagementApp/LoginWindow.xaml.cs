using BusinessObjects;
using Services;
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

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly UserService _userService;
        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // For demonstration, suppose a simple email/password check
            if (Login(username, password))
            {
                MessageBox.Show("Sign-in successful!", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SignUpLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow(); // assuming a SignUpWindow exists
            signUpWindow.Show();
            this.Close(); // Optionally close the current window
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Login di men");
        }

        private bool Login(string userName, string password)
        {
            try
            {
                var user = _userService.GetUser(userName, password);
                if (user != null)
                {
                    UserSession.Instance.SetUser(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while logging in. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
