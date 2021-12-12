<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));

var depths = File.ReadAllLines("day1.txt").Select(int.Parse).ToArray();
var simplePositiveDiffs = 0;
var threePositiveDiffs = 0;

for (int i = 1; i < depths.Count(); i += 1)
{
	var simpleDiff = depths[i] - depths[i - 1];
	simplePositiveDiffs += simpleDiff > 0 ? 1 : 0;
	
	if (i >= 3) 
	{
		// Simplification works here due to telescoping sum.
		var threeDiff = depths[i] - depths[i - 3];
		threePositiveDiffs += threeDiff > 0 ? 1 : 0;
	}
}

Console.WriteLine(simplePositiveDiffs);
Console.WriteLine(threePositiveDiffs);
