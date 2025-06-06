using System;
using System.Text;

namespace CGE_PopUp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new CGE_PopUp();
        f.Start();
    }
}
