using System;
using System.Collections.Specialized;

public class Program
{
    public static void Main()
    {
        int h = 10000;
        int t = 500;
        int v = 50;
        int x = 10;

        (double minTime, double maxTime) = CalcEarPressureTime(h, t, v, x);
    }

    public static(double minTime, double maxTime) CalcEarPressureTime(int h, int t, int v, int x)
    {
        if (h > v * t)
        {
            throw new ArgumentException("Impossible");
        }

        double safeHeight = x * t;
        double minTime = 0;

        if (safeHeight >= h)
        {
            minTime = 0;
        }
        else
        {
            double dangerousHeight = h - safeHeight;
            minTime = dangerousHeight / v;
        }

        double maxTime = 0;

        if (v <= x)
        {
            maxTime = 0;
        }
        else
        {
            maxTime = h / v;
        }

            return (minTime, maxTime);
    }
}