﻿using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Internal;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using static System.Formats.Asn1.AsnWriter;


namespace FinanceManagementApp
{
    public partial class LoginWindow : Window
    {
        private readonly string[] Scopes = { "email", "profile" }; // Quyền truy cập email và thông tin hồ sơ
        private readonly string ClientId = "97989824227-03p89bb3vu53nb7r7grb81aio3qrphhm.apps.googleusercontent.com"; // Thay bằng Client ID của bạn
        private readonly string ClientSecret = "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"; // Thay bằng Client Secret của bạn

        private readonly string connectionString = "Data Source=VIETKHIMM\\KHIEMHUYNH;Initial Catalog=FinanceManagementApplication;User ID=sa;Password=30042004";

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

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM [User] WHERE username = @username AND password = @password";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 1)
                    {
                        MessageBox.Show("CHèo mừng pạn!", "đã rứa trời", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        Dashboard dashboard = new Dashboard(); 
                      
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            this.DragMove(); // Allows dragging the window
        }

        private async void GoogleSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Tạo ClientSecrets để sử dụng cho OAuth 2.0
                var clientSecrets = new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                };

                // Sử dụng GoogleWebAuthorizationBroker để thực hiện xác thực OAuth
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    Scopes,
                    "user",  // Dòng này là ID của người dùng cho OAuth
                    CancellationToken.None,
                    new NullDataStore()  // Không lưu thông tin đăng nhập
                );

                // Kiểm tra nếu người dùng đã đăng nhập thành công
                if (credential != null)
                {
                    var token = credential.Token.AccessToken;  // Lấy Access Token để tiếp tục sử dụng API

                    // Khởi tạo dịch vụ People API để lấy thông tin người dùng
                    var peopleService = new PeopleServiceService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "FinanceManagementApp"
                    });

                    var profileRequest = peopleService.People.Get("people/me");
                    profileRequest.RequestMaskIncludeField = "person.emailAddresses";
                    Person profile = await profileRequest.ExecuteAsync();

                    string email = profile.EmailAddresses?[0]?.Value;
                    if (email != null)
                    {
                        // Kiểm tra email trong cơ sở dữ liệu
                        if (IsEmailInDatabase(email))
                        {
                            MessageBox.Show("Google login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            Dashboard dashboard = new Dashboard();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Email not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to retrieve email from Google account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Google sign-in failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Google sign-in failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Hàm kiểm tra email trong cơ sở dữ liệu
        private bool IsEmailInDatabase(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM [User] WHERE username = @username";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", email);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Tạo đối tượng SignUpWindow mới
            SignUpWindow signUpWindow = new SignUpWindow();

            // Hiển thị cửa sổ SignUpWindow
            signUpWindow.Show();

            // Đóng cửa sổ hiện tại (nếu cần)
            this.Close();
        }
    }
}