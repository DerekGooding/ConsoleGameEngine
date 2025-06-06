using System;
using System.Text;

namespace JumpAndRun;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new JumpAndRun();
        f.Start();
    }
}
