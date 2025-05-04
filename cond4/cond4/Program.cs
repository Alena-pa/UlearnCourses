using System;
using System.ComponentModel.DataAnnotations;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter A B");
        string[] firstInput = Console.ReadLine().Split();
        int A = int.Parse(firstInput[0]);
        int B = int.Parse(firstInput[1]);

        Console.WriteLine("Enter C D");
        string[] secondInput = Console.ReadLine().Split();
        int C = int.Parse(secondInput[0]);
        int D = int.Parse(secondInput[1]);

        if (Math.Max(A, B) <= Math.Min(C, D))
        {
            Console.WriteLine("yep");
        }
        else
        {
            Console.WriteLine("nope");
        }
    }
}