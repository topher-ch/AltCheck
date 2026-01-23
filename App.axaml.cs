using alternator_analyser.Control;
using alternator_analyser.Models;
using alternator_analyser.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using alternator_analyser.ViewModels;
using alternator_analyser.Views;

namespace alternator_analyser;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var gameMonitorService = new GameMonitorService();
            var alternationService = new AlternationService();
            alternationService.RedDefaultHand = HandAssignment.RIGHT;
            alternationService.BlueDefaultHand = HandAssignment.RIGHT;
            alternationService.ResetOnFinishers = true;
            alternationService.ResetOnSingletapSnapDivisor = false;
            alternationService.SingletapSnapDivisor = BeatSnapDivisor.HALF;
            var statsService = new StatsService();
            statsService.SingletapSnapDivisor = BeatSnapDivisor.HALF;
            statsService.LowerMarker = 10;
            statsService.UpperMarker = 25;

            var servicesController = new ServicesController(gameMonitorService, alternationService, statsService);
            var engine = new Engine(servicesController);

            var mainViewModel = new MainViewModel(servicesController, engine);
            
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            desktop.MainWindow = mainWindow;
            
            var cts = new CancellationTokenSource();
            _ = engine.RunAsync(cts.Token);
        }

        base.OnFrameworkInitializationCompleted();
    }
}