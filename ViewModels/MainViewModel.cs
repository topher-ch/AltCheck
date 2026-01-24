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
}