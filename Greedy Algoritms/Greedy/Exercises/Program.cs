using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercises
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task01();
            //  Task02();
            //  Task03();
            //  Task04();
            Task05();
        }
        #region Egyptian Fractions
        private static void Task05()
        {
            ulong[] tokens = Console.ReadLine().Split('/').Select(ulong.Parse).ToArray();
            var nominator = tokens[0];
            var denominator = tokens[1];

            if (nominator >= denominator || nominator == 0)
            {
                Console.WriteLine("Error (fraction is equal to or greater than 1)");
                return;
            }
            var delimiters = new List<ulong>();
            ulong delimiter = 1;
            
            while (nominator > 0)
            {
                delimiter++;
                if (nominator*delimiter < denominator) continue;
                delimiters.Add(delimiter);
                nominator = nominator * delimiter - denominator;
                denominator *= delimiter;
            }

            Console.WriteLine($"{tokens[0]}/{tokens[1]} = {string.Join(" + ", delimiters.Select(x => $"1/{x}"))}");
        }
        #endregion

        #region LectureSchedule
        private static void Task04()
        {
            int count = int.Parse(Console.ReadLine().Substring(10));
            Lecture[] lectures = new Lecture[count];
            for (int i = 0; i < count; i++)
            {
                string[] input = Console.ReadLine().Split(new[] { ": ", " - " }, StringSplitOptions.RemoveEmptyEntries);
                lectures[i] = new Lecture
                {
                    Name = input[0],
                    Start = int.Parse(input[1]),
                    End = int.Parse(input[2])
                };
            }

            var selected = GetLectures(lectures);

            Console.WriteLine($"Lectures ({selected.Count()}):");
            foreach (var lec in selected)
            {
                Console.WriteLine(lec);
            }
        }

        private static IList<Lecture> GetLectures(Lecture[] lectures)
        {
            var selected = new List<Lecture>();
            lectures = lectures.OrderBy(x => x.End).ToArray();
            int currentTime = 0;

            while (lectures.Any(x => !selected.Contains(x) && x.Start >= currentTime))
            {
                var cur = lectures.First(x => !selected.Contains(x) && x.Start >= currentTime);
                selected.Add(cur);
                currentTime = cur.End;
            }

            return selected;
        }

        private class Lecture
        {
            public string Name { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
            public override string ToString() =>
                   $"{Start}-{End} -> {Name}";
        }
        #endregion

        #region KnightTour
        private static void Task03()
        {
            var cm = new ChessMaster();
            int size = int.Parse(Console.ReadLine());
            var result = cm.GetMovesKnight(size);
            foreach (var row in result)
            {
                Console.WriteLine(" " + string.Join(" ", row.Select(x => x.ToString().PadLeft(3))));
            }
        }

        public class ChessMaster
        {
            private int[][] board;
            public int[][] GetMovesKnight(int size, int row = 0, int col = 0)
            {
                this.board = MakeBoard(size);

                int counter = 0;
                Location nextMove = new Location(row, col);

                do
                {
                    row = nextMove.Row;
                    col = nextMove.Col;
                    board[row][col] = ++counter;
                    nextMove = GetRoutes(row, col).FirstOrDefault();
                }
                while (nextMove != null);

                return this.board;
            }

            private int[][] MakeBoard(int size)
            {
                var board = new int[size][];
                for (int i = 0; i < size; i++)
                {
                    board[i] = new int[size];
                }

                return board;
            }

            private int[] GetNextStep()
            {
                var coords = new int[2];

                return coords;
            }

            private ICollection<Location> GetRoutes(int baseRow, int baseCol)
            {
                return GetDirections(baseRow, baseCol)
                    .Select(x => new Location
                    (
                        x[0],
                        x[1],
                        GetDirections(x[0], x[1]).Count()
                    ))
                    .OrderBy(x => x.Power)
                    .ToArray();
            }

            private int[][] GetDirections(int row, int col)
            {
                return new[]
                {
                new []{row+1,col+2},
                new []{row-1,col+2},
                new []{row+1,col-2},
                new []{row-1,col-2},
                new []{row+2,col+1},
                new []{row+2,col-1},
                new []{row-2,col+1},
                new []{row-2,col-1},
            }.Where(x => IsInBoard(x[0], x[1]) && IsFree(x[0], x[1])).ToArray();
            }
            private bool IsFree(int row, int col) => this.board[row][col] == 0;
            private bool IsInBoard(int row, int col) =>
                    row >= 0 && col >= 0 && row < board.Length && col < board.Length;

            private class Location
            {
                public Location(int row, int col, int power = -1)
                {
                    Row = row;
                    Col = col;
                    Power = power;
                }

                public int Row { get; }
                public int Col { get; }
                public int Power { get; }
            }
        }
        #endregion

        #region CPU Schedule
        private static void Task02()
        {
            int taskCount = int.Parse(Console.ReadLine().Replace("Tasks: ", ""));
            var tasks = new TaskCPU[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                var data = Console.ReadLine().Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                tasks[i] = new TaskCPU(data[0], data[1], i + 1);
            }

            var schedule = GenerateSchedule(tasks);

            Console.WriteLine($"Optimal schedule: {string.Join(" -> ", schedule.Select(x => x.Position))}");
            Console.WriteLine($"Total value: {schedule.Sum(x => x.Value)}");
        }

        private static TaskCPU[] GenerateSchedule(TaskCPU[] tasks)
        {
            int maxStep = Math.Min(tasks.Length, tasks.OrderByDescending(x => x.Deadline).FirstOrDefault().Deadline);

            var comboBox = new List<TaskCPU[]>(maxStep);

            GetAllCombsRec(tasks.OrderByDescending(x => x.Value).ToArray(), new List<TaskCPU>(maxStep), comboBox);

            return comboBox.OrderByDescending(x => x.Sum(t => t.Value)).FirstOrDefault();
        }

        private static void GetAllCombsRec(TaskCPU[] tasks, List<TaskCPU> schedule, List<TaskCPU[]> comboBox, int step = 1)
        {
            if (schedule.Count() == schedule.Capacity)
            {
                comboBox.Add(schedule.ToArray());
            }

            for (int i = 0; i < tasks.Count(); i++)
            {
                var currentTask = tasks[i];
                if (schedule.Contains(currentTask) || currentTask.Deadline < step) continue;
                schedule.Add(currentTask);
                GetAllCombsRec(tasks, schedule, comboBox, step + 1);
                schedule.Remove(currentTask);
            }
        }
        private class TaskCPU
        {
            public TaskCPU(int value, int deadline, int position)
            {
                Value = value;
                Deadline = deadline;
                Position = position;
            }

            public int Value { get; }
            public int Deadline { get; }
            public int Position { get; }

        }
        #endregion

        #region Knapsack
        public static void Task01()
        {
            int capacity = int.Parse(Console.ReadLine().Replace("Capacity: ", ""));
            int itemCount = int.Parse(Console.ReadLine().Replace("Items: ", ""));

            IList<Item> items = new List<Item>();
            for (int i = 0; i < itemCount; i++)
            {
                int[] weightValue = Console.ReadLine().Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                items.Add(new Item(weightValue[0], weightValue[1]));
            }

            StealBest(capacity, items);
            var stolenGoods = items.Where(x => x.Stolen > 0).OrderByDescending(x => x.PercentageStolen);//.ThenBy(x => x.TotalPrice).ThenBy(x=>x.Ammount);
            foreach (var theft in stolenGoods)
            {
                var stolenPerc = theft.PercentageStolen == 100 ? "100" : theft.PercentageStolen.ToString("F2");
                Console.WriteLine($"Take {stolenPerc}% of item with price {theft.TotalPrice:F2} and weight {theft.Ammount:F2}");
            }
            Console.WriteLine($"Total price: {stolenGoods.Sum(x => x.Stolen * x.UnitWorth):F2}");
        }

        private static void StealBest(int capacity, IList<Item> items)
        {

            foreach (var item in items.OrderByDescending(x => x.UnitWorth))
            {
                if (capacity == 0) return;

                var stealableQuantity = Math.Min(capacity, item.Ammount);
                item.Stolen = stealableQuantity;
                capacity -= stealableQuantity;
            }
        }
        private class Item
        {
            public Item(decimal totalPrice, int ammount, int used = 0)
            {
                Ammount = ammount;
                TotalPrice = totalPrice;
                Stolen = used;
            }
            public decimal TotalPrice { get; }
            public int Ammount { get; }
            public decimal UnitWorth => (decimal)TotalPrice / Ammount;
            public int Stolen { get; set; }
            public decimal PercentageStolen => 100m * Stolen / Ammount;
        }
        #endregion
    }
}
