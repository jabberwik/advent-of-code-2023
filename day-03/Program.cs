using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var input = await httpClient.GetStringAsync("https://adventofcode.com/2023/day/3/input");
var lines = input.Split('\n');

var total = 0;

foreach(var number in FindNumbers(lines))
{
  var isPartNumber = false;

  // Search above
  if (number.LineNo > 0) {
    var inspectLine = lines[number.LineNo - 1];

    for (var searchIndex = Math.Max(0, number.StartIndex - 1);
             searchIndex <= Math.Min(inspectLine.Length - 1, number.StartIndex + number.Length);
             searchIndex++) {
              if (IsNonDotSymbol(inspectLine[searchIndex])) isPartNumber = true;
             }
  }

  // Search left
  if (number.StartIndex > 0) {
    if (IsNonDotSymbol(lines[number.LineNo][number.StartIndex - 1])) isPartNumber = true;
  }

  // Search right
  if (number.StartIndex + number.Length < lines[number.LineNo].Length) {
    if (IsNonDotSymbol(lines[number.LineNo][number.StartIndex + number.Length])) isPartNumber = true;
  }

  // Search below
  if (number.LineNo < lines.Length - 1) {
    var inspectLine = lines[number.LineNo + 1];

    for (var searchIndex = Math.Max(0, number.StartIndex - 1);
             searchIndex <= Math.Min(inspectLine.Length - 1, number.StartIndex + number.Length);
             searchIndex++) {
              if (IsNonDotSymbol(inspectLine[searchIndex])) isPartNumber = true;
             }
  }

  if (isPartNumber){
    total += number.Value;
  }
}

Console.WriteLine(total);

bool IsNonDotSymbol(char input) => input != '.' && !char.IsDigit(input);

IEnumerable<(int LineNo, int StartIndex, int Length, int Value)> FindNumbers(string[] lines) {
  for (var lineNo = 0; lineNo < lines.Length; lineNo++) {
    var line = lines[lineNo];

    for (var charNo = 0; charNo < line.Length; charNo++) {
      var cursor = line[charNo];

      if (char.IsDigit(cursor)) {

        // Start tracking a number
        var startIndex = charNo;
        var length = 1;

        // End tracking either at the end of the line, or at the first non-digit
        while (++charNo < line.Length) {
          cursor = line[charNo];

          if (!char.IsDigit(cursor)) {
            break;
          }

          length++;
        }

        yield return (lineNo, startIndex, length, int.Parse(line.Substring(startIndex, length)));
      }
    }
  }
}
