using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine;

public class Button
{
    public int X, Y, Width, Height;
    public Sprite OutputSprite, Sprite, FeedbackSprite, HooverSprite;
    readonly string _text;
    Func<bool> _method;
    readonly bool _simple;
    readonly short _foregroundColor, _backgroundColor, _hooverColor;

    public Button(int x, int y, int width, int height, Sprite sprite, Sprite feedbackSprite = null, Sprite hooverSprite = null, Func<bool> method = null)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Sprite = sprite;
        OutputSprite = Sprite;
        FeedbackSprite = feedbackSprite;
        HooverSprite = hooverSprite;
        _method = method;
    }
    public Button(int x, int y, Sprite sprite, Sprite feedbackSprite = null, Sprite hooverSprite = null, Func<bool> method = null)
    {
        X = x;
        Y = y;
        Sprite = sprite;
        Width = sprite.Width;
        Height = sprite.Height;
        OutputSprite = Sprite;
        FeedbackSprite = feedbackSprite;
        HooverSprite= hooverSprite;
        _method = method;
    }
    public Button(int x, int y, string text, short backgroundColor = (short)COLOR.FG_BLACK, short foregroundColor = (short)COLOR.FG_WHITE, short hooverColor = (short)COLOR.FG_DARK_GREY, Func<bool> method = null)
    {
        X = x;
        Y = y;
        _text = text;
        Width = text.Length + 2;
        Height = 3;
        _simple = true;
        _foregroundColor = foregroundColor;
        _backgroundColor = backgroundColor;
        _hooverColor = hooverColor;
        _method = method;

        OutputSprite = BuildSimpleSprite(false, false);
    }

    public void OnButtonClicked(Func<bool> method) => _method = method;

    public void Update(MOUSE_EVENT_RECORD r)
    {
        int mouseX = r.dwMousePosition.X, mouseY = r.dwMousePosition.Y;
        var mouseState = r.dwButtonState;

        if (mouseX <= X + Width && mouseX >= X && mouseY <= Y + Height && mouseY > Y)
        {
            if (mouseState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED)
            {
                if (!_simple)
                {
                    if (FeedbackSprite != null)
                        OutputSprite = FeedbackSprite;
                }
                else
                {
                    OutputSprite = BuildSimpleSprite(true, false);
                }

                _method();
            }
            else
            {
                if (!_simple)
                {
                    if(HooverSprite != null)
                        OutputSprite = HooverSprite;
                }
                else
                {
                    OutputSprite = BuildSimpleSprite(false, true);
                }
            }
        }
        else
        {
            OutputSprite = !_simple ? Sprite : BuildSimpleSprite(false, false);
        }
    }

    private Sprite BuildSimpleSprite(bool clicked, bool hoovered)
    {
        var retSprite = new Sprite(_text.Length + 2, 3);

        var color = clicked ? (short)((_foregroundColor << 4) + _backgroundColor) : (short)((_backgroundColor << 4) + _foregroundColor);

        color = hoovered ? (short)((_hooverColor << 4) + _backgroundColor) : color;

        for (var i = 1; i < retSprite.Width - 1; i++)
        {
            retSprite.SetPixel(i, 0, (char)PIXELS.LINE_STRAIGHT_HORIZONTAL, color);
            retSprite.SetPixel(i, retSprite.Height - 1, (char)PIXELS.LINE_STRAIGHT_HORIZONTAL, color);
            for (var j = 1; j < retSprite.Height - 1; j++)
            {
                retSprite.SetPixel(0, j, (char)PIXELS.LINE_STRAIGHT_VERTICAL, color);
                retSprite.SetPixel(retSprite.Width - 1, j, (char)PIXELS.LINE_STRAIGHT_VERTICAL, color);
            }
        }
        //corners
        retSprite.SetPixel(0, 0, (char)PIXELS.LINE_CORNER_TOP_LEFT, color);
        retSprite.SetPixel(0, retSprite.Height - 1, (char)PIXELS.LINE_CORNER_BOTTOM_LEFT, color);
        retSprite.SetPixel(retSprite.Width - 1, 0, (char)PIXELS.LINE_CORNER_TOP_RIGHT, color);
        retSprite.SetPixel(retSprite.Width - 1, retSprite.Height - 1, (char)PIXELS.LINE_CORNER_BOTTOM_RIGHT, color);

        //text
        for(var i = 0; i < _text.Length; i++)
        {
            retSprite.SetPixel(1+i,1, _text[i], color);
        }

        return retSprite;
    }
}
