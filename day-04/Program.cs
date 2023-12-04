using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var input = await httpClient.GetStringAsync("https://adventofcode.com/2023/day/4/input");
var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

var copies = Enumerable.Repeat(1, lines.Length).ToArray();

for (var cardIndex = 0; cardIndex < lines.Length; cardIndex++)
{
  var numbers = lines[cardIndex].Split(':')[1].Split('|');
  
  var winningNumbers = numbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
  var haveNumbers = numbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

  var wins = haveNumbers.Count(n => winningNumbers.Contains(n));

  Console.WriteLine($"Card {cardIndex} has {wins} wins on {copies[cardIndex]} copies");

  for (var winIndex = cardIndex + 1; winIndex <= cardIndex + wins && winIndex < copies.Length; winIndex++)
  {
    copies[winIndex] += copies[cardIndex];
    Console.WriteLine($"Card {winIndex} now has {copies[winIndex]} copies");
  }
}

Console.WriteLine(copies.Sum());