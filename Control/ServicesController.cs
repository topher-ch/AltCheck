using alternator_analyser.Models;
using alternator_analyser.Services;
using OsuParsers.Decoders;
using OsuParsers.Enums;

namespace alternator_analyser.Control;

public class ServicesController
{
    private readonly GameMonitorService _gameMonitorService;
    private readonly AlternationService _alternationService;
    private readonly StatsService _statsService;

    public ServicesController(
        GameMonitorService gameMonitorService,
        AlternationService alternationService,
        StatsService statsService)
    {
        _gameMonitorService = gameMonitorService;
        _alternationService = alternationService;
        _statsService = statsService;
    }

    private volatile bool _settingsDirty = false;

    public async Task<Dictionary<(int, BeatSnapDivisor), StatsService.Counts>?> GetStats(CancellationToken token)
    {
        var (changed, path) = await _gameMonitorService.CheckForBeatmapChange(token);
        if (_settingsDirty || changed)
        {
            Console.WriteLine(path);
            var beatmap = BeatmapDecoder.Decode(path);
            if (beatmap.GeneralSection.Mode != Ruleset.Taiko)
            {
                MarkSettingsClean();
                return null;
            }
            var alternatedHitObjects = _alternationService.MapAlternatedHitObjects(beatmap);
            var patternCounts = _statsService.CountPatterns(beatmap, alternatedHitObjects);
            foreach (var patternCount in patternCounts)
            {
                Console.WriteLine($"Length: {patternCount.Key.Item1}, BeatSnapDivisors: {patternCount.Key.Item2}");
                Console.WriteLine($"L: {patternCount.Value.LCount}, R: {patternCount.Value.RCount}, B: {patternCount.Value.BCount}");
            }
            MarkSettingsClean();
            return patternCounts;
        }
        MarkSettingsClean();
        return null;
    }

    private void MarkSettingsDirty()
    {
        if (!_settingsDirty)
            _settingsDirty = true;
    }

    private void MarkSettingsClean()
    {
        if (_settingsDirty)
            _settingsDirty = false;
    }
    
    public void ChangeRedDefaultHand(HandAssignment handAssignment)
    {
        _alternationService.RedDefaultHand = handAssignment;
        MarkSettingsDirty();
    }

    public void ChangeBlueDefaultHand(HandAssignment handAssignment)
    {
        _alternationService.BlueDefaultHand = handAssignment;
        MarkSettingsDirty();
    }

    public void ChangeResetOnFinishers(bool resetOnFinishers)
    {
        _alternationService.ResetOnFinishers = resetOnFinishers;
        MarkSettingsDirty();
    }

    public void ChangeResetOnSingletapSnapDivisor(bool resetOnSingletapSnapDivisor)
    {
        _alternationService.ResetOnSingletapSnapDivisor = resetOnSingletapSnapDivisor;
        MarkSettingsDirty();
    }

    public void ChangeSingletapSnapDivisor(BeatSnapDivisor singletapSnapDivisor)
    {
        _alternationService.SingletapSnapDivisor = singletapSnapDivisor;
        _statsService.SingletapSnapDivisor = singletapSnapDivisor;
        MarkSettingsDirty();
    }

    public void ChangeSingletapSnapDivisorByBpm(double? bpm)
    {
        double? beatLength;
        if (bpm != null)
            beatLength = TimingService.BpmToBeatLength(bpm.Value);
        else
            beatLength = null;
        _alternationService.SingletapBeatLength = beatLength;
        _statsService.SingletapBeatLength = beatLength;
        MarkSettingsDirty();
    }

    public void ChangeLowerMarker(int lowerMarker)
    {
        _statsService.LowerMarker = lowerMarker;
        MarkSettingsDirty();
    }

    public void ChangeUpperMarker(int upperMarker)
    {
        _statsService.UpperMarker = upperMarker;
        MarkSettingsDirty();
    }
}