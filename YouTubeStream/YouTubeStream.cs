﻿using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

using OpenCvSharp;
using BigGustave;
using NAudio.Wave;

using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;
using System.Diagnostics;

namespace YouTubeStream;

class YouTubeStream : GameConsole
{
    IntPtr inHandle;
    delegate void MyDelegate();

    readonly string streamURL = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    VideoCapture videoCapture;
    Process ffmpeg;

    readonly int points;

    public YouTubeStream(string videoURL)
      : base(256, 154, "YouTube", fontwidth: 4, fontheight: 4)
    {
        if (videoURL != "empty")
        {
            streamURL = videoURL.Split('=')[1];
        }
        else
        {
            streamURL = "OWBFKL6H7rI"; //Salad Fingers Ep 1
        }
    }
    public override bool OnUserCreate()
    {
        #region nessecities
     //   ConsoleGameEngine.TextWriter.LoadFont("fontsheet.txt", 6, 8);

        inHandle = GetStdHandle(STD_INPUT_HANDLE);
        uint mode = 0;
        GetConsoleMode(inHandle, ref mode);
        mode &= ~ENABLE_QUICK_EDIT_MODE; //disable
        mode |= ENABLE_WINDOW_INPUT; //enable (if you want)
        mode |= ENABLE_MOUSE_INPUT; //enable
        SetConsoleMode(inHandle, mode);

        //ConsoleListener.MouseEvent += ConsoleListener_MouseEvent;
        //ConsoleListener.Start();
        #endregion

        var youtube = new YoutubeClient();
        var videoId = streamURL;

        // Get stream URL synchronously
        var streamManifest = youtube.Videos.Streams.GetManifestAsync(videoId).GetAwaiter().GetResult();

        var streamInfo = streamManifest.GetMuxedStreams().TryGetWithHighestVideoQuality();
        var videoStream = streamManifest.GetVideoOnlyStreams().OrderByDescending(s => s.VideoQuality.MaxHeight).LastOrDefault();
        var audioStream = streamManifest.GetAudioOnlyStreams().OrderByDescending(s => s.Bitrate).LastOrDefault();

        var audioURL = audioStream.Url;

        if (videoStream == null)
            throw new Exception("No video-only streams found.");

        var streamUrl = videoStream.Url;

        videoCapture = new VideoCapture(streamUrl);

        if (!videoCapture.IsOpened()) return false;

        ffmpeg = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{audioURL}\" -f s16le -acodec pcm_s16le -ar 44100 -ac 2 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        ffmpeg.Start();

        var waveFormat = new WaveFormat(44100, 16, 2); // 44.1kHz, 16-bit, stereo
        var bufferedWaveProvider = new BufferedWaveProvider(waveFormat)
        {
            BufferDuration = TimeSpan.FromSeconds(15),
            DiscardOnBufferOverflow = true,
        };

        var waveOut = new WaveOutEvent();

        waveOut.Init(bufferedWaveProvider);
        waveOut.Play();

        var audioThread = new Thread(() =>
        {
            var buffer = new byte[16384]; // 16 KB chunks
            var stream = ffmpeg.StandardOutput.BaseStream;

            while (true)
            {
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break;

                bufferedWaveProvider.AddSamples(buffer, 0, bytesRead);
            }
        });
        audioThread.Start();

        return true;
    }
    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        var frame = new Mat();
        videoCapture.Read(frame);

        var pngBytes = frame.ToBytes(".png");
        var ms = new MemoryStream(pngBytes);
        var png = Png.Open(ms);

        var outputsprite = new Sprite(png.Width, png.Height);
        for (var x = 0; x < png.Width; x++)
        {
            for (var y = 0; y < png.Height; y++)
            {
                var red = png.GetPixel(x, y).R;
                var green = png.GetPixel(x, y).G;
                var blue = png.GetPixel(x, y).B;

                var col = ClosedConsoleColor3Bit(red, green, blue, out var pixel);

                outputsprite.SetPixel(x, y, pixel, col);
            }
        }

        DrawSprite(0, 0, outputsprite);

        return true;
    }

    private void ConsoleListener_MouseEvent(MOUSE_EVENT_RECORD r)
    {
    }
}
