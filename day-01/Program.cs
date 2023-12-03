using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var inputStream = await httpClient.GetStreamAsync("https://adventofcode.com/2023/day/1/input");

using var reader = new StreamReader(inputStream);

int total = 0;

while (!reader.EndOfStream)
{
  var line = await reader.ReadLineAsync();

  var first = line!.First(char.IsDigit);
  var last = line!.Last(char.IsDigit);
  
  total += int.Parse($"{first}{last}");
}

Console.WriteLine(total);