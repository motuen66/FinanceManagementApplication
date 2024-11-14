using FinanceManagementApp.Domain;
using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for GoalManagement.xaml
    /// </summary>
    public partial class GoalManagement : UserControl
    {
        public List<string> MonthLabels { get; set; }
        public SeriesCollection ExpenseSeries { get; set; }
        public GoalManagement()
        {
            InitializeComponent();
            userHeaderControl.ChangedTitleAndSubTitle(ScreenType.Goal);
            ExpenseSeries = new SeriesCollection
        {
            new ColumnSeries
            {
                Values = new ChartValues<double> { 1200.5, 1500.3, 1800.4, 1000.2, 2000.1, 2200.7, 1700.5, 1900.3, 1600.0, 1800.8, 1400.9, 2100.0 }
            }
        };
            DataContext = this;
        }
    }
}
