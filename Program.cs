// See https://aka.ms/new-console-template for more information

using alternator_analyser.Models;
using alternator_analyser.Services;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;

TimingService timingService = new TimingService();
AlternationService alternationService = new AlternationService();
alternationService.RedDefaultHand = HandAssignment.RIGHT;
alternationService.BlueDefaultHand = HandAssignment.RIGHT;
alternationService.ResetOnFinishers = true;
alternationService.ResetOnSingletapSnapDivisor = false;
StatsService statsService = new StatsService();

Beatmap beatmap =
    BeatmapDecoder.Decode(
        "/home/christopher/RiderProjects/alternator_analyser/Cansol - Train of Thought (Nurend) [Last Stop].osu");
BeatSnapDivisor? singletapBeatSnapDivisor = timingService.SingletapBeatSnapDivisor(beatmap);
if (singletapBeatSnapDivisor == null)
{
    Console.WriteLine("no singletap beat snap divisor found");
    return;
}
List<AlternationService.AlternatedHitObject> alternatedHitObjects = alternationService.MapAlternation(beatmap, singletapBeatSnapDivisor.Value);
foreach (AlternationService.AlternatedHitObject alternatedObject in alternatedHitObjects)
{
    Console.WriteLine("Offset: " + alternatedObject.hitObject.StartTime);
    Console.WriteLine("Hand Assignment: " + alternatedObject.handAssignment);
}
Console.WriteLine("");
Dictionary<(BeatSnapDivisor beatSnapDivisor, int length), StatsService.Counts> stats = statsService.Stats(beatmap, BeatSnapDivisor.QUARTER, alternatedHitObjects);
foreach ((BeatSnapDivisor beatSnapDivisor, int length) in stats.Keys)
{
    Console.WriteLine("beatSnapDivisor: " + beatSnapDivisor);
    Console.WriteLine("length: " + length);
    Console.WriteLine("leftCount: " + stats[(beatSnapDivisor, length)].LeftCount);
    Console.WriteLine("rightCount: " + stats[(beatSnapDivisor, length)].RightCount);
    Console.WriteLine("bothCount: " + stats[(beatSnapDivisor, length)].BothCount);
}
StatsService.Counts overall = statsService.OverallCounts(stats);
StatsService.Counts overallNoSingletaps = statsService.OverallCountsNoSingletaps(stats);
Console.WriteLine("overall: " + overall);
Console.WriteLine("overallNoSingletaps: " + overallNoSingletaps);