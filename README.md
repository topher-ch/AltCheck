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

All examples assume RH dominance, fully alternating throughout, and resetting
back to the RH after finishers. Snaps vary depending on beatmap. Lower and Upper
markers are set to 10 and 25, meaning they cluster 10-24 and 25+ as the last 
two columns.

https://osu.ppy.sh/beatmapsets/2283239#taiko/4869109
![4869109](Examples/4869109.png)
Balanced stream map, even distribution between Left and Right hands

https://osu.ppy.sh/beatmapsets/2121587#taiko/4458056
![4458056](Examples/4458056.png)
RH bias stream map, notice especially the bias on 1/4 patterns of length 7

https://osu.ppy.sh/beatmapsets/2054228#taiko/4291522
![4291522](Examples/4291522.png)
RH bias stream map, notice especially the bias on longer streams

https://osu.ppy.sh/beatmapsets/1613246#taiko/3293724
![3293724](Examples/3293724.png)
Slight RH bias

https://osu.ppy.sh/beatmapsets/2042964#taiko/4263291
![4263291](Examples/4263291.png)
RH bias on 1/4 patterns of length 5, however longer streams are largely off
the LH

https://osu.ppy.sh/beatmapsets/2392651#taiko/5186318
![5186318](Examples/5186318.png)
![5186318_2](Examples/5186318_2.png)
At first glance the patterns are evenly distributed, however checking the
hands of nested 1/6 reveals bias towards 1/6 quads starting off the RH.

https://osu.ppy.sh/beatmapsets/2392651#taiko/5186728
![5186728](Examples/5186728.png)
![5186728_2](Examples/5186728_2.png)
Similarly, first glance shows the patterns are evenly distributed. This time,
checking the hands of nested 1/6 reveals no large bias.

https://osu.ppy.sh/beatmapsets/1407228#taiko/2901604
![2901604](Examples/2901604.png)
Lots of long tech streams, with high rhythm complexity. While they do largely
start off the RH, the rhythm complexity makes this bias negligible.

https://osu.ppy.sh/beatmapsets/2350118#taiko/5056926
![5056926](Examples/5056926.png)
All 1/6 patterns come in even lengths, indicating that largely simple
1/4 + 1/6 combinations. Also, scarce compared to 1/4 patterning likely
indicating either a small "tech" section or they are well-dispersed with high
rarity.

https://osu.ppy.sh/beatmapsets/2260133#taiko/4810198
![4810198](Examples/4810198.png)
Slight RH bias, but largely consistent rhythmically complex patterning.

https://osu.ppy.sh/beatmapsets/1355679#taiko/2805796
![2805796](Examples/2805796.png)
Very high rhythmic complexity

https://osu.ppy.sh/beatmapsets/1615826#taiko/3299023
![3299023](Examples/3299023.png)
Very high rhythmic complexity

https://osu.ppy.sh/beatmapsets/2167213#taiko/4572334
![4572334](Examples/4572334.png)
Does not boast much rhythmic complexity at first glance, since the beatmap
instead uses inconsistent longer snaps to provide rhythmic complexity. Large
RH bias in the 10-24 range.

https://osu.ppy.sh/beatmapsets/2167213#taiko/4572333
![4572333](Examples/4572333.png)
Lower diff from the same set as the beatmap above. Funnily enough, large LH
bias in the 10-24 range instead.

https://osu.ppy.sh/beatmapsets/928431#taiko/1939224
![1939224](Examples/1939224.png)
Guess what this is.

https://osu.ppy.sh/beatmapsets/1972518#taiko/4093075
![4093075](Examples/4093075.png)
Swing double beatmap.

https://osu.ppy.sh/beatmapsets/1712810#taiko/3499915
![3499915](Examples/3499915.png)
Large amount of 1/4 patterning, with high occurrences of even note length 
patterning in comparison to earlier stream/farm beatmaps.

https://osu.ppy.sh/beatmapsets/2137833#taiko/4529104
![4529104](Examples/4529104.png)
Double BPM patterning with LH bias.

https://osu.ppy.sh/beatmapsets/2137833#taiko/4499137
![4499137](Examples/4499137.png)
Double BPM patterning with LH bias.
