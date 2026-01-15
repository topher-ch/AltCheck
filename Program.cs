// See https://aka.ms/new-console-template for more information

using alternator_analyser.Models;
using alternator_analyser.Services;

var timingService = new TimingService();
var gameMonitorService = new GameMonitorService(timingService);

while (true)
{
    await gameMonitorService.CheckForBeatmapChange(CancellationToken.None);
    await Task.Delay(50);
}



