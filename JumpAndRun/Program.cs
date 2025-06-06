using System.Text;

namespace JumpAndRun;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new JumpAndRun();
        f.Start();
    }
}
