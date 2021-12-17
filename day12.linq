<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadAllLines("day12.txt");

var map = new Dictionary<string, List<string>>();
foreach (var l in lines)
{
    var nodes = l.Split('-');
    var neighbours = new List<string>[2];

    for (var i = 0; i < 2; i += 1)
    {
        if (!map.TryGetValue(nodes[i], out neighbours[i]))
        {
            neighbours[i] = map[nodes[i]] = new List<string>();
        }

        // Sorry Greg. :)
        neighbours[i].Add(nodes[(i + 1) % 2]);
    }
}

// Part 1
Console.WriteLine(ComputePaths(map).Count);

// Part 2
var allPaths = new List<string>();
var smallCaves = map.Keys.Where(cn => !IsStart(cn) && !IsEnd(cn) && IsSmall(cn));

foreach (var sc in smallCaves) 
{
    allPaths.AddRange(ComputePaths(map, sc));
}

// Needed since otherwise we count paths where the special cave 
// is visited 0 or 1 more times in duplicate for each special small cave.
var allDistinctPaths = allPaths.Distinct().ToList();
Console.WriteLine(allDistinctPaths.Count);

List<string> ComputePaths(Dictionary<string, List<string>> map, string specialSmallCave = "") 
{
    var startCaveVisitCounts = new Dictionary<string, int>();
    foreach (var caveName in map.Keys)
    {
        startCaveVisitCounts[caveName] = 0;
    }
    startCaveVisitCounts["start"] = 1;
        
    var paths = new List<string>();
    var currentPathContext = new Stack<(string, List<string>, Dictionary<string, int>)>();
    
    currentPathContext.Push(("start",  new List<string>(), startCaveVisitCounts));
    while (currentPathContext.TryPop(out var pathContext))
    {
        var currCaveName = pathContext.Item1;
        var currPath = pathContext.Item2;
        currPath.Add(currCaveName);
        
        if (IsEnd(currCaveName))
        {
            paths.Add(string.Join(',', currPath));
            continue;
        }

        var currCaveCounts = pathContext.Item3;
        var nextCaveNames = map[currCaveName].Where(cn => (cn == specialSmallCave && currCaveCounts[cn] < 2) || 
                                                          (IsSmall(cn) && currCaveCounts[cn] < 1) || 
                                                          IsBig(cn));
        foreach (var cave in nextCaveNames)
        {
            var nextPaths = currPath.ToList();
            var nextCaveCounts = currCaveCounts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            nextCaveCounts[cave] += 1;
            
            currentPathContext.Push((cave, nextPaths, nextCaveCounts));
        }
    }
    
    return paths;
}

bool IsSmall(string caveName) => caveName.ToLower() == caveName;
bool IsBig(string caveName) => caveName.ToUpper() == caveName;
bool IsStart(string caveName) => caveName == "start";
bool IsEnd(string caveName) => caveName == "end";
