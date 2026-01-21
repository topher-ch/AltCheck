using alternator_analyser.Models;
using OsuParsers.Beatmaps;

namespace alternator_analyser.Services;

public class StatsService
{
    public BeatSnapDivisor SingletapSnapDivisor;
    public double? SingletapBeatLength;
    public int LowerMarker;
    public int UpperMarker;
    
    public record Counts
    {
        public int LCount { get; set; }
        public int RCount { get; set; }
        public int BCount { get; set; }
    }

    public Dictionary<(int, BeatSnapDivisor), Counts> CountPatterns(Beatmap beatmap, 
        List<AlternationService.AlternatedHitObject> alternatedHitObjects)
    {
        // get red lines and initialize first red line
        var redLines = TimingService.NonInheritedTimingPoints(beatmap);
        if (redLines.Count == 0)
            throw new Exception();
        var redLineIdx = 0;
        var beatSnapLengths = TimingService.TimingPointBeatSnapLengths(redLines[redLineIdx]);
        if (SingletapBeatLength is not null)
            SingletapSnapDivisor = TimingService.ClosestBeatSnapDivisor(SingletapBeatLength.Value, beatSnapLengths);
        // initialize dictionary taking the key "(length, beatSnapDivisorFlag)" to the value "Counts"
        var patternCounts = new Dictionary<(int length, BeatSnapDivisor beatSnapDivisorFlags), Counts>();
        var patternHandStart = alternatedHitObjects[0].HandAssignment;
        var patternLength = 1;
        BeatSnapDivisor beatSnapDivisors = 0;
        // iterate through alternated hit objects
        for (var i = 0; i < alternatedHitObjects.Count - 1; i++)
        {
            var prevHitObject = alternatedHitObjects[i];
            var nextHitObject = alternatedHitObjects[i + 1];
            // update red line if necessary
            if (redLineIdx + 1 < redLines.Count
                && prevHitObject.HitObject.StartTime >= redLines[redLineIdx + 1].Offset)
            {
                beatSnapLengths = TimingService.TimingPointBeatSnapLengths(redLines[redLineIdx+1]);
                if (SingletapBeatLength is not null)
                    SingletapSnapDivisor = TimingService.ClosestBeatSnapDivisor(SingletapBeatLength.Value, beatSnapLengths);
                redLineIdx++;
            }
            // calculate distance and find closest beatSnapDivisor
            var distance = nextHitObject.HitObject.StartTime - prevHitObject.HitObject.StartTime;
            var closestBeatSnapDivisor = TimingService.ClosestBeatSnapDivisor(distance, beatSnapLengths);
            // if the distance is smaller than the SingletapSnapDivisor, update length and divisors, then continue
            if (beatSnapLengths[closestBeatSnapDivisor] < beatSnapLengths[SingletapSnapDivisor])
            {
                patternLength++;
                beatSnapDivisors |= closestBeatSnapDivisor;
                continue;
            }
            // if the distance is larger than the SingletapSnapDivisor, increment the respective count
            // first, clamp lengths larger than markers downwards to the closest marker
            if (patternLength > UpperMarker)
                patternLength = UpperMarker;
            else if (patternLength > LowerMarker)
                patternLength = LowerMarker;
            // then, initialize counts if uninitialized
            if (!patternCounts.ContainsKey((patternLength, beatSnapDivisors)))
                patternCounts[(patternLength, beatSnapDivisors)] = new Counts();
            // finally, increment the respective count
            switch (patternHandStart)
            {
                case HandAssignment.LEFT:
                    patternCounts[(patternLength, beatSnapDivisors)].LCount++;
                    break;
                case HandAssignment.RIGHT:
                    patternCounts[(patternLength, beatSnapDivisors)].RCount++;
                    break;
                case HandAssignment.BOTH:
                    patternCounts[(patternLength, beatSnapDivisors)].BCount++;
                    break;
            }
            // reset pattern trackers
            patternHandStart = nextHitObject.HandAssignment;
            patternLength = 1;
            beatSnapDivisors = 0;
        }
        // count the last pattern
        if (patternLength > UpperMarker)
            patternLength = UpperMarker;
        else if (patternLength > LowerMarker)
            patternLength = LowerMarker;
        if (!patternCounts.ContainsKey((patternLength, beatSnapDivisors)))
            patternCounts[(patternLength, beatSnapDivisors)] = new Counts();
        switch (patternHandStart)
        {
            case HandAssignment.LEFT:
                patternCounts[(patternLength, beatSnapDivisors)].LCount++;
                break;
            case HandAssignment.RIGHT:
                patternCounts[(patternLength, beatSnapDivisors)].RCount++;
                break;
            case HandAssignment.BOTH:
                patternCounts[(patternLength, beatSnapDivisors)].BCount++;
                break;
        }
        return patternCounts;
    }
}