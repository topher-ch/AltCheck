// See https://aka.ms/new-console-template for more information

using alternator_analyser.Models;
using alternator_analyser.Services;

var statsService = new StatsService();
{
    statsService.SingletapSnapDivisor = BeatSnapDivisor.HALF;
}
var alternationService = new AlternationService(statsService);
{
    alternationService.RedDefaultHand = HandAssignment.RIGHT;
    alternationService.BlueDefaultHand = HandAssignment.RIGHT;
    alternationService.ResetOnFinishers = true;
    alternationService.ResetOnSingletapSnapDivisor = false;
    alternationService.SingletapSnapDivisor = BeatSnapDivisor.HALF;
}
var timingService = new TimingService(alternationService);
var gameMonitorService = new GameMonitorService(timingService);

while (true)
{
    await gameMonitorService.CheckForBeatmapChange(CancellationToken.None);
    await Task.Delay(50);
}



