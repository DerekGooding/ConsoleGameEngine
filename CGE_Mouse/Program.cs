using System.Text;
using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;

namespace CGE_Mouse;

internal static class Program
{
    class CGE_Mouse : GameConsole
    {
        IntPtr _inHandle;

        int _cursorX, _cursorY;
        bool _leftMousebuttonClicked, _mouseWheelClicked, _rightMousebuttonClicked;

        public CGE_Mouse()
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

            TextWriter.LoadFont("fontsheet.txt", 7, 9);

            return true;
        }

        public override bool OnUserUpdate(TimeSpan elapsedTime)
        {
            //Clear();

            DrawSprite(0, 0, TextWriter.GenerateTextSprite($"X: {_cursorX}; Y: {_cursorY}", TextWriter.Textalignment.Left, 1));

            if(_leftMousebuttonClicked)
            {
                SetChar(_cursorX, _cursorY, 'X');
            }

            if(_mouseWheelClicked || _rightMousebuttonClicked)
            {
                Clear();
            }

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

    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new CGE_Mouse();
        f.Start();
    }
}
