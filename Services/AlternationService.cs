using alternator_analyser.Models;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;

namespace alternator_analyser.Services;

public class AlternationService
{
    public HandAssignment redDefaultHand;
    public HandAssignment blueDefaultHand;
    public bool resetOnFinishers;
    public bool resetOnSingletapSnapDivisor;
    
    public class AlternatedHitObject
    {
        public HitObject hitObject { get; set; }
        public HandAssignment  handAssignment { get; set; }
    }
    
    public List<AlternatedHitObject> MapAlternation(Beatmap beatmap, BeatSnapDivisor beatSnapDivisor)
    {
        List<AlternatedHitObject> alternatedHitObjects = new List<AlternatedHitObject>();
        
        return alternatedHitObjects;
    }
}