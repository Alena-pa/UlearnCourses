using System;
using System.Diagnostics.CodeAnalysis;

class Program
{
    static void Main()
    {
        double a = 10;
        double r = 6;

        double halfSide = a / 2;
        double maxRadius = a / Math.Sqrt(2);
        double eatenArea = 0;

        if (r <= halfSide)
        {
            eatenArea = Math.PI * r * r;
        }
        else if (r >= maxRadius)
        {
            eatenArea = a * a;
        }
        else
        {
            int steps = 10000;
            double dx = a / steps;
            for (int i = 0; i < steps; i++)
            {
                double x = -halfSide + i * dx;
                for (int j = 0; j < steps; j++)
                {
                    double y = -halfSide + j * dx;
                    if (x * x + y * y <= r * r)
                    {
                        eatenArea += dx * dx;
                    }
                }
            }
        }
        Console.WriteLine($"eaten area: {eatenArea}");
    }
}