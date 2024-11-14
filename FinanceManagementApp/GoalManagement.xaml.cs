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
using System.Net.Http.Headers;
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
            //userHeaderControl.ChangedTitleAndSubTitle(ScreenType.Goal);
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

        private void AddGoalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SavingGoal savingGoal = new SavingGoal();
                savingGoal.UserId = 1;
                savingGoal.Title = txtAddTitle.Text;
                savingGoal.Description = txtAddDescription.Text;
                savingGoal.CurrentAmount = 0;
                savingGoal.GoalAmount = Int32.Parse(txtAddAmmount.Text);
                if (dpAddGoalDate.SelectedDate.HasValue)
                {
                    savingGoal.GoalDate = DateOnly.FromDateTime(dpAddGoalDate.SelectedDate.Value);
                }
                else
                {
                    MessageBox.Show("Please select a valid goal date.");
                    return;
                }
                savingService.CreateSavingGoal(savingGoal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occurs when adding new goal");
            }
            finally
            {
                LoadList(1);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SavingGoal savingGoal = new SavingGoal();
                savingGoal.UserId = 1;
                savingGoal.Id = Int32.Parse(txtGoalId.Text);
                savingGoal.Title = txtTitle.Text;
                savingGoal.Description = txtUpdateDescription.Text;
                savingGoal.CurrentAmount = 0;
                savingGoal.GoalAmount = Int32.Parse(txtUpdateGoalAmmount.Text);
                if(dpUpdateGoalDate.SelectedDate.HasValue)
                {
                    savingGoal.GoalDate = DateOnly.FromDateTime(dpUpdateGoalDate.SelectedDate.Value);
                }
                else
                {
                    MessageBox.Show("Please select a valid goal date.");
                    return;
                }
                savingService.UpdateSavingGoal(savingGoal);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occur when updating");
            } finally
            {
                LoadList(1);
            }
        }

        private void btnUpSelectionChange(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is SavingGoal savingGoal)
            {
                txtGoalId.Text = savingGoal.Id.ToString();
                txtTitle.Text = savingGoal.Title;
                txtUpdateDescription.Text = savingGoal.Description;
                txtUpdateGoalAmmount.Text = savingGoal.GoalAmount.ToString();
                dpUpdateGoalDate.SelectedDate = savingGoal.GoalDate.HasValue
                                                ? savingGoal.GoalDate.Value.ToDateTime(TimeOnly.MinValue)
                                                : (DateTime?)null;
            }
        }
    }
}
