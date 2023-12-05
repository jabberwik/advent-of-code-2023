using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var inputStream = await httpClient.GetStringAsync("https://adventofcode.com/2023/day/5/input");
var lines = inputStream.Split('\n');

var iter = lines.AsEnumerable().GetEnumerator();

iter.MoveNext();
var initialSeeds = iter.Current.Split(':')[1]
    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Select(long.Parse)
    .ToArray();

// blank
iter.MoveNext();

var seedToSoilMap = ReadMap(iter).ToArray();
var soilToFertMap = ReadMap(iter).ToArray();
var fertToWaterMap = ReadMap(iter).ToArray();
var waterToLightMap = ReadMap(iter).ToArray();
var lightToTempMap = ReadMap(iter).ToArray();
var tempToHumidMap = ReadMap(iter).ToArray();
var humidToLocMap = ReadMap(iter).ToArray();

var minLoc = initialSeeds
    .Select(Translate)
    .Min();

Console.WriteLine(minLoc);

IEnumerable<(long a, long b, long range)> ReadMap(IEnumerator<string> lines)
{
    // header
    lines.MoveNext();

    lines.MoveNext();
    do {
        var line = lines.Current;
        var data = line.Split(' ').Select(long.Parse).ToArray();
        yield return (data[0], data[1], data[2]);

        lines.MoveNext();
    } while (!string.IsNullOrWhiteSpace(lines.Current));
}

long Translate(long seed) {
    var soil = Navigate(seed, seedToSoilMap);
    var fert = Navigate(soil, soilToFertMap);
    var water = Navigate(fert, fertToWaterMap);
    var light = Navigate(water, waterToLightMap);
    var temp = Navigate(light, lightToTempMap);
    var humid = Navigate(temp, tempToHumidMap);
    var loc = Navigate(humid, humidToLocMap);
    return loc;
}

long Navigate(long input, (long dest, long src, long range)[] map) {
   var mapLine = map.SingleOrDefault(m => m.src <= input && m.src + m.range > input);
   if (mapLine == default) return input;
   return mapLine.dest + (input - mapLine.src);
}