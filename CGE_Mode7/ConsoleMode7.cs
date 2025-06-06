using ConsoleGameEngine;

namespace CGE_Mode7;

class ConsoleMode7 : GameConsole
{
    Sprite _sprite;
    private double _fPlayerX = 13.7;          // Player Start Position
    private double _fPlayerY = 5.09;
    private double _fPlayerA = -26.7;

    double _fNear = 0.005f;
    double _fFar = 0.03f;
    double _fFoVHalf = 3.14159f / 4.0f;

    public ConsoleMode7()
      : base(200, 120, "Mode7", fontwidth: 4, fontheight: 4)
    { }
    public override bool OnUserCreate()
    {
        _sprite = new Sprite("mk_track.txt");
        return true;
    }

    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        // Control rendering parameters dynamically
        if (GetKeyState(ConsoleKey.Q).Held) _fNear += 0.1 * (elapsedTime.TotalMilliseconds / 1000);
        if (GetKeyState(ConsoleKey.A).Held) _fNear -= 0.1 * (elapsedTime.TotalMilliseconds / 1000);

        if (GetKeyState(ConsoleKey.W).Held) _fFar += 0.1 * (elapsedTime.TotalMilliseconds / 1000);
        if (GetKeyState(ConsoleKey.S).Held) _fFar -= 0.1 * (elapsedTime.TotalMilliseconds / 1000);

        if (GetKeyState(ConsoleKey.Y).Held) _fFoVHalf += 0.1 * (elapsedTime.TotalMilliseconds / 1000);
        if (GetKeyState(ConsoleKey.X).Held) _fFoVHalf -= 0.1 * (elapsedTime.TotalMilliseconds / 1000);

        if (GetKeyState(ConsoleKey.LeftArrow).Held)
            _fPlayerA -= 1.0f * (elapsedTime.TotalMilliseconds / 1000);

        if (GetKeyState(ConsoleKey.RightArrow).Held)
            _fPlayerA += 1.0f * (elapsedTime.TotalMilliseconds / 1000);

        if (GetKeyState(ConsoleKey.UpArrow).Held)
        {
            _fPlayerX += Math.Sin(_fPlayerA) * 0.01 * elapsedTime.TotalMilliseconds;
            _fPlayerY += Math.Cos(_fPlayerA) * 0.01 * elapsedTime.TotalMilliseconds;
        }
        if (GetKeyState(ConsoleKey.DownArrow).Held)
        {
            _fPlayerX -= Math.Sin(_fPlayerA) * 0.01 * elapsedTime.TotalMilliseconds;
            _fPlayerY -= Math.Cos(_fPlayerA) * 0.01 * elapsedTime.TotalMilliseconds;
        }

        Mode7(_fPlayerY / 128, _fPlayerX / 128, _fPlayerA, _fNear, _fFar, _fFoVHalf, _sprite, false);

        return true;
    }
}
