using AltCheck.Models;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Beatmaps.Objects.Taiko;
using OsuParsers.Enums.Beatmaps;

namespace AltCheck.Services;

public class AlternationService
{
    public HandAssignment RedDefaultHand;
    public HandAssignment BlueDefaultHand;
    public bool ResetOnFinishers;
    public bool ResetOnSingletapSnapDivisor;
    public BeatSnapDivisor SingletapSnapDivisor;
    public double? SingletapBeatLength;

    public class AlternatedHitObject(HitObject hitObject, HandAssignment handAssignment)
    {
        public readonly HitObject HitObject = hitObject;
        public readonly HandAssignment HandAssignment = handAssignment;
    }

    public List<AlternatedHitObject> MapAlternatedHitObjects(Beatmap beatmap)
    {
        // get red lines and initialize first red line
        var redLines = TimingService.NonInheritedTimingPoints(beatmap);
        if (redLines.Count == 0)
            throw new Exception();
        var redLineIdx = 0;
        var beatSnapLengths = TimingService.TimingPointBeatSnapLengths(redLines[redLineIdx]);
        if (SingletapBeatLength is not null)
            SingletapSnapDivisor = TimingService.ClosestBeatSnapDivisor(SingletapBeatLength.Value / 2, beatSnapLengths);
        
        // initialize list of alternated hit objects
        var alternatedHitObjects = new List<AlternatedHitObject>();
        AlternatedHitObject? prevAlternatedHitObject = null;
        foreach (var hitObject in beatmap.HitObjects)
        {
            // update red line if necessary
            while (prevAlternatedHitObject is not null
                && redLineIdx + 1 < redLines.Count
                && prevAlternatedHitObject.HitObject.StartTime >= redLines[redLineIdx + 1].Offset)
            {
                beatSnapLengths = TimingService.TimingPointBeatSnapLengths(redLines[redLineIdx+1]);
                if (SingletapBeatLength is not null)
                    SingletapSnapDivisor = TimingService.ClosestBeatSnapDivisor(SingletapBeatLength.Value / 2, beatSnapLengths);
                redLineIdx++;
            }

            // add new alternated hit object to the list
            var nextHandAssigment = NextHandAssignment(prevAlternatedHitObject, hitObject, beatSnapLengths);
            var alternatedHitObject = new AlternatedHitObject(hitObject, nextHandAssigment);
            alternatedHitObjects.Add(alternatedHitObject);
            prevAlternatedHitObject = alternatedHitObject;
        }
        return alternatedHitObjects;
    }
    
    public HandAssignment NextHandAssignment(
        AlternatedHitObject? prevAlternatedHitObject, 
        HitObject nextHitObject, 
        Dictionary<BeatSnapDivisor, double> beatSnapLengths)
    {
        // if nextHitObject is a TaikoDrumRoll or TaikoSpinner then always BOTH
        if (nextHitObject is TaikoDrumroll or TaikoSpinner)
            return HandAssignment.BOTH;
        
        // otherwise nextHitObject is a TaikoHit, throw an exception otherwise
        if (nextHitObject is not TaikoHit taikoHit)
            throw new Exception();
        
        // if ResetOnFinishers is true and nextHitObject is a Finisher then BOTH
        if (ResetOnFinishers && taikoHit.IsBig)
            return HandAssignment.BOTH;
        
        // if the previous hit object does not exist, default
        if (prevAlternatedHitObject == null)
            return (taikoHit.Color == TaikoColor.Red) ? RedDefaultHand : BlueDefaultHand;
        
        // otherwise the previous hit object does exist
        // if the previous hit object is BOTH, default
        if (prevAlternatedHitObject.HandAssignment == HandAssignment.BOTH)
            return (taikoHit.Color == TaikoColor.Red) ? RedDefaultHand : BlueDefaultHand;
        
        // if the previous hit object is larger than the SingletapSnapDivisor and ResetOnSingletapSnapDivisor is true,
        // default
        var distance = nextHitObject.StartTime - prevAlternatedHitObject.HitObject.StartTime;
        var closestBeatSnapDivisor = TimingService.ClosestBeatSnapDivisor(distance, beatSnapLengths);
        if (beatSnapLengths[closestBeatSnapDivisor] >= beatSnapLengths[SingletapSnapDivisor] 
            && ResetOnSingletapSnapDivisor)
            return (taikoHit.Color == TaikoColor.Red) ? RedDefaultHand : BlueDefaultHand;
        
        // otherwise alternate
        return (prevAlternatedHitObject.HandAssignment == HandAssignment.LEFT) 
            ? HandAssignment.RIGHT : HandAssignment.LEFT;
    }
}