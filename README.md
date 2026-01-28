## Introduction

A companion app to be run alongside osu! (stable or lazer) to visualize the
starting hands of patterns and variety of beat snap divisors used in osu!taiko 
beatmaps. See below for examples. Requires tosu to be running to access game data.

## Usage

- Ensure tosu is running beforehand

### Settings

- **Don and Kat:** preferred hand for starting a pattern off the respective note
- **Reset on Finisher:** if you will reset back to **Don** or **Kat** after a finisher
- **Reset on Snap:** if you will reset back to **Don** or **Kat** after the **Snap** length
- **Snap:** the snap that denotes the start and end of a pattern
- **Snap by BPM:** instead of specifying a specific snap, instead specify a BPM (defaults to 1/2)
  - Either **Snap** or **Snap by BPM** can be active at one time, but not both
- **Lower:** the lower marker to group patterns by length
- **Higher:** the higher marker to group patterns by length

## Requirements
- .NET 9.0
- tosu

## Download
- Ensure .NET 9.0 and tosu are installed
- https://github.com/topher-ch/AltCheck/releases/latest

## Licenses
- Avalonia
- LiveCharts2
- OsuParsers
- Semi Avalonia


## Examples
