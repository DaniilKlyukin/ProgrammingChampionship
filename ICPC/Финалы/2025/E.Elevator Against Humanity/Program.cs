namespace E.Elevator_Against_Humanity
{
    public class Program
    {
        static void Main(string[] args)
        {
            var t = long.Parse(Console.ReadLine());

            var floorsCases = new long[t][];

            for (long i = 0; i < t; i++)
            {
                var n = long.Parse(Console.ReadLine());

                var floors = new long[2 * n];

                for (long j = 0; j < n; j++)
                {
                    var fromTo = Console.ReadLine().Split(' ');

                    floors[2 * j] = long.Parse(fromTo[0]);
                    floors[2 * j + 1] = long.Parse(fromTo[1]);
                }

                floorsCases[i] = floors;
            }

            foreach (var floors in floorsCases)
            {
                Console.WriteLine(FindMaxTime(floors));
            }
        }

        public static long FindMaxTime(long[] floors)
        {
            var canVisitIds = new HashSet<long>();

            for (long j = 0; j < floors.Length; j += 2)
            {
                canVisitIds.Add(j);
            }

            var currentFloorId = -1L;
            var currentFloor = 1L;
            var timeSum = 0L;

            while (canVisitIds.Count != 0)
            {
                var maxDistance = 0L;
                var maxDistanceId = 0L;

                foreach (var id in canVisitIds)
                {
                    var distance = Math.Abs(floors[id] - currentFloor);

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        maxDistanceId = id;
                    }
                }

                if (maxDistanceId % 2 == 0)
                {
                    canVisitIds.Add(maxDistanceId + 1);
                }

                currentFloorId = maxDistanceId;
                currentFloor = floors[maxDistanceId];

                canVisitIds.Remove(currentFloorId);

                timeSum += maxDistance;
            }

            return timeSum;
        }
    }
}