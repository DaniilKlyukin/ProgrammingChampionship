namespace D.Doorway;

internal class Program
{
    static void Main(string[] args)
    {
        var n = int.Parse(Console.ReadLine());
        var layers = new Layer[n];

        for (int i = 0; i < n; i++)
        {
            var input = Console.ReadLine().Split().Select(long.Parse).ToArray();
            var k = (int)input[0];
            var x1 = input[1];
            var x2 = input[2];
            var doors = input.Skip(3).Take(k).ToArray();
            layers[i] = new Layer(x1, x2, doors);
        }

        Console.WriteLine(Solve(layers));
    }

    static long Solve(Layer[] layers)
    {
        var wallL = 0L;
        var wallR = long.MaxValue;

        foreach (var layer in layers)
        {
            wallL = Math.Max(wallL, layer.L);
            wallR = Math.Min(wallR, layer.R);
        }

        if (wallL >= wallR)
            return 0;

        var low = 0L;
        var high = wallR - wallL;
        var answer = 0L;

        while (low <= high)
        {
            var mid = (low + high) / 2;

            if (CanMakeHole(layers, wallL, wallR, mid))
            {
                answer = mid;
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return answer;
    }

    // Проверяем, можно ли сделать отверстие длины holeLen в интервале [targetL, targetR]
    static bool CanMakeHole(Layer[] layers, long wallL, long wallR, long holeLen)
    {
        if (holeLen == 0)
            return true;

        if (holeLen > wallR - wallL)
            return false;

        // Для каждого слоя находим интервалы, где можно разместить отверстие
        // и ищем их пересечение
        var currentIntervals = new List<(long Start, long End)> { (wallL, wallR - holeLen) };

        foreach (var layer in layers)
        {
            var layerIntervals = GetPossibleHolePositions(layer, wallL, wallR, holeLen);

            if (layerIntervals.Count == 0)
                return false;

            // Пересекаем currentIntervals с layerIntervals
            var newIntervals = new List<(long Start, long End)>();
            var i = 0;
            var j = 0;
            while (i < currentIntervals.Count && j < layerIntervals.Count)
            {
                var start = Math.Max(currentIntervals[i].Start, layerIntervals[j].Start);
                var end = Math.Min(currentIntervals[i].End, layerIntervals[j].End);

                if (start <= end)
                {
                    newIntervals.Add((start, end));
                }

                if (currentIntervals[i].End < layerIntervals[j].End)
                    i++;
                else
                    j++;
            }

            if (newIntervals.Count == 0)
                return false;

            currentIntervals = newIntervals;
        }

        return currentIntervals.Count > 0;
    }

    // Для данного слоя находим все позиции x, где можно начать отверстие длины holeLen
    // (так чтобы интервал [x, x+holeLen] был свободен при некотором расположении дверей)
    static List<(long Start, long End)> GetPossibleHolePositions(Layer layer, long wallL, long wallR, long holeLen)
    {

        if (layer.Doors.Length == 0)
        {
            var start = Math.Max(layer.L, wallL);
            var end = Math.Min(layer.R - holeLen, wallR - holeLen);

            if (start <= end)
               return [(start, end)];

            return [];
        }

        if (layer.TotalGap < holeLen)
            return [];

        var intervals = new List<(long Start, long End)>();

        for (int i = 0; i < layer.MinGapStart.Length; i++)
        {
            var realMinStart = Math.Max(layer.MinGapStart[i], wallL);
            var realMaxStart = Math.Min(layer.MaxGapEnd[i] - holeLen, wallR - holeLen);

            if (realMinStart > realMaxStart)
                continue;

            if (intervals.Count > 0 && intervals[^1].End >= realMinStart - 1)
            {
                var (start, end) = intervals[^1];
                intervals[^1] = (start, Math.Max(end, realMaxStart));
            }
            else
            {
                intervals.Add((realMinStart, realMaxStart));
            }
        }

        return intervals;
    }
}

public class Layer
{
    public long L { get; }
    public long R { get; }
    public long[] Doors { get; }
    public long[] PrefixLengths { get; }
    public long[] MinGapStart { get; }
    public long[] MaxGapEnd { get; }
    public long TotalDoors => PrefixLengths[^1];
    public long TotalGap => R - L - TotalDoors;

    public Layer(long l, long r, long[] doors)
    {
        L = l;
        R = r;
        Doors = doors;

        PrefixLengths = new long[Doors.Length + 1];

        for (int i = 0; i < Doors.Length; i++)
            PrefixLengths[i + 1] = PrefixLengths[i] + Doors[i];

        MinGapStart = new long[PrefixLengths.Length];
        MaxGapEnd = new long[PrefixLengths.Length];

        for (int i = 0; i < PrefixLengths.Length; i++)
        {
            MinGapStart[i] = L + PrefixLengths[i];
            MaxGapEnd[i] = R - (TotalDoors - PrefixLengths[i]);
        }
    }
}