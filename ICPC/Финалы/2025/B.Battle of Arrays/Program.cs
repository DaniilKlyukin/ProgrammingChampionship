using System.Collections.Generic;

namespace B.Battle_of_Arrays
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var testsCount = int.Parse(Console.ReadLine());

            var tests = new List<(PriorityQueue<int, int> AliceArr, PriorityQueue<int, int> BobArr)>();

            for (int i = 0; i < testsCount; i++)
            {
                Console.ReadLine();

                var aliceStr = Console.ReadLine();
                var bobStr = Console.ReadLine();

                var t1 = Task.Run(() =>
                {
                    var q = new PriorityQueue<int, int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

                    foreach (var item in aliceStr.Split().Select(int.Parse))
                    {
                        q.Enqueue(item, item);
                    }

                    return q;
                });

                var t2 = Task.Run(() =>
                {
                    var q = new PriorityQueue<int, int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

                    foreach (var item in bobStr.Split().Select(int.Parse))
                    {
                        q.Enqueue(item, item);
                    }

                    return q;
                });

                var alice = await t1;
                var bob = await t2;

                tests.Add((alice, bob));
            }

            foreach (var (a, b) in tests)
            {
                while (true)
                {
                    var maxBDestroyed = Move(a, b);

                    if (b.Count == 0)
                    {
                        Console.WriteLine("Alice");
                        break;
                    }

                    var maxADestroyed = Move(b, a);

                    if (a.Count == 0)
                    {
                        Console.WriteLine("Bob");
                        break;
                    }
                }
            }
        }

        public static bool Move(PriorityQueue<int, int> a, PriorityQueue<int, int> b)
        {
            var maxA = a.Peek();
            var maxB = b.Dequeue();

            if (maxA >= maxB)
            {
                return true;
            }
            else
            {
                var d = maxB - maxA;
                b.Enqueue(d, d);
                return false;
            }
        }

        //static async Task Main(string[] args)
        //{
        //    var testsCount = int.Parse(Console.ReadLine());

        //    var tests = new List<(LinkedList<int> AliceArr, LinkedList<int> BobArr)>();

        //    for (int i = 0; i < testsCount; i++)
        //    {
        //        Console.ReadLine();

        //        var aliceStr = Console.ReadLine();
        //        var bobStr = Console.ReadLine();

        //        var t1 = Task.Run(() =>
        //        {
        //            return new LinkedList<int>(
        //             aliceStr
        //             .Split()
        //             .Select(int.Parse)
        //             .OrderDescending());
        //        });

        //        var t2 = Task.Run(() =>
        //        {
        //            return new LinkedList<int>(
        //             bobStr
        //             .Split()
        //             .Select(int.Parse)
        //             .OrderDescending());
        //        });

        //        var alice = await t1;
        //        var bob = await t2;

        //        tests.Add((alice, bob));
        //    }

        //    foreach (var (a, b) in tests)
        //    {
        //        while (true)
        //        {
        //            Move(a, b);

        //            if (b.First == null)
        //            {
        //                Console.WriteLine("Alice");
        //                break;
        //            }

        //            FirstToMin(b);

        //            Move(b, a);

        //            if (a.First == null)
        //            {
        //                Console.WriteLine("Bob");
        //                break;
        //            }

        //            FirstToMin(a);
        //        }
        //    }
        //}

        //public static void Move(LinkedList<int> a, LinkedList<int> b)
        //{
        //    if (a.First.Value >= b.First.Value)
        //    {
        //        b.RemoveFirst();
        //    }
        //    else
        //    {
        //        b.First.Value -= a.First.Value;
        //    }
        //}

        //public static LinkedListNode<int> FirstToMin(LinkedList<int> list)
        //{
        //    var current = list.First;

        //    while (current.Next != null)
        //    {
        //        var next = current.Next;

        //        if (next.Value > current.Value)
        //        {
        //            var temp = next.Value;
        //            next.Value = current.Value;
        //            current.Value = temp;

        //            current = current.Next;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    return current;
        //}
    }
}
