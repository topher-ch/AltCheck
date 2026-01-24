using alternator_analyser.ViewModels;
using Avalonia.Controls;

namespace alternator_analyser.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
    }
}