using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SavingTransaction
{
    public int? SavingGoalId { get; set; }

    public string? Note { get; set; }

    public int Amount { get; set; }

    public DateOnly? Date { get; set; }

    public virtual SavingGoal? SavingGoal { get; set; }
}
