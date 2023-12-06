using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var input = await httpClient.GetStringAsync("https://adventofcode.com/2023/day/6/input");
var lines = input.Split('\n');

var times = lines.First().Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
var dists = lines.Skip(1).First().Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

var races = times.Zip(dists, (t, d) => new { Time = t, Dist = d }).ToArray();

/*
// Demo data
races = [
    new { Time = 7, Dist = 9 },
    new { Time = 15, Dist = 40 },
    new { Time = 30, Dist = 200 }
];
*/

long p1_total = 1;

foreach (var race in races) {
    var x = Solve(race.Time, race.Dist);
    p1_total *= x;
}

Console.WriteLine(p1_total);

var p2_time = long.Parse(lines.First().Split(':')[1].Replace(" ", string.Empty));
var p2_dist = long.Parse(lines.Skip(1).First().Split(':')[1].Replace(" ", string.Empty));

var p2_total = Solve(p2_time, p2_dist);

Console.WriteLine(p2_total);

long Solve(long time, long dist) {
    // Charge Time (C) + Go Time (G) = Race Time (T)
    // Charge Time * Go Time = Distance (D)
    // C * (T - C) = D
    // -C^2 + TC = D
    // C^2 - TC + D = 0
    // Quadratic equation to find the 2 solutions

    // Our "c" term is Distance + 1, because we want to exceed the given distance
    var q = Math.Sqrt(Math.Pow(time, 2) - 4 * (dist + 1));

    // The upper point, round down
    var c1 = Math.Floor((time + q) / 2);

    // The lower point, round up
    var c2 = Math.Ceiling((time - q) / 2);

    var x = (int)(c1 - c2 + 1);

    return x;
}