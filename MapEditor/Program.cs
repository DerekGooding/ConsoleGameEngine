using System.Text;

namespace MapEditor;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new MapEditor();
        f.Start();
    }
}
