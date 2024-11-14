using BusinessObjects.Models;
using FinanceManagementApp.Domain;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.VisualBasic.ApplicationServices;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ISavingService savingService;
        public GoalManagement()
        {
            InitializeComponent();
            userHeaderControl.ChangedTitleAndSubTitle(ScreenType.Goal);
            savingService = new SavingService();
            LoadList(1);
        }

        public void LoadList(int userId)
        {
            var GoalList = savingService.GetSavingGoals(userId);
            isGoal.ItemsSource = GoalList;
            var CurrentGoal = savingService.GetSavingGoalById(userId);
            var monthlyExpenses = savingService.GetMonthlyExpenses(userId);
            int? progessSavedPercent = (CurrentGoal.CurrentAmount / CurrentGoal.GoalAmount) * 100;

            int countNotStarted = 0;
            int countInProgress = 0;
            int countFinished = 0;
            foreach (var item in GoalList)
            {
                if (item.GoalDate.HasValue && item.GoalDate.Value > DateOnly.FromDateTime(DateTime.Now) && item.CurrentAmount == 0)
                {
                    countNotStarted++;
                }
                if (item.IsCompleted == true)
                {
                    countFinished++;
                }
                if (item.GoalDate.HasValue && item.GoalDate.Value > DateOnly.FromDateTime(DateTime.Now) && item.CurrentAmount > 0)
                {
                    countInProgress++;
                }
            }
            (List<string> monthLabels, List<double> doubles) chartData = savingService.Labels();
            MonthLabels = chartData.monthLabels;
            ExpenseSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Expenses",
                    Values = new ChartValues<double>(chartData.doubles)
                }
            };

            var chartInfor = new { 
            Labels = MonthLabels,
            Series = ExpenseSeries
            };
            DataContext = chartInfor;   
            Debug.WriteLine(ExpenseSeries.Count());
            txtNumberOfNotStartedStatus.Text = countNotStarted.ToString();
            txtNumberOfInProgressStatus.Text = countInProgress.ToString();
            txtNumberOfFinishedStatus.Text = countFinished.ToString();
            txtTotalGoal.Text = GoalList.Count.ToString();
        }
    }
}
