using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;

namespace CGE_PopUp;

class CGE_PopUp : GameConsole
{
    IntPtr _inHandle;

    Button _button;
    PopUp _popUp;
    Sprite _popUpSprite;
    PopUpState _popUpState;

    public CGE_PopUp()
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

        ConsoleListener.Start();

        TextWriter.LoadFont("fontsheet.txt", 7, 9);

        _button = new Button(0, 0, TextWriter.GenerateTextSprite("open PopUp", TextWriter.Textalignment.Left, 1));
        _button.OnButtonClicked(ButtonClicked);

        _popUp = new PopUp(40, 40, "Are you sure?", out _popUpSprite);

        ConsoleListener.MouseEvent += ConsoleListener_MouseEvent;

        return true;
    }
    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        Clear();

        DrawSprite(0, 0, _button.OutputSprite);

        if (_popUp.Visible)
            DrawSprite(40, 40, _popUpSprite);

        if(_popUpState != PopUpState.none)
        {
            _popUp.Visible = false;
        }

        return true;
    }

    private void ConsoleListener_MouseEvent(MOUSE_EVENT_RECORD r)
    {
        _popUpState = _popUp.Update(r);
        _button.Update(r);
    }

    private bool ButtonClicked()
    {
        _popUp.Visible = true;
        return true;
    }
}
