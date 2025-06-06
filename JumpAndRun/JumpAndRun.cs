using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;

namespace JumpAndRun;

class JumpAndRun : GameConsole
{
    Player _player;
    Level _level;
    private TimeSpan _keyInputDelay;
    private readonly TimeSpan _keyInputTime = new(0, 0, 0, 0, 120);
    IntPtr _inHandle;
    int _cursorX, _cursorY;
    bool _leftMousebuttonClicked, _mouseWheelClicked, _rightMousebuttonClicked;

    bool _startLevel;
    readonly int _points;
    readonly int _lastHeight;

    public JumpAndRun()
      : base(200, 120, "Fonts", fontwidth: 4, fontheight: 4)
    { }
    public override bool OnUserCreate()
    {
        _inHandle = GetStdHandle(STD_INPUT_HANDLE);
        uint mode = 0;
        GetConsoleMode(_inHandle, ref mode);
        mode &= ~ENABLE_QUICK_EDIT_MODE; //disable
        mode |= ENABLE_WINDOW_INPUT; //enable (if you want)
        mode |= ENABLE_MOUSE_INPUT; //enable
        SetConsoleMode(_inHandle, mode);

        ConsoleListener.MouseEvent += ConsoleListener_MouseEvent;

        ConsoleListener.Start();

        TextWriter.LoadFont("fontsheet.txt", 6, 8);

        _player = new Player();
        _level = new Level();
        _player.LoadAnimation("runnin ninja.txt");
        //Load sprites, setup variables and whatever
        return true;
    }
    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        _keyInputDelay += elapsedTime;
        _player.Update(KeyStates, elapsedTime, this);

        if (_startLevel)
        {
            _level.Update(elapsedTime);
        }

        Clear();
        DrawSprite((int)_player.xPosition, (int)_player.yPosition, _player.outputSprite);
        DrawSprite(0, 0, TextWriter.GenerateTextSprite($"   NINJA TOWER   {_level.points} ", TextWriter.Textalignment.Left, 1));

        //draw plattforms
        foreach (var p in _level.plattforms)
        {
            DrawSprite(p.X, p.Y, new Sprite(p.L, 1, COLOR.BG_DARK_GREEN));
        }

        //draw walls
        //foreach (Level.Plattform p in level.walls)
        //{
        //    DrawSprite(p.x, p.y, new Sprite(1, p.l, GameConsole.COLOR.BG_DARK_GREEN));
        //}

        if(_player.yPosition < 50) _startLevel = true;
        if (_player.yPosition > 120) _startLevel = false;

        return true;
    }

    private void ConsoleListener_MouseEvent(MOUSE_EVENT_RECORD r)
    {
        _cursorX = r.dwMousePosition.X;
        _cursorY = r.dwMousePosition.Y;

        _leftMousebuttonClicked = r.dwButtonState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED;
        _mouseWheelClicked = r.dwButtonState == MOUSE_EVENT_RECORD.FROM_LEFT_2ND_BUTTON_PRESSED;
        _rightMousebuttonClicked = r.dwButtonState == MOUSE_EVENT_RECORD.RIGHTMOST_BUTTON_PRESSED;
    }
}
