<Query Kind="Statements" />

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var directions = File.ReadAllLines("day2.txt").ToArray();

// Part 1
var pos = (0, 0);
foreach (var line in directions) 
{
    var instruction = line.Split(" ");
    var dir = instruction[0];
    var amount = int.Parse(instruction[1]);
    
    switch(dir) 
    {
        case "forward":
            pos.Item1 += amount;
        break;
        
        case "down":
            pos.Item2 += amount;
        break;
            
        case "up":
            pos.Item2 -= amount;
        break;            
    }
}
Console.WriteLine(pos.Item1 * pos.Item2);

// Part 2
var pos2 = (0, 0, 0);
foreach (var line in directions)
{
    var instruction = line.Split(" ");
    var dir = instruction[0];
    var amount = int.Parse(instruction[1]);

    switch (dir)
    {
        case "forward":
            pos2.Item1 += amount;
            pos2.Item2 += pos2.Item3 * amount;
            break;

        case "down":
            pos2.Item3 += amount;
            break;

        case "up":
            pos2.Item3 -= amount;
            break;
    }
}
Console.WriteLine(pos2.Item1 * pos2.Item2);