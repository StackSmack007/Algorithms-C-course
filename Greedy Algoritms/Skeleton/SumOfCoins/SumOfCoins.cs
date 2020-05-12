namespace SumOfCoins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SumOfCoins
    {
        public static void Main(string[] args)
        {
            var availableCoins = new[] { 1, 2, 5, 10, 20, 50 };
            var targetSum = 923;

            var selectedCoins = ChooseCoins(availableCoins, targetSum);

            Console.WriteLine($"Number of coins to take: {selectedCoins.Values.Sum()}");
            foreach (var selectedCoin in selectedCoins)
            {
                Console.WriteLine($"{selectedCoin.Value} coin(s) with value {selectedCoin.Key}");
            }
        }

        public static Dictionary<int, int> ChooseCoins(IList<int> coins, int sum)
        {
            coins = coins.OrderBy(x => x).ToArray();
            var result = new Dictionary<int, int>();
            var currentAmmount = sum;

            for (int i = coins.Count() - 1; i >= 0; i--)
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

            if (currentAmmount > 0) throw new InvalidOperationException("Error");

            return result;
        }

    }
}