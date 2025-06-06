using System.Text;

namespace CGE_Animation;

    internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new ConsoleAnimation();
        f.Start();
    }
}
