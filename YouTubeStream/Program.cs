using System.Text;

namespace YouTubeStream;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        if (args.Length > 0)
        {
            using var f = new YouTubeStream(args[0]);
            f.Start();
        }
        else
        {
            using var f = new YouTubeStream("empty");
            f.Start();
        }
    }
}