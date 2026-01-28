using AltCheck.Models;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;

namespace AltCheck.Services;

public static class TimingService
{
    public static List<TimingPoint> NonInheritedTimingPoints(Beatmap beatmap)
    {
        return beatmap.TimingPoints
            .Where(tp => !tp.Inherited)
            .ToList();
    }
    
    public static Dictionary<BeatSnapDivisor, double> TimingPointBeatSnapLengths(TimingPoint redLine)
    {
        var beatSnapLengths = new Dictionary<BeatSnapDivisor, double>();
        var beatLength = redLine.BeatLength;
        beatSnapLengths[BeatSnapDivisor.WHOLE] = beatLength;
        beatSnapLengths[BeatSnapDivisor.HALF] = beatLength / 2;
        beatSnapLengths[BeatSnapDivisor.THIRD] = beatLength / 3;
        beatSnapLengths[BeatSnapDivisor.QUARTER] = beatLength / 4;
        beatSnapLengths[BeatSnapDivisor.FIFTH] = beatLength / 5;
        beatSnapLengths[BeatSnapDivisor.SIXTH] = beatLength / 6;
        beatSnapLengths[BeatSnapDivisor.SEVENTH] = beatLength / 7;
        beatSnapLengths[BeatSnapDivisor.EIGHTH] = beatLength / 8;
        beatSnapLengths[BeatSnapDivisor.NINTH] = beatLength / 9;
        beatSnapLengths[BeatSnapDivisor.TWELFTH] = beatLength / 12;
        beatSnapLengths[BeatSnapDivisor.SIXTEENTH] = beatLength / 16;
        return beatSnapLengths;
    }

    public static BeatSnapDivisor ClosestBeatSnapDivisor(
        double distance, 
        Dictionary<BeatSnapDivisor, double> beatSnapLengths)
    {
        var closest = BeatSnapDivisor.SIXTEENTH;
        var closestDistance = double.MaxValue;
        foreach (var divisor in Enum.GetValues<BeatSnapDivisor>())
        {
            var candidateDistance = distance - beatSnapLengths[divisor];
            if (candidateDistance < -5)
                continue;
            if (candidateDistance > closestDistance)
                continue;
            closest = divisor;
            closestDistance = candidateDistance;
        }
        return closest;
    }

    public static double BpmToBeatLength(double bpm)
    {
        return 60000 / bpm;
    }
}