using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab
{
    public class Program
    {
        static void Main(string[] args)
        {
            Task1();
        }

        private static void Task1()
        {
            var coins = Console.ReadLine().Replace("Coins: ", "").Split(", ").Select(int.Parse).ToArray();
            int sum = int.Parse(Console.ReadLine().Replace("Sum: ", ""));

            IDictionary<int, int> coinCount = GetOptimalCoinCombo(sum, coins);
            Console.WriteLine($"Number of coins to take: {coinCount.Sum(x => x.Key * x.Value)}");

            foreach (var kvp in coinCount)
            {
                Console.WriteLine($"{kvp.Value} coin(s) with value {kvp.Key}");
            }
        }

        private static bool IsGreedyFitting(int[] coins)
        {
            if (coins.Length < 3) return true;
            coins = coins.OrderBy(x => x).ToArray();
            for (int i = 1; i < coins.Length - 1; i++)
            {
                if (coins[i - 1] + coins[i + 1] < 2 * coins[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static IDictionary<int, int> GetOptimalCoinCombo(int sum, int[] coins) =>
        IsGreedyFitting(coins) ? GetCoinsGreedy(sum, coins) : GetCoinsBrute(sum, coins);

        private static IDictionary<int, int> GetCoinsBrute(int sum, int[] coins)
        {
            var results = new HashSet<IDictionary<int, int>>();

            GetAllCoinOptions(sum, coins, results, new Dictionary<int, int>());

            if (!results.Any()) throw new InvalidOperationException($"The sum of {sum} can't be composed of coins: {string.Join(", ", coins.OrderBy(x => x))}");

            return results.OrderBy(x => x.Sum(ct => ct.Value)).First();
        }

        private static void GetAllCoinOptions(int total, int[] coins, HashSet<IDictionary<int, int>> salutionBank, Dictionary<int, int> salution)
        {
            int currentSum = salution.Sum(x => x.Key * x.Value);
            if (currentSum == total)
            {
                salutionBank.Add(new Dictionary<int, int>(salution));
                return;
            }

            for (int i = 0; i < coins.Length; i++)
            {
                int coinValue = coins[i];
                if (coinValue + currentSum <= total)
                {
                    if (!salution.ContainsKey(coinValue))
                    {
                        salution[coinValue] = 0;
                    }
                    salution[coinValue]++;

                    GetAllCoinOptions(total, coins, salutionBank, salution);

                    if (salution[coinValue] > 1) salution[coinValue]--;
                    else salution.Remove(coinValue);
                }
            }
        }

        private static IDictionary<int, int> GetCoinsGreedy(int sum, int[] coins)
        {
            coins = coins.OrderBy(x => x).ToArray();
            var result = new Dictionary<int, int>();
            var currentAmmount = sum;

            for (int i = coins.Length - 1; i >= 0; i--)
            {
                var coin = coins[i];
                if (currentAmmount >= coin)
                {
                    int ammount = currentAmmount / coin;
                    result[coin] = ammount;
                    currentAmmount -= ammount * coin;
                }

                if (currentAmmount == 0) break;
            }

            if (currentAmmount > 0) throw new ArgumentOutOfRangeException("Error");

            return result;
        }
    }
}