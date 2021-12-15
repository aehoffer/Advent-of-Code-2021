<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var chunkLines = File.ReadAllLines("day10.txt");

var chunks = new [] { '(', '[', '{', '<', ')', ']', '}', '>' };
var corruptedScoresValues = new [] { 3, 57, 1197, 25137 };
var incompleteScoresValues = new [] { 1, 2, 3, 4 };

var totalCorruptedScore = 0;
var incompleteScores = new List<long>();
var matches = new Stack<char>();

foreach(var line in chunkLines) 
{
    var isIncomplete = true;
    
    matches.Clear();
    for (var i = 0; i < line.Length; i += 1)
    {
        var chunk = line[i];
        var chunkIndex = Array.FindIndex(chunks, c => c == chunk);
        
        // Opening chunk
        if (chunkIndex < 4) 
        {
            matches.Push(chunk);
        }
        // Closing chunk
        else
        {
            var topChunk = matches.Pop();
            var topChunkIndex = Array.FindIndex(chunks, c => c == topChunk);
            
            // Matching brace check.
            if (chunkIndex - topChunkIndex != 4)
            {
                totalCorruptedScore += corruptedScoresValues[chunkIndex - 4];
                isIncomplete = false;
                break;
            }
        }
    }
    
    if (isIncomplete)
    {
        var currentScore = 0L;
        while (matches.TryPop(out var topChunk))
        {
            var chunkIndex = Array.FindIndex(chunks, c => c == topChunk);
            currentScore *= 5; 
            currentScore += incompleteScoresValues[chunkIndex];
        }
        
        incompleteScores.Add(currentScore);
    }
}

// Part 1
Console.WriteLine(totalCorruptedScore);

// Part 2
incompleteScores.Sort();
var middle = incompleteScores.Count / 2;
Console.WriteLine(incompleteScores[middle]);
