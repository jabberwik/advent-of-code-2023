using System.Text.RegularExpressions;

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var inputStream = await httpClient.GetStreamAsync("https://adventofcode.com/2023/day/1/input");

var digits = new[] {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
var numberMatch = new Regex(@$"(?=(\d|{string.Join('|', digits)}))");

using var reader = new StreamReader(inputStream);

int total = 0;

while (!reader.EndOfStream)
{
  var line = await reader.ReadLineAsync();

  var matches = numberMatch.Matches(line!);

  var firstStr = matches.First().Groups[1].Value;
  var lastStr = matches.Last().Groups[1].Value;

  var first = int.TryParse(firstStr, out var n1) ? n1 : Array.IndexOf(digits, firstStr) + 1;
  var last = int.TryParse(lastStr, out var n2) ? n2 : Array.IndexOf(digits, lastStr) + 1;
  
  total += (first * 10) + last;
}

Console.WriteLine(total);