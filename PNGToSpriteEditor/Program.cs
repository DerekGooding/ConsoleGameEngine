using System;
using System.Collections.Generic;
using System.Text;

namespace PNGToSpriteEditor;
internal class Program
{
    static Dictionary<byte, string> AnsiLookup;

    static void Main(string[] args)
    {
        Encoding cp437 = Encoding.GetEncoding(437);

        Console.WriteLine("Drag and drop a PNG- or Sprite (.txt)-File in here and press Enter");
        Console.WriteLine("For a new file type New:Width;Height");
        Console.Write("Your file here: ");

        string file = Console.ReadLine();

        using var f = new PNGToSpriteEditor(file);
        f.Start();
    }
}
