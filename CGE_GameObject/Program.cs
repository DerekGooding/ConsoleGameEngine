using System;
using System.Text;

namespace CGE_GameObject;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new ConsoleGameObject();
        f.Start();
    }
}
