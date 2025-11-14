using alternator_analyser.Models;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;

namespace alternator_analyser.Services;

public class AlternationService
{
    public class AlternatedHitObject
    {
        public HitObject hitObject { get; set; }
        public HandAssignment  handAssignment { get; set; }
    }
    
    public List<AlternatedHitObject> MapAlternation(Beatmap beatmap, BeatSnapDivisor beatSnapDivisor)
    {
        return new List<AlternatedHitObject>();
    }
}