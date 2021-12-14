<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var startIndividualFishTimers = File.ReadAllLines("day6.txt")[0].Split(',').Select(long.Parse).ToArray();

// Initialize the first time array with our input.
var fishCountTimers = new long[9];
foreach (var timer in startIndividualFishTimers) 
{
    fishCountTimers[timer] += 1;
}

long GetFishCounts(long[] fishCountTimers, int days)
{
    for (int day = 0; day < days; day += 1)
    {
        var startNewBornFish = fishCountTimers[0];
        for (var i = 0; i < fishCountTimers.Length - 1; i += 1)
        {
            fishCountTimers[i] = fishCountTimers[i + 1];
        }
        
        // Add existing fish to current ones at 7 days.
        fishCountTimers[6] += startNewBornFish;
        // New born fish get put at 9 days here.
        fishCountTimers[8] = startNewBornFish;
    }

    return fishCountTimers.Sum();
}

// Part 1
Console.WriteLine(GetFishCounts((long[])fishCountTimers.Clone(), 80));
// Part 2
Console.WriteLine(GetFishCounts((long[])fishCountTimers.Clone(), 256));
