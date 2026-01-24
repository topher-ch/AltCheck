using System.ComponentModel;
using alternator_analyser.Control;
using alternator_analyser.Models;
using alternator_analyser.Services;

namespace alternator_analyser.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private readonly ServicesController _controller;
    private readonly Engine _engine;
    
    public MainViewModel(ServicesController controller, Engine engine)
    {
        _controller = controller;
        _engine = engine;
        _engine.StatsUpdated += OnStatsUpdated;
    }

    private void OnStatsUpdated(Dictionary<(int, BeatSnapDivisor), StatsService.Counts> stats)
    {
        Console.WriteLine("MainViewModel: stats received");
    }
    
    protected void RaisePropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public string DonHandTitle { get; } = "Don";
    public string[] DonHandOptions { get; } = ["Left", "Right"];
    private int _donHand = 1;
    public int DonHand
    {
        get => _donHand;
        set
        {
            if (_donHand == value)
                return;
            _donHand = value;
            RaisePropertyChanged(nameof(DonHand));
            var hand = (_donHand == 0) ? HandAssignment.LEFT : HandAssignment.RIGHT;
            _controller.ChangeRedDefaultHand(hand);
        }
    }

    public string KatHandTitle { get; } = "Kat";
    public string[] KatHandOptions { get; } = ["Left", "Right"];
    private int _katHand = 1;
    public int KatHand
    {
        get => _katHand;
        set
        {
            if (_katHand == value)
                return;
            _katHand = value;
            RaisePropertyChanged(nameof(KatHand));
            var hand = (_katHand == 0) ? HandAssignment.LEFT : HandAssignment.RIGHT;
            _controller.ChangeBlueDefaultHand(hand);
        }
    }

    public string ResetOnFinisherTitle { get; } = "Reset on Finisher";
    private bool _resetOnFinisher = true;
    public bool ResetOnFinisher
    {
        get => _resetOnFinisher;
        set
        {
            if (_resetOnFinisher == value)
                return;
            _resetOnFinisher = value;
            RaisePropertyChanged(nameof(ResetOnFinisher));
            _controller.ChangeResetOnFinishers(_resetOnFinisher);
        }
    }

    public string ResetOnSnapTitle { get; } = "Reset on Snap";
    private bool _resetOnSnap = false;
    public bool ResetOnSnap
    {
        get => _resetOnSnap;
        set
        {
            if (_resetOnSnap == value)
                return;
            _resetOnSnap = value;
            RaisePropertyChanged(nameof(ResetOnSnap));
            _controller.ChangeResetOnSingletapSnapDivisor(_resetOnSnap);
        }
    }

    public string SnapDivisorTitle { get; } = "Snap";
    public string[] SnapDivisorOptions { get; } =
        ["1/1", "1/2", "1/3", "1/4", "1/5", "1/6", "1/7", "1/8", "1/9", "1/12", "1/16"];
    public double SnapDivisorOpacity { get; set; } = 1;
    private int _snapDivisor = 3;
    public int SnapDivisor
    {
        get => _snapDivisor;
        set
        {
            if (_snapDivisor == value)
                return;
            _snapDivisor = value;
            _snapBpm = null;
            SnapDivisorOpacity = 1;
            SnapBpmOpacity = 0.5;
            RaisePropertyChanged(nameof(SnapDivisor));
            RaisePropertyChanged(nameof(SnapBpm));
            RaisePropertyChanged(nameof(SnapDivisorOpacity));
            RaisePropertyChanged(nameof(SnapBpmOpacity));
            _controller.ChangeSingletapSnapDivisor(IntToBeatSnapDivisor(_snapDivisor));
            _controller.ChangeSingletapSnapDivisorByBpm(null);
        }
    }

    private static BeatSnapDivisor IntToBeatSnapDivisor(int n)
    {
        BeatSnapDivisor[] beatSnapDivisors =
        [
            BeatSnapDivisor.WHOLE, BeatSnapDivisor.HALF, BeatSnapDivisor.THIRD, BeatSnapDivisor.QUARTER,
            BeatSnapDivisor.FIFTH, BeatSnapDivisor.SIXTH, BeatSnapDivisor.SEVENTH, BeatSnapDivisor.EIGHTH,
            BeatSnapDivisor.NINTH, BeatSnapDivisor.TWELFTH, BeatSnapDivisor.SIXTEENTH
        ];
        return beatSnapDivisors[n];
    }

    public string SnapBpmTitle { get; } = "Snap by BPM";
    public double SnapBpmOpacity { get; set; } = 0.5;
    private int? _snapBpm = null;
    public string SnapBpm
    {
        get
        {
            if (_snapBpm == null)
                return "";
            return _snapBpm.Value.ToString();
        }
        set
        {
            if (!int.TryParse(value, out var bpm))
                return;
            if (bpm < 0 || bpm > 500)
                return;
            if (_snapBpm == bpm)
                return;
            _snapBpm = bpm;
            SnapBpmOpacity = 1;
            SnapDivisorOpacity = 0.5;
            RaisePropertyChanged(nameof(SnapBpm));
            RaisePropertyChanged(nameof(SnapBpmOpacity));
            RaisePropertyChanged(nameof(SnapDivisorOpacity));
            _controller.ChangeSingletapSnapDivisorByBpm(_snapBpm);
        }
    }

    public string LowerMarkerTitle { get; } = "Lower";
    private int _lowerMarker = 10;
    public string LowerMarker
    {
        get => _lowerMarker.ToString();
        set
        {
            if (!int.TryParse(value, out var lower))
                return;
            if (lower < 0 || lower > 500 || lower > _upperMarker)
                return;
            if (_lowerMarker == lower)
                return;
            _lowerMarker = lower;
            RaisePropertyChanged(nameof(LowerMarker));
            _controller.ChangeLowerMarker(_lowerMarker);
        }
    }
    
    public string UpperMarkerTitle { get; } = "Upper";
    private int _upperMarker = 25;
    public string UpperMarker
    {
        get => _upperMarker.ToString();
        set
        {
            if (!int.TryParse(value, out var upper))
                return;
            if (upper < 0 || upper > 500 || upper < _lowerMarker)
                return;
            if (_upperMarker == upper)
                return;
            _upperMarker = upper;
            RaisePropertyChanged(nameof(UpperMarker));
            _controller.ChangeUpperMarker(_upperMarker);
        }
    }
}