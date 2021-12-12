<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadAllLines("day5.txt").Select(l =>
{
    var endPoints = l.Split(" -> ");
    var p1 = endPoints[0].Split(',');
    var p2 = endPoints[1].Split(',');

    return new Line
    {
        x1 = int.Parse(p1[0]),
        y1 = int.Parse(p1[1]),
        x2 = int.Parse(p2[0]),
        y2 = int.Parse(p2[1])
    };
}).ToArray();

var linesMap = new int[1000, 1000];
var flatLinesMap = new int[1000, 1000];
foreach (var l in lines) 
{
    // Horizontal line
    if (l.y1 == l.y2) 
    {
        var start = Math.Min(l.x1, l.x2);
        var end = Math.Max(l.x1, l.x2);
        
        for (var i = start; i <= end; i += 1) 
        {
            flatLinesMap[i, l.y1] += 1;
            linesMap[i, l.y1] += 1;
        }
    }
    // Vertical line
    else if (l.x1 == l.x2)
    {
        var start = Math.Min(l.y1, l.y2);
        var end = Math.Max(l.y1, l.y2);
        
        for (var j = start; j <= end; j += 1)
        {
            flatLinesMap[l.x1, j] += 1;
            linesMap[l.x1, j] += 1;
        }
    }
    // Diagonal line
    else 
    {
        var dir = ((l.x2 - l.x1) / Math.Abs(l.x2 - l.x1), (l.y2 - l.y1) / Math.Abs(l.y2 - l.y1));
        var step = (l.x1, l.y1);
        var maxStepCount = Math.Abs(l.x2 - l.x1) + 1;
        
        for (var stepCount = 0; stepCount < maxStepCount; stepCount += 1) 
        {
            linesMap[step.Item1, step.Item2] += 1;
            step = (step.Item1 + dir.Item1, step.Item2 + dir.Item2);
        }
    }
}

var flatLineIntersectionCount = 0;
var intersectionCount = 0;
for (var i = 0; i < 1000; i += 1) 
{
    for (var j = 0; j < 1000; j += 1) 
    {
        flatLineIntersectionCount += flatLinesMap[i, j] > 1 ? 1 : 0;
        intersectionCount += linesMap[i, j] > 1 ? 1 : 0;    
    }    
}
//PrintMap(flatLinesMap, 10, 10);
Console.WriteLine(flatLineIntersectionCount);
//PrintMap(linesMap, 10, 10);
Console.WriteLine(intersectionCount);

/// <summary>
/// Printing a section of a map for debugging purposes.
/// </summary>
void PrintMap(int[,] map, int width, int height) 
{
    for (var j = 0; j < height; j += 1) 
    {
        for (var i = 0; i < width; i += 1) 
        {
            Console.Write($"{(map[i, j] > 0 ? map[i, j] : ".")}");
        }
        Console.WriteLine();
    }    
}

struct Line 
{
    public int x1 { get; set; }
    public int y1 { get; set; }
    
    public int x2 { get; set; }
    public int y2 { get; set; }
}