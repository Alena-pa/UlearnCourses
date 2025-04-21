using System;

class Programm
{
    static void Main()
    {
        int summ = 0;
        for (int i = 0; i < 1000; i++)
        {
            if (i % 3 == 0 || i % 5 == 0)
            {
                summ += i;
            }
        }
        Console.WriteLine(summ);
    }
}