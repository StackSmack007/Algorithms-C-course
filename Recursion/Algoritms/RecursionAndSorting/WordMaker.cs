using System;
using System.Collections.Generic;
using System.Linq;

namespace RecursionAndSorting
{
    public class WordMaker
    {
        public HashSet<string> GetWords(string word)
        {
            if (NoRepeatChars(word))
            {
                int count = 1;
                for (int i = 1; i <= word.Length; i++)
                {
                    count *= i;
                }
                Console.WriteLine(count);
            }
            var indexCombos = new HashSet<string>();
            GetWords(word, new List<char>(word.Length), indexCombos);
            return indexCombos;
        }

        private bool NoRepeatChars(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                for (int j = i + 1; j < word.Length; j++)
                {
                    if (word[i] == word[j]) return false;
                }
            }
            return true;
        }

        private void GetWords(string word, IList<char> used, HashSet<string> bank)
        {
            if (used.Count() == word.Length)
            {
                bank.Add(string.Join("", used));
                return;
            }

            for (int i = 0; i < word.Length; i++)
            {
                char current = word[i];
                bool sameElementPrevious = used.Count() > 0 && used.Last() == current;
                if (sameElementPrevious) continue;
                bool allSymbolsUsed = used.Count(x => x == current) == word.Count(x => x == current);
                if (allSymbolsUsed) continue;
                used.Add(current);
                GetWords(word, used, bank);
                used.RemoveAt(used.Count() - 1);
            }
        }

        private const char delimiter = '|';
        public HashSet<T[]> GetCombsNoRepeat<T>(T[] elmnts) where T : IComparable
        {
            var indexCombos = new HashSet<string>();
            PopulateCombs(elmnts, new Stack<int>(), indexCombos);

            return indexCombos
                .Select(x => x.Split(delimiter).Select(int.Parse).Select(e => elmnts[e]).ToArray())
                .ToHashSet();
        }

        private void PopulateCombs<T>(T[] elmnts, Stack<int> indexesUsed, HashSet<string> bank) where T : IComparable
        {
            if (indexesUsed.Count == elmnts.Length)
            {
                var newComb = string.Join(delimiter, indexesUsed.Reverse());
                bank.Add(newComb);
                return;
            }

            for (int i = 0; i < elmnts.Length; i++)
            {
                bool sameElementUsedBefore = indexesUsed.Any() && elmnts[indexesUsed.Peek()].CompareTo(elmnts[i]) == 0;
                if (indexesUsed.Contains(i) || sameElementUsedBefore) continue;
                indexesUsed.Push(i);
                PopulateCombs(elmnts, indexesUsed, bank);
                indexesUsed.Pop();
            }
        }
    }
    public class WordGenerator
    {
        public HashSet<string> GetWords(char[] sortedArr)
        {
            var bank = new HashSet<string>();
            GetWordCombs(sortedArr, 0, sortedArr.Length - 1, bank);
            return bank;
        }
        private void GetWordCombs(char[] word, int start, int end, HashSet<string> bank)
        {
            if (IsValidWord(word)) bank.Add(string.Join("", word));
            for (int left = end - 1; left >= start; left--)
            {
                for (int right = left + 1; right <= end; right++)
                    if (word[left] != word[right])
                    {
                        Swap(word, left, right);
                        GetWordCombs(word, left + 1, end, bank);
                    }
                var firstElement = word[left];
                for (int i = left; i <= end - 1; i++)
                {
                    word[i] = word[i + 1];
                }
                word[end] = firstElement;
            }
        }

        private void Swap(char[] word, int left, int right)
        {
            var temp = word[left];
            word[left] = word[right];
            word[right] = temp;
        }

        private bool IsValidWord(char[] word)
        {
            for (int i = 1; i < word.Length; i++)
            {
                if (word[i - 1] == word[i]) return false;
            }
            return true;
        }
    }
}

