using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            int[,] opt = BuildLcsMatrix(first, second);
            return ReconstructLcs(first, second, opt);
        }

        private static int[,] BuildLcsMatrix(List<string> first, List<string> second)
        {
            int len1 = first.Count;
            int len2 = second.Count;
            int[,] opt = new int[len1 + 1, len2 + 1];

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (first[i - 1] == second[j - 1])
                    {
                        opt[i, j] = opt[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        opt[i, j] = System.Math.Max(opt[i - 1, j], opt[i, j - 1]);
                    }
                }
            }

            return opt;
        }

        private static List<string> ReconstructLcs(List<string> first, List<string> second, int[,] opt)
        {
            var lcs = new List<string>();
            int i = first.Count;
            int j = second.Count;

            while (i > 0 && j > 0)
            {
                if (TokensMatch(first, second, i, j))
                {
                    lcs.Add(first[i - 1]);
                    MoveDiagonally(ref i, ref j);
                }
                else if (CameFromTop(opt, i, j))
                {
                    i--;
                }
                else
                {
                    j--;
                }
            }

            lcs.Reverse();
            return lcs;
        }

        private static bool TokensMatch(List<string> first, List<string> second, int i, int j)
        {
            return first[i - 1] == second[j - 1];
        }

        private static bool CameFromTop(int[,] opt, int i, int j)
        {
            return opt[i - 1, j] >= opt[i, j - 1];
        }

        private static void MoveDiagonally(ref int i, ref int j)
        {
            i--;
            j--;
        }

    }
}