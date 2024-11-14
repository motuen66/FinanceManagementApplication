using BusinessObjects;
using DataAccessLayer;
using Services;
using System.Windows;
using System.Windows.Controls;

namespace FinanceManagementApp
{
    public partial class UserProfile : UserControl
    {
        private IUserService _userservice;
        public UserProfile()
        {
            InitializeComponent();
            _userservice = new UserService();
            getBalance();

            
        }
       
    
        private void getBalance()
        {
            var user = _userservice.GetUserById(1);
            if (user == null)
            {
                MessageBox.Show("User not found.");
            }
            else
            {
                int balance = user.Balance ?? 0; // Use ?? 0 to handle a null Balance value
                txtBalance.Text = balance.ToString();
               
            }
        }


        private void AddBalance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int amountToAdd = Int32.Parse(txtAddBalance.Text); // Lấy số tiền muốn cộng thêm
                String description = txtAddBalanceDes.Text;
                int userId = 1;

                IncomeTransaction transaction = new IncomeTransaction
                {
                   UserId = 1,
                   Amount = amountToAdd,
                   Description = description,
                   Date = DateTime.Now
                };

                // Thực hiện cập nhật số dư
                UserDAO.AddBalance(transaction);


                MessageBox.Show("Balance updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Cập nhật lại giá trị số dư hiển thị
                getBalance();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid number for balance.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





    }
}
