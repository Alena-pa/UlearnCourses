using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        bool first = true;
        double prevSmoothedY = 0;

        foreach (var point in data)
        {
            double smoothedY;

            if (first)
            {
                smoothedY = point.OriginalY;
                first = false;
            }
            else
            {
                smoothedY = alpha * point.OriginalY + (1 - alpha) * prevSmoothedY;
            }

            prevSmoothedY = smoothedY;
            yield return point.WithExpSmoothedY(smoothedY);
        }
    }
}
