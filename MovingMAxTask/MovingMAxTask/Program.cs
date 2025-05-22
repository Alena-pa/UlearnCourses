using System;
using System.Collections.Generic;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        if (windowWidth <= 0)
            throw new ArgumentException("Window width must be positive.", nameof(windowWidth));

        var deque = new LinkedList<(double value, int index)>();
        var index = 0;
        var buffer = new List<DataPoint>();

        foreach (var point in data)
        {
            while (deque.Count > 0 && deque.Last.Value.value <= point.OriginalY)
                deque.RemoveLast();

            deque.AddLast((point.OriginalY, index));

            while (deque.Count > 0 && deque.First.Value.index <= index - windowWidth)
                deque.RemoveFirst();

            yield return point.WithMaxY(deque.First.Value.value);

            index++;
        }
    }
}