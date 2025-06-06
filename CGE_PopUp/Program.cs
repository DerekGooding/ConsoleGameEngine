using System.Text;

namespace CGE_PopUp;

internal static class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new CGE_PopUp();
        f.Start();
    }
}
