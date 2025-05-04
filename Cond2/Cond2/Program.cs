using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter x y z: ");
        string[] brickInput = Console.ReadLine().Split();
        int x = int.Parse(brickInput[0]);
        int y = int.Parse(brickInput[1]);
        int z = int.Parse(brickInput[2]);

        Console.WriteLine("Enter a b");
        string[] holeInput = Console.ReadLine().Split();
        int a = int.Parse(holeInput[0]);
        int b = int.Parse(holeInput[1]);

        if (WillFit(x, y, z, a, b)){
            Console.WriteLine("will fit");
        }
        else
        {
            Console.WriteLine("will not fit");
        }
    }
    public static bool WillFit(int x, int y, int z, int a, int b)
    {
        var bricksSide = new List<int>{x, y, z };
        bricksSide.Sort();
        int min1 = bricksSide[0];
        int min2 = bricksSide[1];

        var holesSides = new List<int> { a, b };
        holesSides.Sort();
        int holeMin = holesSides[0];
        int holeMax = holesSides[1];

        return min1 <= holeMin && min2 <= holeMax;
    }
}
