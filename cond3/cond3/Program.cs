using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter ticket number: ");
        string input = Console.ReadLine();
        if (input.Length != 6 || !int.TryParse(input, out int ticketNumber))
        {
            Console.WriteLine("Incorrect input");
            return;
        }

        if (IsDeltaOneLucky(ticketNumber))
            Console.WriteLine("yep");
        else
            Console.WriteLine("nope");
    }
    public static bool CheckTicket(int ticket)
    {
        string s = ticket.ToString("D6");
        int sum1 = s[0] - '0' + s[1] - '0' + s[2] - '0';
        int sum2 = s[3] - '0' + s[4] - '0' + s[5] - '0';
        return sum1 == sum2;
    }

    static bool IsDeltaOneLucky(int ticket)
    {
        string s = ticket.ToString("D6");
        int sum1 = s[0] - '0' + s[1] - '0' + s[2] - '0';
        int sum2 = s[3] - '0' + s[4] - '0' + s[5] - '0';

        if (Math.Abs(sum1 - sum2) != 1)
        {
            return false;
        }
        return CheckTicket(ticket - 1) || CheckTicket(ticket + 1);
    }
}