namespace F.Fragmented_Nim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var t = int.Parse(Console.ReadLine());

            var games = new int[t][];

            for (int i = 0; i < t; i++)
            {
                Console.ReadLine();

                games[i] = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            }

            foreach (var game in games)
            {
                Console.WriteLine(GetWinner(game));
            }
        }

        public static string GetWinner(int[] game)
        {
            var notOneCount = game.Count(x => x != 1);
            var isCountOdd = game.Length % 2 == 1;

            if (notOneCount == 0)
            {
                return isCountOdd ? "Alice" : "Bob";
            }

            var isNotOneCountEven = notOneCount % 2 == 0;

            return (isCountOdd == isNotOneCountEven) ? "Bob" : "Alice";
        }
    }
}