using System.Collections.Concurrent;

namespace ConcurrentCollections1
{
    internal class Program
    {
        private static readonly object _lock = new object();

        private static void Main(string[] args)
        {
            try
            {
                DemoUpdateData();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception:{ex.Message}");
                Console.ResetColor();
            }
        }

        private static void DemoUpdateData()
        {
            var robots = new ConcurrentDictionary<string, int>();

            robots.TryAdd(key: "robot1", value: 10);
            robots.TryAdd(key: "robot2", value: 20);
            robots.TryAdd(key: "robot3", value: 30);
            robots.TryAdd(key: "robot4", value: 40);

            if (robots.TryAdd(key: "robot4", value: 44))
            {
                Console.WriteLine("robot4 added to dictionary");
            }
            else
            {
                Console.WriteLine("robot4 not added to dictionary");
            }

            PrintAllRobots(robots);

            var t1 = Task.Run(() => Update2(robots));
            var t2 = Task.Run(() => Update2(robots));
            var t3 = Task.Run(() => Update2(robots));

            Task.WaitAll(t1, t2, t3);

            PrintAllRobots(robots);

            Console.ReadLine();
        }

        private static void Update2(ConcurrentDictionary<string, int> robots)
        {
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} Start");

            var foundCount = SearchForGems();
            var currentCount = robots["robot3"];
            var b = robots.TryUpdate("robot3", foundCount + currentCount, currentCount);

            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} update success: {b}");
        }

        private static void Update1(ConcurrentDictionary<string, int> robots)
        {
            lock (_lock)
            {
                var foundCount = SearchForGems();
                var currentCount = robots["robot3"];
                robots["robot3"] = foundCount + currentCount;
            }
        }

        private static void PrintAllRobots(ConcurrentDictionary<string, int> robots)
        {
            Console.WriteLine("print all robots starts");
            foreach (var robot in robots)
            {
                Console.WriteLine(robot.ToString());
            }
            Console.WriteLine();
        }

        private static int SearchForGems() => 3;

        private static void Demo1()
        {
            // dictionary operations
            // Add, Remove, Update, Count
            // TryAdd, TryGetValue

            //var robots = new Dictionary<int, Robot>();
            var robots = new ConcurrentDictionary<int, Robot>();

            Robot robot1, robot2, robot3, robot4, currentRobot, tryRobot;

            #region CreateRobots

            robot1 = new Robot()
            {
                Id = 1,
                Name = "Robot 1",
                Team = "Star-chasers",
                TeamColor = ConsoleColor.DarkYellow,
                GemstoneCount = 10
            };
            robot2 = new Robot()
            {
                Id = 2,
                Name = "Robot 2",
                Team = "Star-chasers",
                TeamColor = ConsoleColor.DarkYellow,
                GemstoneCount = 10
            };

            robot3 = new Robot()
            {
                Id = 3,
                Name = "Robot 3",
                Team = "Deltron",
                TeamColor = ConsoleColor.Cyan,
                GemstoneCount = 10
            };
            robot4 = new Robot()
            {
                Id = 4,
                Name = "Robot 4",
                Team = "Deltron",
                TeamColor = ConsoleColor.Magenta,
                GemstoneCount = 10
            };

            #endregion CreateRobots

            robots.TryAdd(robot1.Id, robot1);
            robots.TryAdd(robot2.Id, robot2);
            robots.TryAdd(robot3.Id, robot3);
            robots.TryAdd(robot4.Id, robot4);

            if (!robots.TryAdd(robot4.Id, robot4))
            {
                // Adds robot successfully when it is not already in dictionary
                // returns false if robot exists in dictionary, without throwing exception
                Console.WriteLine("Cannot add, robot already in the dictionary.");
            }

            WriteHeaderToConsole("List all items in dictionary");
            Console.WriteLine($"Team count: {robots.Count}");
            foreach (var keyPair in robots)
            {
                Console.ForegroundColor = keyPair.Value.TeamColor;
                Console.WriteLine(keyPair.Value);
            }
            robots.TryRemove(1, out _);
            currentRobot = robots[3];
            currentRobot.GemstoneCount += 1;
            robots[3] = currentRobot;

            var newRobot = new Robot()
            {
                Id = 66,
                Name = "Robot 66",
                Team = "Deltron",
                TeamColor = ConsoleColor.Magenta,
                GemstoneCount = 10
            };
            var r1 = robots.GetOrAdd(newRobot.Id, newRobot); //add + get
            var r2 = robots.GetOrAdd(newRobot.Id, newRobot); //get

            WriteHeaderToConsole("List after removing a robot");
            Console.WriteLine($"Team count: {robots.Count}");
            foreach (var keyPair in robots)
            {
                Console.ForegroundColor = keyPair.Value.TeamColor;
                Console.WriteLine(keyPair.Value);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Use .TryGetValue");

            robots.TryGetValue(3, out tryRobot);
            Console.WriteLine(tryRobot);
            Console.ResetColor();
        }

        private static void WriteHeaderToConsole(string headerText)
        {
            Console.ResetColor();
            Console.WriteLine("-----------------------------");
            Console.WriteLine(headerText);
            Console.WriteLine("-----------------------------");
        }
    }
}