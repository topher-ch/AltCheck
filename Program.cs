using alternator_analyser.Control;
using alternator_analyser.Models;
using alternator_analyser.Services;

var gameMonitorService = new GameMonitorService();
var alternationService = new AlternationService();
{
    alternationService.RedDefaultHand = HandAssignment.RIGHT;
    alternationService.BlueDefaultHand = HandAssignment.RIGHT;
    alternationService.ResetOnFinishers = true;
    alternationService.ResetOnSingletapSnapDivisor = false;
    alternationService.SingletapSnapDivisor = BeatSnapDivisor.HALF;
}
var statsService = new StatsService();
{
    statsService.SingletapSnapDivisor = BeatSnapDivisor.HALF;
    statsService.LowerMarker = 10;
    statsService.UpperMarker = 25;
}
var servicesController = new ServicesController(gameMonitorService, alternationService, statsService);
var engine = new Engine(servicesController);

// need to subscribe UI to the StatsUpdated Action

var cts = new CancellationTokenSource();
var runTask = engine.RunAsync(cts.Token);

await Task.Delay(-1);
