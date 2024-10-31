using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public int? Balance { get; set; }

    public string? AvatarPath { get; set; }

    public virtual ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();
}
