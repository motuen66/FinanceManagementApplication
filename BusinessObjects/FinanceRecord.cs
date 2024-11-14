using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class FinanceRecord
{
    public int? UserId { get; set; }

    public int? TotalIncome { get; set; }

    public int? TotalExpense { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public virtual User? User { get; set; }
}
