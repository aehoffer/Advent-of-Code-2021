<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#LINQPad optimize+

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var binaryNums = File.ReadAllLines("day3.txt").ToList();
var bitPositionsCount = binaryNums[0].Length;
var bitPositionValueCounts = new int[binaryNums[0].Length, 2];

// Part 1
// Calculate the number of bits per position per line
for (var i = 0; i < binaryNums.Count; i += 1) 
{
	var bits = binaryNums[i].Select(c => c.ToString()).ToArray();
	for (var j = 0; j < bits.Length; j += 1)
	{
		var bit = int.Parse(bits[j]);
		bitPositionValueCounts[j, bit] += 1;
	}
}

var gammaRate = 0U;
for (var i = 0; i < bitPositionValueCounts.GetLength(0); i += 1) 
{
	gammaRate <<= 1;
	gammaRate += bitPositionValueCounts[i, 0] > bitPositionValueCounts[i, 1] ? 0U : 1;
}

var epsilonRate = gammaRate ^ ((1 << bitPositionsCount) - 1);

Console.WriteLine(gammaRate * epsilonRate);

// Part 2
string CalculateParameterFromNumbers(HashSet<string> binaryNums, int maxPos, char bitCriteria)
{
	var bitCounts = new[] { 0, 0 };
	for (var pos = 0; pos < maxPos && binaryNums.Count > 1; pos += 1)
	{
		Array.Clear(bitCounts);		
		foreach (var b in binaryNums)
		{
			bitCounts[b[pos] == '0' ? 0 : 1] += 1;
		}

		var mostCommon = bitCounts[0] >= bitCounts[1] ? '0' : '1';
		var leastCommon = bitCounts[0] <= bitCounts[1] ? '0' : '1';
		
		char filterBit;
		if (mostCommon == leastCommon) 
		{
			filterBit = bitCriteria;	
		}
		else
		{
			filterBit = bitCriteria == '1' ? mostCommon : leastCommon;	
		}

		binaryNums.RemoveWhere(b => b[pos] != filterBit);
	}
	
	return binaryNums.First();
}

var o2Rating = Convert.ToInt32(CalculateParameterFromNumbers(new HashSet<string>(binaryNums), bitPositionsCount, '1'), 2);
var co2Rating = Convert.ToInt32(CalculateParameterFromNumbers(new HashSet<string>(binaryNums), bitPositionsCount, '0'), 2);

Console.WriteLine(o2Rating * co2Rating);


