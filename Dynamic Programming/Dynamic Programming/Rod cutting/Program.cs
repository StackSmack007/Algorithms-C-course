using System;
using System.Collections.Generic;
using System.Linq;

namespace Rod_cutting
{
    class Program
    {

        class RodComb
        {
            public int Price { get; set; }
            public List<int> Elements { get; set; }
            public RodComb()
            {
                Elements = new List<int>();
            }
        }

        static void Main(string[] args)
        {
            int[] lengthPrices = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            int rodLength = int.Parse(Console.ReadLine());

            var info = new RodComb[lengthPrices.Length];
            GetOptPricings(lengthPrices, info);

            var firstToAdd = info[rodLength % lengthPrices.Length];
            int totalPrice = firstToAdd.Price;
            List<int> pieces = firstToAdd.Elements.ToList();

            if (rodLength > lengthPrices.Length)
            {
                var lastEl = info.Last();
                int wholeLengths = rodLength / lengthPrices.Length;
                totalPrice += lastEl.Price * wholeLengths;
                for (int i = 0; i < wholeLengths; i++)
                {
                    pieces.AddRange(lastEl.Elements);
                }
            }
            pieces = pieces.OrderBy(x => x).ToList();

            Console.WriteLine(totalPrice);
            Console.WriteLine(string.Join(" ", pieces));
        }
        private static void GetOptPricings(int[] prices, RodComb[] info)
        {
            for (int i = 0; i < prices.Length; i++)
            {
                info[i] = new RodComb()
                {
                    Price = prices[i],
                    Elements = new List<int>() { i }
                };

                var current = info[i];

                for (int j = 0; j <= i / 2; j++)
                {
                    RodComb first = info[j];
                    RodComb second = info[i - j];
                    int priceSum = first.Price + second.Price;
                    if (priceSum > current.Price)
                    {
                        current.Price = priceSum;
                        var elmnts = first.Elements.ToList();
                        elmnts.AddRange(second.Elements);
                        current.Elements = elmnts;
                    }
                }
            }
        }
    }
}

