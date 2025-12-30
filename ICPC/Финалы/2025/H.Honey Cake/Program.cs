namespace H.Honey_Cake
{
    internal class Program
    {
        public static HashSet<long> SmallPrimes = new HashSet<long> { 2, 3, 5 };

        static void Main(string[] args)
        {
            var line1Arr = Console.ReadLine().Split();
            var w = long.Parse(line1Arr[0]);
            var h = long.Parse(line1Arr[1]);
            var d = long.Parse(line1Arr[2]);

            var n = long.Parse(Console.ReadLine());

            var remainingDimensions = new long[3] { w, h, d };

            var peopleFactors = Factorize(n);

            foreach (var factor in peopleFactors)
            {
                if (!TryDivideDimension(remainingDimensions, factor))
                {
                    Console.WriteLine("-1");
                    return;
                }
            }

            Console.WriteLine(
                $"{w / remainingDimensions[0] - 1} " +
                $"{h / remainingDimensions[1] - 1} " +
                $"{d / remainingDimensions[2] - 1}");
        }

        private static bool TryDivideDimension(long[] dimensions, long factor)
        {
            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] % factor == 0)
                {
                    dimensions[i] /= factor;
                    return true;
                }
            }

            return false;
        }

        public static List<long> Factorize(long number)
        {
            if (number <= 1)
                return new List<long>();

            var factors = new List<long>();
            var queue = new Queue<long>();
            queue.Enqueue(number);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var (a, b) = FindFactors(current);

                if (a == 1 || b == 1)
                {
                    var factor = Math.Max(a, b);
                    if (factor != 1)
                        factors.Add(factor);
                }
                else
                {
                    if (!EnqueueFactor(queue, a))
                        factors.Add(a);

                    if (!EnqueueFactor(queue, b))
                        factors.Add(b);
                }
            }

            return factors;
        }

        private static bool EnqueueFactor(Queue<long> queue, long factor)
        {
            if (SmallPrimes.Contains(factor))
                return false;

            queue.Enqueue(factor);
            return true;
        }

        private static (long A, long B) FindFactors(long n)
        {
            foreach (var prime in SmallPrimes)
            {
                if (n % prime == 0)
                    return (prime, n / prime);
            }

            return FermatFactorization(n);
        }

        private static (long A, long B) FermatFactorization(long n)
        {
            var x = (long)Math.Ceiling(Math.Sqrt(n));
            if (x * x == n)
                return (x, n / x);

            while (true)
            {
                var ySquared = x * x - n;
                var y = (long)Math.Sqrt(ySquared);

                if (y * y == ySquared)
                {
                    var a = x + y;
                    var b = x - y;
                    return (a, b);
                }

                x++;

                if (x > (n + 1) / 2)
                    return (n, 1);
            }
        }
    }
}