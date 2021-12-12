<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadAllLines("day9.txt");
var heightMap = new int[lines.Length, lines[0].Length];
for (var i = 0; i < lines.Length; i += 1) 
{
    for (var j = 0; j < lines[0].Length; j += 1) 
    {
        heightMap[i, j] = int.Parse(lines[i][j].ToString());    
    }
}

IEnumerable<(int, int)> GetNeighbours(int[,] heightMap, int x, int y) 
{
    return new[] {(x + 1, y), (x, y - 1), (x - 1, y), (x, y + 1)}
                 .Where(p => 0 <= p.Item1 && p.Item1 < heightMap.GetLength(0) &&
                             0 <= p.Item2 && p.Item2 < heightMap.GetLength(1));
}

// Part 1
var lowPointsCoordinates = new List<(int, int)>();
for (var x = 0; x < heightMap.GetLength(0); x += 1)
{
    for (var y = 0; y < heightMap.GetLength(1); y += 1) 
    {
        var currHeight = heightMap[x, y];
        var neighboursMinHeight = GetNeighbours(heightMap, x, y)
                                  .Min(p => heightMap[p.Item1, p.Item2]);
        
        if (currHeight < neighboursMinHeight) 
        {
            lowPointsCoordinates.Add((x, y));
        }
    }    
}
Console.WriteLine(lowPointsCoordinates.Sum(p => 1 + heightMap[p.Item1, p.Item2]));

// Part 2
// Using the low points we found, determine the actual basin size for each using BFS traversal.
// Note that we don't include any 9s in any of them which gives us another stopping point.
var basinPointHeights = new List<(int, int, int)>[lowPointsCoordinates.Count];
var seenPoints = new HashSet<(int, int)>();
var basinPoints = new Queue<(int, int)>();
for (var i = 0; i < lowPointsCoordinates.Count; i += 1)
{
    var startPoint = (lowPointsCoordinates[i].Item1, lowPointsCoordinates[i].Item2);
    basinPointHeights[i] = new List<(int, int, int)>();
    
    basinPoints.Enqueue(startPoint);
    seenPoints.Add(startPoint);
    while (basinPoints.Count > 0) 
    {
        var center = basinPoints.Dequeue();
        basinPointHeights[i].Add((center.Item1, center.Item2, heightMap[center.Item1, center.Item2]));
        
        var neighbours = GetNeighbours(heightMap, center.Item1, center.Item2)
                         .Where(p => !seenPoints.Contains(p) && 
                                     heightMap[p.Item1, p.Item2] != 9 &&
                                     heightMap[center.Item1, center.Item2] < heightMap[p.Item1, p.Item2]);
        foreach (var n in neighbours) 
        {
            basinPoints.Enqueue(n);
            seenPoints.Add(n);
        }
    }
}
Console.WriteLine(basinPointHeights.Select(list => list.Count)
                                   .OrderByDescending(c => c)
                                   .Take(3)
                                   .Aggregate((c1, c2) => c1 * c2));
