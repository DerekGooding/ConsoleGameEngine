using System.Text;

namespace CGE_GameObject;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new ConsoleGameObject();
        f.Start();
    }
}
