using System.Text;

namespace PNGToSpriteEditor;
internal static class Program
{
    static readonly Dictionary<byte, string> AnsiLookup;

    static void Main()
    {
        var cp437 = Encoding.GetEncoding(437);

        Console.WriteLine("Drag and drop a PNG- or Sprite (.txt)-File in here and press Enter");
        Console.WriteLine("For a new file type New:Width;Height");
        Console.Write("Your file here: ");

        var file = Console.ReadLine();

        using var f = new PNGToSpriteEditor(file);
        f.Start();
    }
}
