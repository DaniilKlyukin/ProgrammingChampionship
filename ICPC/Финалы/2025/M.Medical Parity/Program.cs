namespace M.Medical_Parity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var casesCount = int.Parse(Console.ReadLine());

            var cases = new string[2 * casesCount];

            for (int i = 0; i < 2 * casesCount; i++)
            {
                cases[i] = Console.ReadLine();
            }

            for (int i = 0; i < 2 * casesCount; i += 2)
            {
                var flips = CountFlips(cases[i], cases[i + 1]);
                Console.WriteLine(flips);
            }
        }

        private static int CountFlips(string x, string y)
        {
            // 1 - верно x
            // 2 - верно y

            var ci1 = 0;
            var flips1 = 0;

            var ci2 = 0;
            var flips2 = 0;

            for (int i = 0; i < x.Length; i++)
            {
                var xi = x[i] - '0';
                var yi = y[i] - '0';

                if (xi == 1)
                {
                    ci1++;
                    ci2++;
                }

                var cy1 = ci1 % 2 == 0 ? 0 : 1;
                var cy2 = ci2 % 2 == 0 ? 0 : 1;

                if (cy1 != yi)
                {
                    flips1++;
                }

                if (cy2 != yi)
                {
                    flips2++;
                    ci2--;
                }

                if (flips2 < flips1)
                {
                    flips1 = flips2;
                    ci1 = ci2;
                }
                else if (flips1 < flips2)
                {
                    flips2 = flips1;
                    ci2 = ci1;
                }
            }

            return Math.Min(flips1, flips2);
        }
    }
}
