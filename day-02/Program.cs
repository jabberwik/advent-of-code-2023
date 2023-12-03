using System.Text.RegularExpressions;

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var inputStream = await httpClient.GetStreamAsync("https://adventofcode.com/2023/day/2/input");

var lineRe = new Regex(@"Game (\d+): (.*)");
var gameRe = new Regex(@"(\d+) (red|green|blue),?");

using var reader = new StreamReader(inputStream);

int total = 0;

while (!reader.EndOfStream)
{
  var line = await reader.ReadLineAsync();

  var match = lineRe.Match(line);

  var gameId = int.Parse(match.Groups[1].Value);

  var games = match.Groups[2].Value.Split("; ");

  var gameCubes = games.Select(game =>
    gameRe.Matches(game)
      .Select(gameMatch => new {
        CubeCount = int.Parse(gameMatch.Groups[1].Value),
        CubeColor = gameMatch.Groups[2].Value,
      })
      .ToDictionary(x => x.CubeColor, x => x.CubeCount)
    );

  var minimums = new {
    Red = gameCubes.Select(g => g.GetValueOrDefault("red")).Max(),
    Green = gameCubes.Select(g => g.GetValueOrDefault("green")).Max(),
    Blue = gameCubes.Select(g => g.GetValueOrDefault("blue")).Max(),
  };

  var power = minimums.Red * minimums.Green * minimums.Blue;
  total += power;
}

Console.WriteLine(total);