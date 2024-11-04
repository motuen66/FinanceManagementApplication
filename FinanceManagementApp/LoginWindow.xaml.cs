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
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // For demonstration, suppose a simple email/password check
            if (email == "test@example.com" && password == "password123")
            {
                MessageBox.Show("Sign-in successful!", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
                // Navigate to another window or perform actions for successful login
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void SignUpLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Handle the sign-up link click event here.
            // For example, navigate to the Sign-Up window:
            SignUpWindow signUpWindow = new SignUpWindow(); // assuming a SignUpWindow exists
            signUpWindow.Show();
            this.Close(); // Optionally close the current window
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("helloooooooooooooooooooooooooo");
        }
    }
}
