using System.Text;

namespace CGE_UIElements_Simple;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new CGE_UIElements_Simple();
        f.Start();
    }
}
