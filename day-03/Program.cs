using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Cookie", Environment.GetEnvironmentVariable("AOC_SESSION"));
var input = await httpClient.GetStringAsync("https://adventofcode.com/2023/day/3/input");
var lines = input.Split('\n');

Enumerable.Range(0, 15).ToList().ForEach(x => Console.WriteLine(lines[x]));

var p1_total = 0;
var p2_total = 0;

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
    p1_total += number.Value;
  }
}

foreach (var star in FindStars(lines)) {
  var adjacentNumbers = AdjacentNumbers(lines, star).ToArray();

  if (adjacentNumbers.Length == 2) {
    p2_total += adjacentNumbers[0] * adjacentNumbers[1];
  }
}

Console.WriteLine(p1_total);
Console.WriteLine(p2_total);

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

IEnumerable<(int LineNo, int ColNo)> FindStars(string[] lines) {
  for (var lineNo = 0; lineNo < lines.Length; lineNo++) {
    var line = lines[lineNo];

    for (var charNo = 0; charNo < line.Length; charNo++) {
      if (line[charNo] == '*') yield return (lineNo, charNo);
    }
  }
}

IEnumerable<int> AdjacentNumbers(string[] lines, (int LineNo, int ColNo) startPoint) {

  var inspectLine = lines[startPoint.LineNo];

  // Search left on the same line
  if (TrySearchLeft(inspectLine, startPoint.ColNo, out var left)) yield return left;
  
  // Search right on the same line
  if (TrySearchRight(inspectLine, startPoint.ColNo, out var right)) yield return right;

  // Search above
  if (startPoint.LineNo > 0) {
    inspectLine = lines[startPoint.LineNo - 1];

    // If there is no digit directly above, search left and right on the same line
    if (!char.IsDigit(inspectLine[startPoint.ColNo])) {
      if (TrySearchLeft(inspectLine, startPoint.ColNo, out var topleft)) yield return topleft;
      if (TrySearchRight(inspectLine, startPoint.ColNo, out var topright)) yield return topright;
    } else {
      yield return ScanNumber(inspectLine, startPoint.ColNo);
    }
  }

  // Search below
  if (startPoint.LineNo < lines.Length - 1) {
    inspectLine = lines[startPoint.LineNo + 1];

    // If there is no digit directly above, search left and right on the same line
    if (!char.IsDigit(inspectLine[startPoint.ColNo])) {
      if (TrySearchLeft(inspectLine, startPoint.ColNo, out var botleft)) yield return botleft;
      if (TrySearchRight(inspectLine, startPoint.ColNo, out var botright)) yield return botright;
    } else {
      yield return ScanNumber(inspectLine, startPoint.ColNo);
    }
  }

}

bool TrySearchLeft(string line, int startCol, out int value) {
  var left = line[Math.Max(0, startCol - 1)];
  if (char.IsDigit(left)) {
    var endIndex = startCol - 1;
    var startIndex = endIndex;

    // Keep searching left until you hit a non-digit or start of line
    while (startIndex >= 0 && char.IsDigit(line[startIndex])) {
      startIndex--;
    }
    startIndex++;

    value = int.Parse(line.Substring(startIndex, endIndex - startIndex + 1));
    return true;
  }

  value = 0;
  return false;
}

bool TrySearchRight(string line, int startCol, out int value) {
  var right = line[Math.Min(startCol + 1, line.Length - 1)];
  if (char.IsDigit(right)) {
    var startIndex = startCol + 1;
    var endIndex = startIndex;

    // Keep searching right until you stop seeing digits or reach end of line
    while (endIndex <= line.Length - 1 && char.IsDigit(line[endIndex])) {
      endIndex++;
    }
    endIndex--;

    value = int.Parse(line.Substring(startIndex, endIndex - startIndex + 1));
    return true;
  }

  value = 0;
  return false;
}

int ScanNumber(string line, int startCol) {
  var startIndex = startCol;
  var endIndex = startCol;

  while (startIndex >= 0 && char.IsDigit(line[startIndex])) {
    startIndex--;
  }
  startIndex++;

  while (endIndex <= line.Length - 1 && char.IsDigit(line[endIndex])) {
    endIndex++;
  }
  endIndex--;

  return int.Parse(line.Substring(startIndex, endIndex - startIndex + 1));
}