using BusinessObjects;
using DataAccessLayer;
using MaterialDesignThemes.Wpf;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for IncomeManagement.xaml
    /// </summary>
    public partial class IncomeManagement : UserControl, INotifyPropertyChanged
    {
        private IncomeTransaction _selectedTransaction;
        private readonly IIncomeTransactionService _service;
        // Public Property to hold the selected transaction
        public IncomeTransaction SelectedIncomeTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (_selectedTransaction != value)
                {
                    _selectedTransaction = value;
                    OnPropertyChanged(nameof(SelectedIncomeTransaction)); 
                }
            }
        }

        public IncomeManagement()
        {
            InitializeComponent();
            LoadIncomeTransaction();
            _service = new IncomeTransactionService();
            DataContext = this;
        }
        public ObservableCollection<IncomeTransaction> IncomeTransaction { get; set; }
        private void LoadIncomeTransaction()
        {
            dgIncomeTransactions.ItemsSource = IncomeTransactionDAO.GetAllIncomeTransaction(1);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        private void btnCreateIncome(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu các trường thông tin chưa được điền đầy đủ
            if (string.IsNullOrEmpty(txtAmount?.Text) || string.IsNullOrEmpty(txtDescription?.Text) || dpTransactionDate?.SelectedDate == null)
            {
                MessageBox.Show("Please fill all information.");
                return;
            }

                IncomeTransaction it = new IncomeTransaction
                {

                    UserId = 1,
                    Amount = Int32.Parse(txtAmount.Text),
                    Description = txtDescription.Text,
                    Date = dpTransactionDate.SelectedDate
                };

            try
            {
                // Kiểm tra nếu _service không phải là null
                if (_service == null)
                {
                    MessageBox.Show("Service not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _service.CreateIncomeTransaction(it);
                LoadIncomeTransaction();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có sự cố khi tạo giao dịch
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
