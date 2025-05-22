using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var window = new Queue<double>();
        double sum = 0;

        foreach (var point in data)
        {
            window.Enqueue(point.OriginalY);
            sum += point.OriginalY;

            if (window.Count > windowWidth)
            {
                sum -= window.Dequeue();
            }

            double average = sum / window.Count;
            yield return point.WithAvgSmoothedY(average);
        }
    }
}