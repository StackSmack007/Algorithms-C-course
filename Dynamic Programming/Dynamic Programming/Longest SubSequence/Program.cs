using System;
using System.Collections.Generic;
using System.Linq;

namespace Longest_SubSequence
{
    class Program
    {

        public class SequenceData
        {
            static void Main(string[] args)
            {
                var input = Console.ReadLine();
                var data = new SequenceData(input);
                MarkScore(data);
                //last bigest score's index
                int lastIndex = Array.IndexOf(data.Scores, data.Scores.OrderByDescending(x => x).First());

                var result = new Stack<int>();
                
                while (lastIndex!=-1)
                {
                    result.Push(data.Sequence[lastIndex]);
                    lastIndex = data.PrevBest[lastIndex];
                }
                Console.WriteLine(string.Join(" ",result));
            }
            public SequenceData(string input)
            {
                Sequence = input.Split(' ').Select(int.Parse).ToArray();
                Scores = new int[Sequence.Length];
                PrevBest = new int[Sequence.Length];
            }
            public int[] Sequence { get; set; }
            public int[] Scores { get; set; }
            public int[] PrevBest { get; set; }
            public int Length => this.Sequence.Length;
        }

        public static void MarkScore(SequenceData data, int index = 0)
        {
            if (index == data.Length)
            {
                return;
            }

            int scoreBest = 1;
            int prevIndexBest = -1;
            for (int i = 0; i < index; i++)
            {
                if (data.Sequence[i] < data.Sequence[index] && data.Scores[i] >= scoreBest)
                {
                    scoreBest = data.Scores[i] + 1;
                    prevIndexBest = i;
                }
            }

            data.Scores[index] = scoreBest;
            data.PrevBest[index] = prevIndexBest;
            MarkScore(data, index + 1);
        }
    }
}
