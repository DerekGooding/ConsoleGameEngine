using System;
using System.Text;

namespace SpriteEditor;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new SpriteEditor();
        f.Start();
    }
}
