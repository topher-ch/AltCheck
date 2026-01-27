using AltCheck.Control;
using AltCheck.Models;

namespace AltCheck.Services;

public class Engine
{
    private readonly ServicesController _controller;
    public event Action<Dictionary<(int, BeatSnapDivisor), StatsService.Counts>>? StatsUpdated;

    public Engine(ServicesController controller)
    {
        _controller = controller;
    }

    public async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var newStats = await _controller.GetStats(token);
            if (newStats != null)
                StatsUpdated?.Invoke(newStats);
            await Task.Delay(50, token);
        }
    }
}