using System.Text;

namespace SpriteEditor;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new SpriteEditor();
        f.Start();
    }
}
