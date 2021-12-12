<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var crabPositions = File.ReadAllLines("day7.txt")[0].Split(',').Select(int.Parse).ToArray();

var fuelPositionsConst = new int[crabPositions.Max() + 1];
var fuelPositionsIncr = new int[crabPositions.Max() + 1];
for (var fuelPos = 0; fuelPos < fuelPositionsConst.Length; fuelPos += 1) 
{
    var totalFuelConst = 0;
    var totalFuelIncr = 0;
    foreach (var crabPos in crabPositions) 
    {
        var delta = Math.Abs(crabPos - fuelPos);
        
        // Just use it directly here.
        totalFuelConst += delta;
        // Sum of an arithmetic series for this.
        totalFuelIncr += delta * (delta + 1) / 2;
    }
    
    fuelPositionsConst[fuelPos] = totalFuelConst;
    fuelPositionsIncr[fuelPos] = totalFuelIncr;
}

// Part 1
Console.WriteLine(fuelPositionsConst.Min());
// Part 2
Console.WriteLine(fuelPositionsIncr.Min());
