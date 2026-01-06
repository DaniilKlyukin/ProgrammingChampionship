namespace G.GretasGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = int.Parse(Console.ReadLine());
            var results = new long[t];

            for (int test = 0; test < t; test++)
            {
                var n = int.Parse(Console.ReadLine());
                var a = Console.ReadLine().Split().Select(long.Parse).ToArray();

                results[test] = Solve(a);
            }

            foreach (var res in results)
            {
                Console.WriteLine(res);
            }
        }

        public static long Solve(long[] a)
        {
            if (a.Length % 2 == 1)
            {
                return SolveOdd(a);
            }
            else
            {
                return SolveEven(a);
            }
        }

        public static long SolveOdd(long[] a)
        {
            // Сумма всех a_i должна быть четной
            var sumA = a.Sum();

            var totalB = sumA / 2;  // Σb[i] = Σa[i] / 2

            // Вычисляем b[0] = Σb[i] - (a[2] + a[4] + ... + a[n-1])
            var sumAlternateA = 0L;
            for (int i = 2; i < a.Length; i += 2)
                sumAlternateA += a[i];

            var b0 = totalB - sumAlternateA;

            // Восстанавливаем все b[i] по формуле b[i] = a[i] - b[i-1]
            var b = new long[a.Length];
            b[0] = b0;

            for (int i = 1; i < a.Length; i++)
            {
                b[i] = a[i] - b[i - 1];
            }

            var maxB = b.Max();
            var sumB = b.Sum();

            // k ≥ max( max(b[i]), ceil(Σb[i]/(n-1)) )
            return Math.Max(maxB, (sumB + a.Length - 2) / (a.Length - 1));
        }

        public static long SolveEven(long[] a)
        {
            // Сумма всех a_i должна быть четной
            var sumA = a.Sum();

            var sumB = sumA / 2;  // Σb[i] = Σa[i] / 2

            // Частное решение: bHat[0] = 0
            var bHat = new long[a.Length];
            bHat[0] = 0;

            for (int i = 1; i < a.Length; i++)
                bHat[i] = a[i] - bHat[i - 1];

            // Общее решение: b[i] = bHat[i] + (-1)^i * delta
            // Для i=0 (четный индекс): b[0] = bHat[0] + delta = delta
            // Для i=1 (нечетный индекс): b[1] = bHat[1] - delta
            var (minDelta, maxDelta) = GetDeltaBounds(bHat);

            var delta = FindOptimalDelta(bHat, minDelta, maxDelta);

            // Вычисляем максимальное b[i] с оптимальным delta
            var maxB = 0L;
            for (int i = 0; i < a.Length; i++)
            {
                var bi = bHat[i] + ((i % 2 == 0) ? delta : -delta);
                maxB = Math.Max(maxB, bi);
            }

            // k ≥ max( max(b[i]), ceil(Σb[i]/(n-1)) )
            return Math.Max(maxB, (long)Math.Ceiling((double)sumB / (a.Length - 1)));
        }

        // Возвращает минимальное и максимальное допустимые значения delta
        private static (long minDelta, long maxDelta) GetDeltaBounds(long[] bHat)
        {
            // Для i четного (0, 2, 4, ...): b[i] = bHat[i] + delta ≥ 0 => delta ≥ -bHat[i]
            // Для i нечетного (1, 3, 5, ...): b[i] = bHat[i] - delta ≥ 0 => delta ≤ bHat[i]

            var minDelta = long.MinValue;
            var maxDelta = long.MaxValue;

            for (int i = 0; i < bHat.Length; i++)
            {
                if (i % 2 == 0)
                {
                    // delta ≥ -bHat[i]
                    minDelta = Math.Max(minDelta, -bHat[i]);
                }
                else
                {
                    // delta ≤ bHat[i]
                    maxDelta = Math.Min(maxDelta, bHat[i]);
                }
            }

            return (minDelta, maxDelta);
        }

        // Находит оптимальное delta в пределах [minDelta, maxDelta]
        // Оптимизирует max(b[i]) = max(maxEvenB + delta, maxOddB - delta)
        private static long FindOptimalDelta(long[] bHat, long minDelta, long maxDelta)
        {
            // Находим максимальные значения bHat для четных и нечетных индексов
            var maxEvenB = long.MinValue;  // максимальное bHat[i] для четных i
            var maxOddB = long.MinValue;   // максимальное bHat[i] для нечетных i

            for (int i = 0; i < bHat.Length; i++)
            {
                if (i % 2 == 0)
                {
                    maxEvenB = Math.Max(maxEvenB, bHat[i]);
                }
                else
                {
                    maxOddB = Math.Max(maxOddB, bHat[i]);
                }
            }

            // Мы хотим минимизировать max(maxEvenB + delta, maxOddB - delta)
            // Оптимум, когда они равны: maxEvenB + delta = maxOddB - delta
            // => 2*delta = maxOddB - maxEvenB
            // => delta = (maxOddB - maxEvenB) / 2
            var targetDelta = (maxOddB - maxEvenB) / 2;

            // Ограничиваем targetDelta диапазоном [minDelta, maxDelta]
            if (targetDelta < minDelta) targetDelta = minDelta;
            if (targetDelta > maxDelta) targetDelta = maxDelta;

            // Проверяем также targetDelta+1, так как нужен целочисленный оптимум
            var candidate2 = targetDelta + 1;
            if (candidate2 <= maxDelta)
            {
                var value1 = Math.Max(maxEvenB + targetDelta, maxOddB - targetDelta);
                var value2 = Math.Max(maxEvenB + candidate2, maxOddB - candidate2);

                if (value2 < value1)
                    return candidate2;
            }

            return targetDelta;
        }
    }
}