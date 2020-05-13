using System;
using System.Collections.Generic;
using System.Linq;

namespace KnapSack
{
    public class Program
    {
        private const string STOP_COMMAND = "end";
        private class Item
        {
            public Item(string name, int size, int value)
            {
                Name = name;
                Size = size;
                Value = value;
            }

            public string Name { get; }
            public int Size { get; }
            public int Value { get; }
        }

        static void Main(string[] args)
        {
            int capacity = int.Parse(Console.ReadLine());
            var availableItems = new List<Item>();
            var input = string.Empty;
            while ((input = Console.ReadLine()).ToLower() != STOP_COMMAND)
            {
                string name = input.Split(' ')[0];
                int size = int.Parse(input.Split(' ')[1]);
                int value = int.Parse(input.Split(' ')[2]);
                availableItems.Add(new Item(name, size, value));
            }

            //rows -> itemsAvailable
            //cols -> capacity 0-cap
            //3d -> [0] - value, [1] - isUsed=1/notUsed=0;
            //+1 for the 0cap and 0 item to be included!;
            int[,,] resultSet = GetItemsMatrix(capacity, availableItems);

            ICollection<Item> stolenItems = GetItems(availableItems, resultSet);
            PrintResult(stolenItems);
        }

        private static void PrintResult(ICollection<Item> stolenItems)
        {
            Console.WriteLine($"Total Weight: {stolenItems.Sum(x => x.Size)}");
            Console.WriteLine($"Total Value: {stolenItems.Sum(x => x.Value)}");
            Console.WriteLine(string.Join(Environment.NewLine, stolenItems.OrderBy(x=>x.Name).Select(x => x.Name)));
        }

        private static ICollection<Item> GetItems(List<Item> availableItems, int[,,] resultSet)
        {
            var knapSack = new List<Item>();
            int row = resultSet.GetLength(0) - 1;
            int col = resultSet.GetLength(1) - 1;
            while (row >= 0)
            {
                bool isItemTaken = resultSet[row, col, 1] == 1;
                if (isItemTaken)
                {
                    var item = availableItems[row];
                    knapSack.Add(item);
                    col -= item.Size;
                }

                row--;
            }

            return knapSack;
        }

        private static int[,,] GetItemsMatrix(int capacity, List<Item> availableItems)
        {
            //contains extra first row of 0 items and extra first col for 0 capacity!
            int[,,] resultSet = new int[availableItems.Count() + 1, capacity + 1, 2];
            for (int row = 1; row < resultSet.GetLength(0); row++)
            {
                var item = availableItems[row - 1];
                for (int col = 1; col < resultSet.GetLength(1); col++)
                {
                    int notIncludedItem_Price = resultSet[row - 1, col, 0];
                    int includedItem_Price = col >= item.Size ? item.Value + resultSet[row - 1, col - item.Size, 0] : 0;
                    if (includedItem_Price > notIncludedItem_Price)
                    {
                        resultSet[row, col, 0] = includedItem_Price;
                        resultSet[row, col, 1] = 1;
                    }
                    else
                    {
                        resultSet[row, col, 0] = notIncludedItem_Price;
                    }
                }
            }

            //Does NOT contain extra first row of 0 items and extra first col for 0 capacity!
            int[,,] clearResult = new int[availableItems.Count(), capacity, 2];
            for (int row = 0; row < clearResult.GetLength(0); row++)
            {
                for (int col = 0; col < clearResult.GetLength(1); col++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        clearResult[row, col, i] = resultSet[row + 1, col + 1, i];
                    }
                }
            }

            return clearResult;
        }

        private static void PrintArr(int[,,] arr, int set)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j, set] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
