<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var startIndividualFishTimers = File.ReadAllLines("day6.txt")[0].Split(',').Select(long.Parse).ToArray();

var startFishCountTimers = new long[9];
var veteranFishCountTimers = new long[7];

// Initialize the first time array with our input.
foreach (var timer in startIndividualFishTimers) 
{
    veteranFishCountTimers[timer] += 1;
}

long GetFishCounts(long[] startFishCountTimers, long[] veteranFishCountTimers, int days)
{
    for (int day = 0; day < days; day += 1)
    {
        var startNewBornFish = startFishCountTimers[0];
        var veteranNewBornFish = veteranFishCountTimers[0];
        
        for (var i = 0; i < startFishCountTimers.Length - 1; i += 1)
        {
            startFishCountTimers[i] = startFishCountTimers[i + 1];
        }
                
        for (var i = 0; i < veteranFishCountTimers.Length - 1; i += 1)
        {
            veteranFishCountTimers[i] = veteranFishCountTimers[i + 1];
        }
        
        startFishCountTimers[^1] = startNewBornFish + veteranNewBornFish;
        veteranFishCountTimers[^1] = startNewBornFish + veteranNewBornFish;
    }

    return startFishCountTimers.Sum() + veteranFishCountTimers.Sum();
}

// Part 1
Console.WriteLine(GetFishCounts((long[])startFishCountTimers.Clone(), (long[])veteranFishCountTimers.Clone(), 80));
// Part 2
Console.WriteLine(GetFishCounts((long[])startFishCountTimers.Clone(), (long[])veteranFishCountTimers.Clone(), 256));
