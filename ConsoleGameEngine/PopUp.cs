using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine;

public class PopUp
{
    public int X, Y;
    public bool Visible;
    readonly Button _oKButton, _cancleButton;
    readonly string _text;
    readonly GameConsole.COLOR _backgroundColor, _foregroundColor;
    PopUpState _state;

    public PopUp(int x, int y, string text, out Sprite outputSprite, GameConsole.COLOR backgroundColor = GameConsole.COLOR.FG_BLUE, GameConsole.COLOR foregroundColor = GameConsole.COLOR.FG_WHITE)
    {
        X = x;
        Y = y;
        _text = text;
        _backgroundColor = backgroundColor;
        _foregroundColor = foregroundColor;
        _state = PopUpState.none;

        var textSprite = TextWriter.GenerateTextSprite(_text, TextWriter.Textalignment.Center, 1, (short)_backgroundColor, (short)_foregroundColor);
        _oKButton = new Button(0,0,TextWriter.GenerateTextSprite("  OK  ", TextWriter.Textalignment.Left, 1), feedbackSprite: TextWriter.GenerateTextSprite("  OK  ", TextWriter.Textalignment.Left, 1, backgroundColor: 0, foregroundColor: 15));
        _cancleButton = new Button(0, 0, TextWriter.GenerateTextSprite("CANCLE", TextWriter.Textalignment.Left, 1), feedbackSprite: TextWriter.GenerateTextSprite("CANCLE", TextWriter.Textalignment.Left, 1, backgroundColor: 0, foregroundColor: 15));

        _oKButton.OnButtonClicked(OKButtonClicked);
        _cancleButton.OnButtonClicked(CancleButtonClicked);

        outputSprite = textSprite.Width > _oKButton.Width + _cancleButton.Width
            ? new Sprite(textSprite.Width + 4, textSprite.Height + 6 + _oKButton.Height, GameConsole.COLOR.BG_BLUE)
            : new Sprite(_oKButton.Width + _cancleButton.Width + 4, _oKButton.Width + _cancleButton.Width + 4, GameConsole.COLOR.BG_BLUE);

        outputSprite.AddSpriteToSprite(2, 2, textSprite);
        outputSprite.AddSpriteToSprite(2, 2 +textSprite.Height + 2, _oKButton.OutputSprite);
        outputSprite.AddSpriteToSprite(outputSprite.Width - 2 - _cancleButton.Width, 2 + textSprite.Height + 2, _cancleButton.OutputSprite);

        _oKButton.X = x + 2;
        _oKButton.Y = y + 4 + textSprite.Height;

        _cancleButton.X = x + outputSprite.Width - _cancleButton.Width;
        _cancleButton.Y = y + 4 + textSprite.Height;
    }

    public PopUpState Update(MOUSE_EVENT_RECORD r)
    {
        if (Visible)
        {
            _state = PopUpState.none;
            _oKButton.Update(r);
            _cancleButton.Update(r);
        }
        else
        { _state = PopUpState.none; }

        return _state;
    }

    private bool OKButtonClicked()
    {
        _state = PopUpState.okClicked;
        return true;
    }
    private bool CancleButtonClicked()
    {
        _state = PopUpState.cancleClicked;
        return true;
    }
}
