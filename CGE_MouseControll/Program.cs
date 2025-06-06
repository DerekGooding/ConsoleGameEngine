using System.Text;

namespace CGE_Fonts;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new Fonts();
        f.Start();
    }
}
