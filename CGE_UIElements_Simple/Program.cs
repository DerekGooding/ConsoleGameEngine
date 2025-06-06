using System;
using System.Text;

namespace CGE_UIElements_Simple;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new CGE_UIElements_Simple();
        f.Start();
    }
}
