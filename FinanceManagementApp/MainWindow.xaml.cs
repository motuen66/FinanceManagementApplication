﻿using FinanceManagementApp.Domain;
using MaterialDesignThemes.Wpf;
using Services;
using System.Configuration;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
        }
        private void OnSidebar_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is UserSidebar sidebar && sidebar.SidebarListBox.SelectedItem is SampleItem selectedItem)
            {
                switch (selectedItem.Title)
                {
                    case "Dashboard":
                        MainContent.Content = new Dashboard();
                        break;
                    case "Budget":
                        MainContent.Content = new BudgetManagement();
                        break;
                    case "Income":
                        MainContent.Content = new IncomeManagement();
                        break;
                    case "Expense":
                        MainContent.Content = new ExpenseManagement();
                        break;
                    case "Settings":
                        MainContent.Content = new Settings();
                        break;
                    case "Goals":
                        MainContent.Content = new GoalManagement();
                        break;
                    default:
                        MainContent.Content = new Dashboard();
                        break;
                }
            }
        }
    }
}