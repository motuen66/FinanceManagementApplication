namespace BusinessObjects;

public partial class BudgetItem
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? BudgetName { get; set; }
    public int CurrentAmount { get; set; }

    public int LimitAmount { get; set; }

    public bool? IsOverBudget { get; set; }
    public string BudgetStatus => IsOverBudget == true ? "Over" : "On track";

    public bool? IsDelete { get; set; }

    public int ExpensePercent
    {
        get
        {
            return (int)((double)CurrentAmount / LimitAmount * 100);
        }
    }

    public string CurrentAmountStr => Util.Instance.FormatMoney(CurrentAmount);
    public string LimitAmountStr => Util.Instance.FormatMoney((int)LimitAmount);

    public virtual User? User { get; set; }
}