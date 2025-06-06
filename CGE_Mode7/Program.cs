using System.Text;

namespace CGE_Mode7;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new ConsoleMode7();
        f.Start();
    }
}
