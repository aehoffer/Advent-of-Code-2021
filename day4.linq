<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var bingoInputs = File.ReadAllText("day4.txt").Split($"{Environment.NewLine}{Environment.NewLine}");
var drawnNumbers = bingoInputs[0].Split(',').Select(int.Parse).ToArray();
var bingoCards = bingoInputs.Skip(1).Select(s => new BingoCard(s)).ToArray();

var firstWinningScore = 0;
var foundFirstBingo = false;

foreach(var num in drawnNumbers) 
{
    foreach (var card in bingoCards)
    {
        var location = card.MarkSpace(num, true);
        if (!foundFirstBingo && card.HasBingo(num)) 
        {
            firstWinningScore = card.GetScore(num);
            foundFirstBingo = true;
        }
    }
} 
Console.WriteLine(firstWinningScore);

// Part 2
var lastWinningScore = 0;
var reverseDrawNumbers = drawnNumbers.Reverse().ToArray();

foreach(var num in reverseDrawNumbers) 
{
    foreach (var card in bingoCards)
    {
        var location = card.MarkSpace(num, false);
        if (!card.HasBingo()) 
        {
            // Space needs to be remarked to not have it count for it score.
            card.MarkSpace(location, true);
            lastWinningScore = card.GetScore(num);
            goto FoundLastWinningScore;
        }
    }    
} FoundLastWinningScore:
Console.WriteLine(lastWinningScore);

class BingoCard
{
    private static readonly Regex WhiteSpace = new Regex(@"\s+", RegexOptions.Compiled);
    
    private struct Space
    {
        public int Number { get; set; }
        public bool Marked { get; set; }
    }
    
    private readonly Space[,] Spaces = new Space[5, 5];

    public BingoCard(string board)
    {
        var rows = board.Trim().Split(Environment.NewLine);
        if (rows.Length != 5) throw new ArgumentException("Bingo card must have 5 rows.", nameof(rows));
        
        for (var i = 0; i < 5; i += 1) 
        {
            var matches = WhiteSpace.Split(rows[i].Trim());
            if (matches.Length != 5) throw new ArgumentException($"Bingo row {i} must have 5 columns.", nameof(rows));
            
            for (var j = 0; j < 5; j += 1) 
            {
                Spaces[i, j] = new Space 
                {
                    Number = int.Parse(matches[j]),
                    Marked = false
                };
            }
        }
    }
    
    public int GetScore(int number) 
    {
        var unmarkedSum = 0;
        
        for (var i = 0; i < 5; i += 1) 
        {
            for (var j = 0; j < 5; j += 1) 
            {
                unmarkedSum += Spaces[i, j].Marked ? 0 : Spaces[i, j].Number;
            }
        }
        
        return unmarkedSum * number;
    }
    
    public void MarkSpace((int, int) location, bool mark)
    {
        if (!(0 <= location.Item1 && location.Item1 < 5 &&
              0 <= location.Item2 && location.Item2 < 5)) return;
              
        Spaces[location.Item1, location.Item2].Marked = mark;
    }

    public (int, int) MarkSpace(int num, bool mark)
    {
        var location = FindLocation(num);
        if (location.Item1 >= 0 && location.Item2 >= 0)
        {
            Spaces[location.Item1, location.Item2].Marked = mark;
        }

        return location;
    }

    public bool HasBingo(int num)
    {
        var location = FindLocation(num);

        return HasBingo(location);
    }

    public bool HasBingo()
    {
        // Check all possible columns and rows by just going
        // down the diagonal entries.
        for (int i = 0; i < 5; i += 1)
        {
            if (HasBingo((i, i))) return true;
        }

        return false;
    }

    public bool HasBingo((int, int) location) 
    {
        if (!(0 <= location.Item1 && location.Item1 < 5 && 
              0 <= location.Item2 && location.Item2 < 5)) return false;
        
        // Check the row of the space.
        var rowCheck = true;
        for (var j = 0; j < 5; j += 1) 
        {
            rowCheck &= Spaces[location.Item1, j].Marked;
        }
        if (rowCheck) return true;

        // Check the column of the space.
        var colCheck = true;
        for (var i = 0; i < 5; i += 1)
        {
            colCheck &= Spaces[i, location.Item2].Marked;
        }
        if (colCheck) return true;

        return false;
    }

    private (int, int) FindLocation(int num)
    {
        for (var i = 0; i < 5; i += 1)
        {
            for (var j = 0; j < 5; j += 1)
            {
                if (Spaces[i, j].Number == num) return (i, j);
            }
        }

        return (-1, -1);
    }
}
