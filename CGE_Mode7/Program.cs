using System;
using System.Text;

namespace CGE_Mode7;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new ConsoleMode7();
        f.Start();
    }
}
