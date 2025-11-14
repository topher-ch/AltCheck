using OsuParsers.Beatmaps;
using alternator_analyser.Models;

namespace alternator_analyser.Services;

public class TimingService
{
    public void TimingPoints(Beatmap beatmap)
    {
        
    }

    public BeatSnapDivisor ProminentBeatSnapDivisor(Beatmap beatmap)
    {
        return BeatSnapDivisor.WHOLE;
    }
}