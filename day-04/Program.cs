using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var inputStream = await httpClient.GetStreamAsync("https://adventofcode.com/2023/day/4/input");

using var reader = new StreamReader(inputStream);

int total = 0;

while (!reader.EndOfStream)
{
  var line = await reader.ReadLineAsync();

  var numbers = line.Split(':')[1].Split('|');
  
  var winningNumbers = numbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
  var haveNumbers = numbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

  var wins = haveNumbers.Count(n => winningNumbers.Contains(n));

  if (wins > 0) total += (int)Math.Pow(2, wins - 1);
}

Console.WriteLine(total);