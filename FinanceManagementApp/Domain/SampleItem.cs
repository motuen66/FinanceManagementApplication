﻿using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;

namespace FinanceManagementApp.Domain;

public class SampleItem : ViewModelBase
{
    public string? Title { get; set; }
    public PackIconKind SelectedIcon { get; set; }
    public PackIconKind UnselectedIcon { get; set; }
    private object? _notification = null;

    public object? Notification
    {
        get { return _notification; }
        set { Set(ref _notification, value); }
    }

}