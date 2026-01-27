using System.ComponentModel;
using AltCheck.Control;
using AltCheck.Models;
using AltCheck.Services;
using LiveChartsCore.SkiaSharpView;

namespace AltCheck.ViewModels;

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

    private Axis[] _xAxes = 
    [
        new Axis
        {
            Name = "Length",
            Labels = ["2", "3", "4", "5", "6", "7", "8", "9", "10-24", "25+"]
        }
    ];
    public Axis[] XAxes
    {
        get => _xAxes;
        set
        {
            _xAxes = value;
            RaisePropertyChanged(nameof(XAxes));
        }
    }
    
    public int PositiveAxisMinOrMax { get; set; } = 50;
    public int NegativeAxisMinOrMax { get; set; } = -50;
    
    public string WholeName { get; } = "1/1";
    public string WholeColour { get; } = "";
    public IReadOnlyList<int> WholeLeftValues { get; set; } = [];
    public IReadOnlyList<int> WholeRightValues { get; set; } = [];
    public IReadOnlyList<int> WholeBothValues { get; set; } = [];
    
    public string HalfName { get; } = "1/2";
    public string HalfColour { get; } = "";
    public IReadOnlyList<int> HalfLeftValues { get; set; } = [];
    public IReadOnlyList<int> HalfRightValues { get; set; } = [];
    public IReadOnlyList<int> HalfBothValues { get; set; } = [];
    
    public string ThirdName { get; } = "1/3";
    public string ThirdColour { get; } = "";
    public IReadOnlyList<int> ThirdLeftValues { get; set; } = [];
    public IReadOnlyList<int> ThirdRightValues { get; set; } = [];
    public IReadOnlyList<int> ThirdBothValues { get; set; } = [];
    
    public string QuarterName { get; } = "1/4";
    public string QuarterColour { get; } = "";
    public IReadOnlyList<int> QuarterLeftValues { get; set; } = [];
    public IReadOnlyList<int> QuarterRightValues { get; set; } = [];
    public IReadOnlyList<int> QuarterBothValues { get; set; } = [];
    
    public string FifthName { get; } = "1/5";
    public string FifthColour { get; } = "";
    public IReadOnlyList<int> FifthLeftValues { get; set; } = [];
    public IReadOnlyList<int> FifthRightValues { get; set; } = [];
    public IReadOnlyList<int> FifthBothValues { get; set; } = [];
    
    public string SixthName { get; } = "1/6";
    public string SixthColour { get; } = "";
    public IReadOnlyList<int> SixthLeftValues { get; set; } = [];
    public IReadOnlyList<int> SixthRightValues { get; set; } = [];
    public IReadOnlyList<int> SixthBothValues { get; set; } = [];
    
    public string SeventhName { get; } = "1/7";
    public string SeventhColour { get; } = "";
    public IReadOnlyList<int> SeventhLeftValues { get; set; } = [];
    public IReadOnlyList<int> SeventhRightValues { get; set; } = [];
    public IReadOnlyList<int> SeventhBothValues { get; set; } = [];
    
    public string EighthName { get; } = "1/8";
    public string EighthColour { get; } = "";
    public IReadOnlyList<int> EighthLeftValues { get; set; } = [];
    public IReadOnlyList<int> EighthRightValues { get; set; } = [];
    public IReadOnlyList<int> EighthBothValues { get; set; } = [];
    
    public string NinthName { get; } = "1/9";
    public string NinthColour { get; } = "";
    public IReadOnlyList<int> NinthLeftValues { get; set; } = [];
    public IReadOnlyList<int> NinthRightValues { get; set; } = [];
    public IReadOnlyList<int> NinthBothValues { get; set; } = [];
    
    public string TwelfthName { get; } = "1/12";
    public string TwelfthColour { get; } = "";
    public IReadOnlyList<int> TwelfthLeftValues { get; set; } = [];
    public IReadOnlyList<int> TwelfthRightValues { get; set; } = [];
    public IReadOnlyList<int> TwelfthBothValues { get; set; } = [];
    
    public string SixteenthName { get; } = "1/16";
    public string SixteenthColour { get; } = "";
    public IReadOnlyList<int> SixteenthLeftValues { get; set; } = [];
    public IReadOnlyList<int> SixteenthRightValues { get; set; } = [];
    public IReadOnlyList<int> SixteenthBothValues { get; set; } = [];
    
    private void OnStatsUpdated(Dictionary<(int, BeatSnapDivisor), StatsService.Counts> stats)
    {
        var axisMinOrMax = 10;
        var chartValues = new int[11][][];
        for (var b = 0; b < 11; b++)
        {
            chartValues[b] = new int[3][];
            for (var h = 0; h < 3; h++)
            {
                chartValues[b][h] = new int[_lowerMarker];
            }
        }
        foreach (var stat in stats)
        {
            int lengthIdx;
            if (stat.Key.Item1 == 1) continue;
            if (stat.Key.Item1 == _lowerMarker) lengthIdx = _lowerMarker - 2;
            else if (stat.Key.Item1 == _upperMarker) lengthIdx = _lowerMarker - 1;
            else lengthIdx = stat.Key.Item1 - 2;
            var beatSnapDivisorIdx = BeatSnapDivisorToInt(BeatSnapDivisorLargestFlag(stat.Key.Item2));

            chartValues[beatSnapDivisorIdx][0][lengthIdx] -= stat.Value.LCount;
            chartValues[beatSnapDivisorIdx][1][lengthIdx] += stat.Value.RCount;
            chartValues[beatSnapDivisorIdx][2][lengthIdx] += stat.Value.BCount;

            var lTotal = 0;
            var rTotal = 0;
            var bTotal = 0;
            for (var i = 0; i < 11; i++)
            {
                lTotal += chartValues[i][0][lengthIdx];
                rTotal += chartValues[i][1][lengthIdx];
                bTotal += chartValues[i][2][lengthIdx];
            }
            while (axisMinOrMax < -lTotal
                   || axisMinOrMax < rTotal
                   || axisMinOrMax < bTotal)
            {
                axisMinOrMax += 10;
            }
        }

        var axisLabels = new string[_lowerMarker];
        for (var i = 2; i < _lowerMarker; i++)
        {
            axisLabels[i - 2] = i.ToString();
        }

        axisLabels[_lowerMarker - 2] = _lowerMarker + "-" + (_upperMarker - 1);
        axisLabels[_lowerMarker - 1] = _upperMarker + "+";

        XAxes =
        [
            new Axis
            {
                Name = "Length",
                Labels = axisLabels
            }
        ];
        PositiveAxisMinOrMax = axisMinOrMax;
        NegativeAxisMinOrMax = -axisMinOrMax;
        
        WholeLeftValues = chartValues[0][0];
        WholeRightValues = chartValues[0][1];
        WholeBothValues = chartValues[0][2];
        
        HalfLeftValues = chartValues[1][0];
        HalfRightValues = chartValues[1][1];
        HalfBothValues = chartValues[1][2];
        
        ThirdLeftValues = chartValues[2][0];
        ThirdRightValues = chartValues[2][1];
        ThirdBothValues = chartValues[2][2];
        
        QuarterLeftValues = chartValues[3][0];
        QuarterRightValues = chartValues[3][1];
        QuarterBothValues = chartValues[3][2];
        
        FifthLeftValues = chartValues[4][0];
        FifthRightValues = chartValues[4][1];
        FifthBothValues = chartValues[4][2];
        
        SixthLeftValues = chartValues[5][0];
        SixthRightValues = chartValues[5][1];
        SixthBothValues = chartValues[5][2];
        
        SeventhLeftValues = chartValues[6][0];
        SeventhRightValues = chartValues[6][1];
        SeventhBothValues = chartValues[6][2];
        
        EighthLeftValues = chartValues[7][0];
        EighthRightValues = chartValues[7][1];
        EighthBothValues = chartValues[7][2];
        
        NinthLeftValues = chartValues[8][0];
        NinthRightValues = chartValues[8][1];
        NinthBothValues = chartValues[8][2];
        
        TwelfthLeftValues = chartValues[9][0];
        TwelfthRightValues = chartValues[9][1];
        TwelfthBothValues = chartValues[9][2];
        
        SixteenthLeftValues = chartValues[10][0];
        SixteenthRightValues = chartValues[10][1];
        SixteenthBothValues = chartValues[10][2];
        
        RaisePropertyChanged(nameof(PositiveAxisMinOrMax));
        RaisePropertyChanged(nameof(NegativeAxisMinOrMax));
        
        RaisePropertyChanged(nameof(WholeLeftValues));
        RaisePropertyChanged(nameof(WholeRightValues));
        RaisePropertyChanged(nameof(WholeBothValues));
        
        RaisePropertyChanged(nameof(HalfLeftValues));
        RaisePropertyChanged(nameof(HalfRightValues));
        RaisePropertyChanged(nameof(HalfBothValues));
        
        RaisePropertyChanged(nameof(ThirdLeftValues));
        RaisePropertyChanged(nameof(ThirdRightValues));
        RaisePropertyChanged(nameof(ThirdBothValues));
        
        RaisePropertyChanged(nameof(QuarterLeftValues));
        RaisePropertyChanged(nameof(QuarterRightValues));
        RaisePropertyChanged(nameof(QuarterBothValues));
        
        RaisePropertyChanged(nameof(FifthLeftValues));
        RaisePropertyChanged(nameof(FifthRightValues));
        RaisePropertyChanged(nameof(FifthBothValues));
        
        RaisePropertyChanged(nameof(SixthLeftValues));
        RaisePropertyChanged(nameof(SixthRightValues));
        RaisePropertyChanged(nameof(SixthBothValues));
        
        RaisePropertyChanged(nameof(SeventhLeftValues));
        RaisePropertyChanged(nameof(SeventhRightValues));
        RaisePropertyChanged(nameof(SeventhBothValues));
        
        RaisePropertyChanged(nameof(EighthLeftValues));
        RaisePropertyChanged(nameof(EighthRightValues));
        RaisePropertyChanged(nameof(EighthBothValues));
        
        RaisePropertyChanged(nameof(NinthLeftValues));
        RaisePropertyChanged(nameof(NinthRightValues));
        RaisePropertyChanged(nameof(NinthBothValues));
        
        RaisePropertyChanged(nameof(TwelfthLeftValues));
        RaisePropertyChanged(nameof(TwelfthRightValues));
        RaisePropertyChanged(nameof(TwelfthBothValues));
        
        RaisePropertyChanged(nameof(SixteenthLeftValues));
        RaisePropertyChanged(nameof(SixteenthRightValues));
        RaisePropertyChanged(nameof(SixteenthBothValues));
    }

    private static BeatSnapDivisor BeatSnapDivisorLargestFlag(BeatSnapDivisor beatSnapDivisor)
    {
        return Enum.GetValues<BeatSnapDivisor>()
            .Where(f => beatSnapDivisor.HasFlag(f))
            .Max();
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
    private int _snapDivisor = 1;
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

    private static int BeatSnapDivisorToInt(BeatSnapDivisor beatSnapDivisor)
    {
        return beatSnapDivisor switch
        {
            BeatSnapDivisor.WHOLE => 0,
            BeatSnapDivisor.HALF => 1,
            BeatSnapDivisor.THIRD => 2,
            BeatSnapDivisor.QUARTER => 3,
            BeatSnapDivisor.FIFTH => 4,
            BeatSnapDivisor.SIXTH => 5,
            BeatSnapDivisor.SEVENTH => 6,
            BeatSnapDivisor.EIGHTH => 7,
            BeatSnapDivisor.NINTH => 8,
            BeatSnapDivisor.TWELFTH => 9,
            BeatSnapDivisor.SIXTEENTH => 10,
            _ => -1
        };
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