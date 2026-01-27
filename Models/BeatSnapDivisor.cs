namespace AltCheck.Models;

[Flags]
public enum BeatSnapDivisor
{
    WHOLE = 1,
    HALF = 2,
    THIRD = 4,
    QUARTER = 8,
    FIFTH = 16,
    SIXTH = 32,
    SEVENTH = 64,
    EIGHTH = 128,
    NINTH = 256,
    TWELFTH = 512,
    SIXTEENTH = 1024
}