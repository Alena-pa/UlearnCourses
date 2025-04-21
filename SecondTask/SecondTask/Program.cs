using System;

class Program
{
    static void Main()
    {
        int hours = 1;
        int minutes = 30;
        hours = hours % 12;
        double hourAngle = 30 * hours + 0.5 * minutes;
        double minuteAngle = 6 * minutes;

        double angle = Math.Abs(hourAngle - minuteAngle);
        double answer = Math.Min(angle, 360 - angle);

        Console.WriteLine(answer);
    }
}