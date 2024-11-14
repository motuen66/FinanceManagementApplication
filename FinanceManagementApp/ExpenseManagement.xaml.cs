using BusinessObjects;
using DataAccessLayer;
using FinanceManagementApp.Domain;
using MaterialDesignThemes.Wpf;
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
using static MaterialDesignThemes.Wpf.Theme;

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for ExpenseManagement.xaml
    /// </summary>
    public partial class ExpenseManagement : UserControl, INotifyPropertyChanged
    {
        private ExpenseTransaction _selectedTransaction;

        // Public Property to hold the selected transaction
        public ExpenseTransaction SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (_selectedTransaction != value)
                {
                    _selectedTransaction = value;
                    OnPropertyChanged(nameof(SelectedTransaction));
                }
            }
        }
        public ObservableCollection<ExpenseTransaction> ExpenseTransactions { get; set; }
        public string CurrentDate => DateTime.Now.ToString("dd/MM/yyyy");

        public event PropertyChangedEventHandler PropertyChanged;

        public ExpenseManagement()
        {
            InitializeComponent();
            LoadExpenseTransactions();
            DataContext = this;
        }

        private void LoadExpenseTransactions()
        {
            ExpenseTransactions = new ObservableCollection<ExpenseTransaction>(ExpenseTransactionDAO.GetAllExpenseTransactionsById(3));
        }

        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {

        }
        private void dgExpenseTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgExpenseTransactions.SelectedItem is ExpenseTransaction selected)
            {
                cmbBudget.SelectedValue = selected.BudgetId;
                txtAmount.Text = selected.Amount.ToString();
                txtNote.Text = selected.Note.ToString();
                dpTransactionDate.SelectedDate = selected.Date?.ToDateTime(TimeOnly.MinValue);
                Console.WriteLine(cmbBudget.SelectedValue);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Assuming SelectedTransaction is the transaction being edited
           
            
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected transaction from the clicked menu item
            var menuItem = sender as MenuItem;
            var transaction = menuItem.DataContext as ExpenseTransaction;

            if (transaction != null)
            {
                // Set the selected transaction to the dialog's fields
                cmbBudget.SelectedValue = transaction.BudgetId;
                txtAmount.Text = transaction.Amount.ToString();
                txtNote.Text = transaction.Note.ToString();
                dpTransactionDate.SelectedDate = transaction.Date?.ToDateTime(TimeOnly.MinValue);

                // Open the dialog
                UpdateDialogHost.IsOpen = true;
            }
        }


        private void UpdateTransaction(ExpenseTransaction transaction)
        {
            var updatedTransaction = ExpenseTransactions.FirstOrDefault(t =>
         t.Date == transaction.Date &&
         t.Amount == transaction.Amount &&
         t.Note == transaction.Note &&
         t.Budget == transaction.Budget);
            if (updatedTransaction != null)
            {
                updatedTransaction.Amount = transaction.Amount;
                updatedTransaction.Note = transaction.Note;
                updatedTransaction.Date = transaction.Date;
                updatedTransaction.Budget = transaction.Budget;
                // Notify DataGrid to update view
                OnPropertyChanged(nameof(ExpenseTransactions));
            }
        }

        private void DeleteTransaction(ExpenseTransaction transaction)
        {
            // Logic to delete the transaction
        }
    }
}

