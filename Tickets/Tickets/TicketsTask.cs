using System.Numerics;

namespace Tickets;

public static class TicketsTask
{
    public static BigInteger Solve(int halfLen, int totalSum)
    {
        if (totalSum % 2 != 0)
            return 0;

        int targetSum = totalSum / 2;
        BigInteger[,] dp = new BigInteger[halfLen + 1, targetSum + 1];

        dp[0, 0] = 1;

        for (int i = 1; i <= halfLen; i++)
        {
            for (int s = 0; s <= targetSum; s++)
            {
                for (int d = 0; d <= 9 && d <= s; d++)
                {
                    dp[i, s] += dp[i - 1, s - d];
                }
            }
        }

        return dp[halfLen, targetSum] * dp[halfLen, targetSum];
    }
}