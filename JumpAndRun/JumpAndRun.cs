using System;

using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;

namespace JumpAndRun;

class JumpAndRun : GameConsole
{
    Player player;
    Level level;
    TimeSpan keyInputDelay = new TimeSpan(), keyInputTime = new TimeSpan(0, 0, 0, 0, 120);
    IntPtr inHandle;
    int cursorX = 0, cursorY = 0;
    bool leftMousebuttonClicked = false, mouseWheelClicked = false, rightMousebuttonClicked = false;

    bool startLevel = false;
    int points = 0;
    int lastHeight = 0;

    public JumpAndRun()
      : base(200, 120, "Fonts", fontwidth: 4, fontheight: 4)
    { }
    public override bool OnUserCreate()
    {
        inHandle = GetStdHandle(STD_INPUT_HANDLE);
        uint mode = 0;
        GetConsoleMode(inHandle, ref mode);
        mode &= ~ENABLE_QUICK_EDIT_MODE; //disable
        mode |= ENABLE_WINDOW_INPUT; //enable (if you want)
        mode |= ENABLE_MOUSE_INPUT; //enable
        SetConsoleMode(inHandle, mode);

        ConsoleListener.MouseEvent += ConsoleListener_MouseEvent;

        ConsoleListener.Start();

        TextWriter.LoadFont("fontsheet.txt", 6, 8);

        player = new Player();
        level = new Level();
        player.LoadAnimation("runnin ninja.txt");
        //Load sprites, setup variables and whatever
        return true;
    }
    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        keyInputDelay += elapsedTime;
        player.Update(KeyStates, elapsedTime, this);

        if (startLevel)
        {
            level.Update(elapsedTime);
        }

        Clear();
        DrawSprite((int)player.xPosition, (int)player.yPosition, player.outputSprite);
        DrawSprite(0, 0, TextWriter.GenerateTextSprite($"   NINJA TOWER   {level.points} ", TextWriter.Textalignment.Left, 1));

        //draw plattforms
        foreach (Level.Plattform p in level.plattforms)
        {
            DrawSprite(p.x, p.y, new Sprite(p.l, 1, COLOR.BG_DARK_GREEN));
        }

        //draw walls
        //foreach (Level.Plattform p in level.walls)
        //{
        //    DrawSprite(p.x, p.y, new Sprite(1, p.l, GameConsole.COLOR.BG_DARK_GREEN));
        //}

        if(player.yPosition < 50) startLevel = true;
        if (player.yPosition > 120) startLevel = false;

        return true;
    }

    private void ConsoleListener_MouseEvent(MOUSE_EVENT_RECORD r)
    {
        cursorX = r.dwMousePosition.X;
        cursorY = r.dwMousePosition.Y;

        leftMousebuttonClicked = r.dwButtonState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED;
        mouseWheelClicked = r.dwButtonState == MOUSE_EVENT_RECORD.FROM_LEFT_2ND_BUTTON_PRESSED;
        rightMousebuttonClicked = r.dwButtonState == MOUSE_EVENT_RECORD.RIGHTMOST_BUTTON_PRESSED;
    }
}
