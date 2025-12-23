namespace A.Alphabet_City;

public class Program
{
    const int CHARS_COUNT = 26;

    /// <summary>
    /// Реализовано 2 способа:
    /// 1) через словари (общий, но медленный ~300 мс на решение)
    /// 2) через массивы (только для 26 букв, но быстрый ~125 мс на решение)
    /// </summary>
    public static void Main()
    {
        var line1 = Console.ReadLine();
        var line1Arr = line1.Split(' ');

        var n = int.Parse(line1Arr[0]);
        var m = int.Parse(line1Arr[1]);

        var letters = new string[n];

        for (int i = 0; i < letters.Length; i++)
        {
            letters[i] = Console.ReadLine();
        }

        //var maps = BuildLetterMaps(letters);
        //var totalMap = BuildTotalMap(maps);

        var (maps, totalMap) = BuildMaps(letters);

        var results = new int[letters.Length];

        for (int i = 0; i < letters.Length; i++)
        {
            results[i] = ProcessFast(totalMap, maps, i, m);
        }

        Console.WriteLine(string.Join(" ", results));
    }

    public static int Process(Dictionary<char, int> totalMap, Dictionary<char, int>[] maps, int missingIndex, int copiesCount)
    {
        var minCount = int.MaxValue;

        foreach (var (c, missCount) in maps[missingIndex])
        {
            var noMissCount = totalMap[c] - missCount;

            if (noMissCount <= 0)
                return -1;

            var current = copiesCount - (double)missCount / noMissCount;

            if (current < 0)
                return -1;

            var currentInt = (int)current;

            if (current < minCount)
                minCount = currentInt;
        }

        return minCount;
    }

    public static Dictionary<char, int>[] BuildLetterMaps(string[] letters)
    {
        var maps = new Dictionary<char, int>[letters.Length];

        for (int i = 0; i < letters.Length; i++)
        {
            maps[i] = new Dictionary<char, int>();

            foreach (var c in letters[i])
            {
                if (maps[i].ContainsKey(c))
                    maps[i][c]++;
                else
                    maps[i].Add(c, 1);
            }
        }

        return maps;
    }

    public static Dictionary<char, int> BuildTotalMap(Dictionary<char, int>[] maps)
    {
        var map = new Dictionary<char, int>();

        for (int i = 0; i < maps.Length; i++)
        {
            foreach (var (c, count) in maps[i])
            {
                if (map.ContainsKey(c))
                    map[c] += count;
                else
                    map.Add(c, count);
            }
        }

        return map;
    }

    #region Optimized solution
    public static int ProcessFast(int[] totalMap, int[,] maps, int missingIndex, int copiesCount)
    {
        var minK = int.MaxValue;

        for (int i = 0; i < CHARS_COUNT; i++)
        {
            if (maps[missingIndex, i] == 0)
                continue;

            var noMissCount = totalMap[i] - maps[missingIndex, i];

            if (noMissCount <= 0)
                return -1;

            var currentK = copiesCount - (double)maps[missingIndex, i] / noMissCount;

            if (currentK < 0)
                return -1;

            var currentInt = (int)currentK;

            if (currentK < minK)
                minK = currentInt;
        }

        return minK;
    }

    public static (int[,] letterMaps, int[] totalMap) BuildMaps(string[] letters)
    {
        var letterMaps = new int[letters.Length, CHARS_COUNT];
        var totalMap = new int[CHARS_COUNT];

        for (int i = 0; i < letters.Length; i++)
        {
            for (int j = 0; j < letters[i].Length; j++)
            {
                var charIndex = letters[i][j] - 'A';

                letterMaps[i, charIndex]++;
                totalMap[charIndex]++;
            }
        }

        return (letterMaps, totalMap);
    }
    #endregion
}