using System;
using System.Text;

namespace CGE_Fonts;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new Fonts();
        f.Start();
    }
}
