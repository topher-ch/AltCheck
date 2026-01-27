using AltCheck.ViewModels;
using Avalonia.Controls;

namespace AltCheck.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }
}