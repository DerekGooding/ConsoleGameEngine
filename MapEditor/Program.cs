using System;
using System.Text;

namespace MapEditor;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new MapEditor();
        f.Start();
    }
}
