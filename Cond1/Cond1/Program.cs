using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the starting cell (eg e2): ");
        string start = Console.ReadLine().ToLower();

        Console.Write("Enter the ending cell (eg g3): ");
        string end = Console.ReadLine().ToLower();

        if (!IsValidCell(start) || !IsValidCell(end))
        {
            Console.WriteLine("Incorrect cell format.");
            return;
        }

        int x1 = start[0] - 'a' + 1;
        int y1 = start[1] - '1' + 1;

        int x2 = end[0] - 'a' + 1;
        int y2 = end[1] - '1' + 1;

        if (x1 == x2 && y1 == y2)
        {
            Console.WriteLine("The figure remained in place.");
            return;
        }

        if (IsBishopMove(x1, y1, x2, y2))
            Console.WriteLine("Elephant");

        if (IsKnightMove(x1, y1, x2, y2))
            Console.WriteLine("Knight");

        if (IsRookMove(x1, y1, x2, y2))
            Console.WriteLine("Knight");

        if (IsQueenMove(x1, y1, x2, y2))
            Console.WriteLine("Queen");

        if (IsKingMove(x1, y1, x2, y2))
            Console.WriteLine("King");
    }

    static bool IsValidCell(string cell)
    {
        return cell.Length == 2 &&
               cell[0] >= 'a' && cell[0] <= 'h' &&
               cell[1] >= '1' && cell[1] <= '8';
    }

    static bool IsBishopMove(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) == Math.Abs(y1 - y2);
    }

    static bool IsKnightMove(int x1, int y1, int x2, int y2)
    {
        int dx = Math.Abs(x1 - x2);
        int dy = Math.Abs(y1 - y2);
        return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
    }

    static bool IsRookMove(int x1, int y1, int x2, int y2)
    {
        return (x1 == x2 || y1 == y2);
    }

    static bool IsQueenMove(int x1, int y1, int x2, int y2)
    {
        return IsBishopMove(x1, y1, x2, y2) || IsRookMove(x1, y1, x2, y2);
    }

    static bool IsKingMove(int x1, int y1, int x2, int y2)
    {
        return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2)) == 1;
    }
}
