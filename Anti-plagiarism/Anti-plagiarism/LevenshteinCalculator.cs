using System;
using System.Collections.Generic;

using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var results = new List<ComparisonResult>();

            for (int i = 0; i < documents.Count; i++)
            {
                for (int j = i + 1; j < documents.Count; j++)
                {
                    double distance = CalculateLevenshteinDistance(documents[i], documents[j]);
                    results.Add(new ComparisonResult(documents[i], documents[j], distance));
                }
            }

            return results;
        }

        private double CalculateLevenshteinDistance(DocumentTokens doc1, DocumentTokens doc2)
        {
            int len1 = doc1.Count;
            int len2 = doc2.Count;
            double[,] dp = InitializeMatrix(len1, len2);

            FillMatrix(dp, doc1, doc2);

            return dp[len1, len2];
        }

        private double[,] InitializeMatrix(int len1, int len2)
        {
            var dp = new double[len1 + 1, len2 + 1];

            for (int i = 0; i <= len1; i++)
                dp[i, 0] = i;

            for (int j = 0; j <= len2; j++)
                dp[0, j] = j;

            return dp;
        }

        private void FillMatrix(double[,] dp, DocumentTokens doc1, DocumentTokens doc2)
        {
            int len1 = doc1.Count;
            int len2 = doc2.Count;

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    double substitutionCost = TokenDistanceCalculator.GetTokenDistance(doc1[i - 1], doc2[j - 1]);

                    dp[i, j] = Math.Min(
                        Math.Min(
                            dp[i - 1, j] + 1,         // Deletion
                            dp[i, j - 1] + 1),        // Insertion
                        dp[i - 1, j - 1] + substitutionCost); // Substitution
                }
            }
        }
    }
}
