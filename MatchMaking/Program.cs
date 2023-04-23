const int LongCount = 7;
const int ShortCount = 10;
const int Range = 5;
var random = new Random();
var bagLong = Enumerable.Range(0, LongCount).Select(index => random.Next(1, Range + 1)).ToArray();
var bagShort = Enumerable.Range(0, ShortCount).Select(index => random.Next(-Range, 0)).ToArray();
Console.WriteLine($"Long: {string.Join(", ", bagLong)}");
Console.WriteLine($"Short: {string.Join(", ", bagShort)}");
var results = MatchMake(bagLong, bagShort);

Console.WriteLine("——————————————————————————————————————————————————");
var bestResults = results.GroupBy(result => result.Volume).OrderByDescending(group => group.Key).FirstOrDefault();
Console.WriteLine($"Solution count: {results.Count()}, Best Volume: {bestResults?.Key ?? 0}");
foreach (var (result, index) in bestResults?.Select((result, index) => (result, index)) ?? Enumerable.Empty<(Result, int)>())
{
    Console.WriteLine($"Solution {index}: ————————————————————————————————————————————— Volume: {result.Volume}");
    Console.WriteLine($"\tLong: {string.Join(", ", result.Longs)}\n\tShorts: {string.Join(", ", result.Shorts)}\n\tVolume= {result.Volume}");
}

static List<Result> MatchMake(int[] longs, int[] shorts)
{
    var currentLongs = new List<int>(longs.Length);
    var currentShorts = new List<int>(shorts.Length);
    var results = new List<Result>();
    Backtrack(0, longs, shorts, currentLongs, currentShorts, results);
    return results;
}

static void Backtrack(int index, int[] longs, int[] shorts, List<int> currentLongs, List<int> currentShorts, List<Result> results)
{
    int sumLong = currentLongs.Sum(), sumShort = currentShorts.Sum();
    if (sumLong != 0 && sumLong + sumShort == 0)
    {
        results.Add(new Result(currentLongs.ToArray(), currentShorts.ToArray(), sumLong));
    }

    bool longValid = index < longs.Length, shortValid = index < shorts.Length;
    if (!longValid && !shortValid) return;

    Backtrack(index + 1, longs, shorts, currentLongs, currentShorts, results);

    if (longValid)
    {
        currentLongs.Add(longs[index]);
        Backtrack(index + 1, longs, shorts, currentLongs, currentShorts, results);
        currentLongs.RemoveAt(currentLongs.Count() - 1);
    }

    if (shortValid)
    {
        currentShorts.Add(shorts[index]);
        Backtrack(index + 1, longs, shorts, currentLongs, currentShorts, results);
        currentShorts.RemoveAt(currentShorts.Count() - 1);
    }

    if (longValid && shortValid)
    {
        currentLongs.Add(longs[index]);
        currentShorts.Add(shorts[index]);
        Backtrack(index + 1, longs, shorts, currentLongs, currentShorts, results);
        currentLongs.RemoveAt(currentLongs.Count() - 1);
        currentShorts.RemoveAt(currentShorts.Count() - 1);
    }
}

Console.WriteLine("Complete.");
Console.ReadLine();

record Result(int[] Longs, int[] Shorts, int Volume);
