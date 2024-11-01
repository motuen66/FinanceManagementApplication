﻿using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class BudgetItem
{
    public int Id { get; set; }

    public string? BudgetName { get; set; }

    public int? LimitAmount { get; set; }

    public bool? IsOverBudget { get; set; }

    public bool? IsDelete { get; set; }
}
