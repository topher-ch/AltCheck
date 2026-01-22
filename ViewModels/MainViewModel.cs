using alternator_analyser.Control;
using alternator_analyser.Models;
using alternator_analyser.Services;

namespace alternator_analyser.ViewModels;

public class MainViewModel
{
    private readonly ServicesController _controller;
    private readonly Engine _engine;

    public MainViewModel(ServicesController controller, Engine engine)
    {
        _controller = controller;
        _engine = engine;
        _engine.StatsUpdated += OnStatsUpdated;
    }

    private void OnStatsUpdated(Dictionary<(int, BeatSnapDivisor), StatsService.Counts> stats)
    {
        Console.WriteLine("MainViewModel: stats received");
    }
}