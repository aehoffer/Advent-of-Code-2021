<Query Kind="Statements" />

#LINQPad optimize-

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadAllLines("day11.txt");

var octopuses = new int[lines.Length, lines[0].Length];
for (var i = 0; i < lines.Length; i += 1)
{
    for (var j = 0; j < lines[0].Length; j += 1)
    {
        octopuses[i, j] = int.Parse(lines[i][j].ToString());
    }
}

// Part 1
var totalFlashCounts = 0;
var flashes = 0;
for (var i = 0; i < 100; i += 1) 
{
    flashes = StepAndGetFlashCounts(octopuses);
    totalFlashCounts += flashes;
}
Console.WriteLine(totalFlashCounts);

// Part 2
var step = 100;
for (; flashes < octopuses.Length; step += 1)
{
    flashes = StepAndGetFlashCounts(octopuses);
}
Console.WriteLine(step);

int StepAndGetFlashCounts(int[,] octopuses)
{
    // First, the energy level of each octopus increases by 1.
    var seenOctopusFlashes = new HashSet<(int, int)>();
    for (var i = 0; i < lines.Length; i += 1)
    {
        for (var j = 0; j < lines[0].Length; j += 1)
        {
            octopuses[i, j] = (octopuses[i, j] + 1) % 10;
            if (octopuses[i, j] == 0) 
            {
                seenOctopusFlashes.Add((i, j));
            }
        }
    }

    // Then, any octopus with an energy level greater than 9 flashes.
    // If this causes an octopus to have an energy level greater than 9, it also flashes. 
    // This process continues as long as new octopuses keep having their energy level increased beyond 9.
    // Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
    var currentFlashes = new Queue<(int, int)>(seenOctopusFlashes);
    while (currentFlashes.TryDequeue(out var currFlashCoords))
    {
        var neighbours = GetNeighbours(octopuses, currFlashCoords.Item1, currFlashCoords.Item2)
                         .Where(p => !seenOctopusFlashes.Contains(p));
        
        foreach (var n in neighbours)
        {
            octopuses[n.Item1, n.Item2] = (octopuses[n.Item1, n.Item2] + 1) % 10;
            if (octopuses[n.Item1, n.Item2] == 0)
            {
                seenOctopusFlashes.Add(n);
                currentFlashes.Enqueue(n);
            }
        }
    }
    
    return seenOctopusFlashes.Count;
}

IEnumerable<(int, int)> GetNeighbours(int[,] octopuses, int x, int y)
{
    return new[] { (x + 1, y), (x + 1, y + 1), (x, y + 1), (x - 1, y + 1), (x - 1, y), (x - 1, y - 1), (x, y - 1), (x + 1, y - 1) }
                 .Where(p => 0 <= p.Item1 && p.Item1 < octopuses.GetLength(0) &&
                             0 <= p.Item2 && p.Item2 < octopuses.GetLength(1));
}
