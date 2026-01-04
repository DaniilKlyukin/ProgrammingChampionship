namespace J.Jinx_or_Jackpot
{
    class Program
    {
        public const double INITIAL_MONEY = 1e3;

        static void Main(string[] args)
        {
            var input = Console.ReadLine().Split();
            var n = int.Parse(input[0]);
            var k = int.Parse(input[1]);

            var p = Console.ReadLine().Split().Select(int.Parse).ToArray();

            // Преобразуем проценты в вероятности
            var probabilities = p.Select(pi => pi / 100.0).ToArray();

            var expectedProfit = SolveOptimal(n, k, probabilities);
            Console.WriteLine(expectedProfit);
        }

        static double SolveOptimal(int n, int k, double[] p)
        {
            // Проверка крайних случаев
            var allZero = p.All(prob => prob == 0.0);
            var allOne = p.All(prob => prob == 1.0);

            if (allZero)
            {
                // Если все вероятности 0, всегда проигрываем
                return 0;
            }

            if (allOne)
            {
                // Если все вероятности 1, всегда выигрываем
                // Ставим всё каждый раз, деньги удваиваются k раз
                var finalMoney = INITIAL_MONEY * (1 << k);
                return finalMoney - INITIAL_MONEY;
            }

            // Предвычисляем все p(a,b) с обработкой крайних случаев
            var p_ab = new double[k + 1, k + 1];

            // Для каждого (a,b) вычисляем p(a,b)
            for (int a = 0; a <= k; a++)
            {
                for (int b = 0; a + b <= k; b++)
                {
                    if (a + b == k) continue;

                    var numerator = 0.0;
                    var denominator = 0.0;

                    for (int i = 0; i < n; i++)
                    {
                        var prob = p[i];

                        // Безопасное вычисление prob^a * (1-prob)^b
                        double term;

                        if (prob == 0.0)
                        {
                            term = (a == 0) ? 1.0 : 0.0;
                        }
                        else if (prob == 1.0)
                        {
                            term = (b == 0) ? 1.0 : 0.0;
                        }
                        else
                        {
                            // Используем логарифмы для избежания underflow/overflow
                            var logTerm = a * Math.Log(prob) + b * Math.Log(1.0 - prob);
                            term = Math.Exp(logTerm);
                        }

                        denominator += term;
                        numerator += term * prob;
                    }

                    p_ab[a, b] = (denominator > 1e-12) ? numerator / denominator : 0.0;
                }
            }

            // Остальной код без изменений...
            var C = new double[k + 1, k + 1];

            for (int a = 0; a <= k; a++)
            {
                int b = k - a;
                if (b >= 0 && b <= k)
                {
                    C[a, b] = 1.0;
                }
            }

            for (int total = k - 1; total >= 0; total--)
            {
                for (int a = 0; a <= total; a++)
                {
                    var b = total - a;
                    var p_val = p_ab[a, b];
                    var option1 = p_val * C[a + 1, b] + (1 - p_val) * C[a, b + 1];
                    var option2 = 2.0 * p_val * C[a + 1, b];
                    C[a, b] = Math.Max(option1, option2);
                }
            }

            return INITIAL_MONEY * (C[0, 0] - 1.0);
        }
    }
}
