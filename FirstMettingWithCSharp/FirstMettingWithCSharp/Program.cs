using System.Runtime.InteropServices;

class Program
{
    static void firstTask()
    {
        int firstNumber = 1;
        int secondNumber = 2;
        (firstNumber, secondNumber) = (secondNumber, firstNumber);
    }
    static void secondTask()
    {
        int number = 123;
        int reversed = (number % 10) * 100 + ((number / 10) % 10) * 10 + (number / 100);
    }
    static void thirdTask()
    {
        int H = 20;
        H %= 12;
        int angle = H * 30;
    }
    static void fourthTask()
    {
        int N = 20;
        int X = 3;
        int Y = 5;

        int count = 0;
        for (int i = 0; i < N; i++)
        {
            if (i % X == 0 || i % Y == 0)
            {
                count++;
            }
        }
    }
    static void fifthTask()
    {
        int a = 1900;
        int b = 2020;

        int leapYearsCount = (b / 4 - b / 100 + b / 400) - ((a - 1) / 4 - (a - 1) / 100 + (a - 1) / 400);
    }
    static void sixthTask()
    {
        int x0 = 3, y0 = 4;

        int x1 = 1, y1 = 2;
        int x2 = 5, y2 = 6;

        int A = y2 - y1;
        int B = x1 - x2;
        int C = x2 * y1 - y2 * x1;

        double distance = Math.Abs(A * x0 + B * y0 + C) / Math.Sqrt(A * A + B * B);
    }
    static void seventhTask()
    {
        int A = 3;
        int B = 4;

        int[] paralleVector = { -B, A };
        int[] perpedVector = { B, -A };
    }
    static void eigthTask()
    {
        int A = 1;
        int B = -3, C = 5;

        int x1 = 1, y1 = 2;

        int denominator = A * A + B * B;
        int x = (B * (B * x1 - A * y1) - A * C) / denominator;
        int y = (A * (-B * x1 + A * y1) - B * C) / denominator;
    }
}