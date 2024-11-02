using FinanceManagementApp.Domain;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceManagementApp
{
    /// <summary>
    /// Interaction logic for UserSidebar.xaml
    /// </summary>
    public partial class UserSidebar : UserControl
    {
        public List<SampleItem> SampleList { get; set; }
        public UserSidebar()
        {
            InitializeComponent();
            DataContext = this;

            SampleList = new()
        {
            new SampleItem
            {
                Title = "Payment",
                SelectedIcon = PackIconKind.CreditCard,
                UnselectedIcon = PackIconKind.CreditCardOutline,
                Notification = 1
            },
            new SampleItem
            {
                Title = "Home",
                SelectedIcon = PackIconKind.Home,
                UnselectedIcon = PackIconKind.HomeOutline,
            },
            new SampleItem
            {
                Title = "Special",
                SelectedIcon = PackIconKind.Star,
                UnselectedIcon = PackIconKind.StarOutline,
            },
            new SampleItem
            {
                Title = "Shared",
                SelectedIcon = PackIconKind.Users,
                UnselectedIcon = PackIconKind.UsersOutline,
            },
            new SampleItem
            {
                Title = "Files",
                SelectedIcon = PackIconKind.Folder,
                UnselectedIcon = PackIconKind.FolderOutline,
            },
            new SampleItem
            {
                Title = "Library",
                SelectedIcon = PackIconKind.Bookshelf,
                UnselectedIcon = PackIconKind.Bookshelf,
            },
        };
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //UserDataGrid.ItemsSource = userService.getAllUsers();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        => SampleList[0].Notification = SampleList[0].Notification is null ? 1 : null;

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
            => SampleList[0].Notification = SampleList[0].Notification is null ? "123+" : null;
    }
}
