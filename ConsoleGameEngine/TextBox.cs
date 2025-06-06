using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine;

public class TextBox(int x,
                     int y,
                     int length,
                     string tag,
                     bool simple = true,
                     TextBox.ObjectPosition tagPosition = TextBox.ObjectPosition.Top,
                     short backgroundColor = (short)COLOR.FG_BLACK,
                     short foregroundColor = (short)COLOR.FG_WHITE,
                     string content = "")
{
    public int X = x, Y = y;
    public Sprite OutputSprite = new(1, 1);
    public bool Selected;
    public string Content = content;

    readonly string _tag = tag;
    readonly int _length = length; //character-count
    readonly bool _simple = simple; // simple - ascii-charcters, advanced - sprites
    readonly short _foregroundColor = foregroundColor, _backgroundColor = backgroundColor;
    readonly ObjectPosition _tagPosition = tagPosition;

    int _inputFieldWidth, _inputFieldHeight;

    TimeSpan _buttonDelay;
    readonly TimeSpan _buttonTime = new(0, 0, 0, 0, 120);

    public void UpdateSelection(MOUSE_EVENT_RECORD r)
    {
        int mouseX = r.dwMousePosition.X, mouseY = r.dwMousePosition.Y;
        var mouseState = r.dwButtonState;

        if (mouseState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED)
            Selected = mouseX <= X + _inputFieldWidth && mouseX >= X && mouseY <= Y + _inputFieldHeight && mouseY > Y;
    }
    public void UpdateInput(KeyState[] KeyStates, TimeSpan elapsedTime)
    {
        _buttonDelay += elapsedTime;

        //check for keyboard inputs if selected
        if(Selected)
        {
            if (Content.Length < _length)
            {
                //A-Z
                for (var i = 65; i <= 90; i++)
                {
                    if (GetKeyState((ConsoleKey)i).Held && _buttonDelay >= _buttonTime)
                    {
                        Content += Console.CapsLock ? (char)i : (char)(i + 32);
                        _buttonDelay = new TimeSpan();
                    }
                }

                //0 - 9 - ignores capslock
                for (var i = 48; i <= 57; i++)
                {
                    if (GetKeyState((ConsoleKey)i).Held && _buttonDelay >= _buttonTime)
                    {
                        Content += (char)i;
                        _buttonDelay = new TimeSpan();
                    }
                }

                //seperators (,.;:-)
                if (KeyStates[108].Held && _buttonDelay >= _buttonTime)
                {
                    Content += Console.CapsLock ? ':' : '.';
                    _buttonDelay = new TimeSpan();
                }
                if (KeyStates[109].Held && _buttonDelay >= _buttonTime)
                {
                    Content += Console.CapsLock ? '_' : '-';
                    _buttonDelay = new TimeSpan();
                }
                if (KeyStates[110].Held && _buttonDelay >= _buttonTime)
                {
                    Content += Console.CapsLock ? ';' : ',';
                    _buttonDelay = new TimeSpan();
                }

                if (KeyStates[32].Held && _buttonDelay >= _buttonTime) //space
                {
                    Content += ' ';
                    _buttonDelay = new TimeSpan();
                }
            }

            //(back-)space / enter
            if (KeyStates[13].Held && _buttonDelay >= _buttonTime) //enter
                Selected = false;

            if (KeyStates[8].Held && _buttonDelay >= _buttonTime) //backspace
            {
                Content = Content.Length > 0 ? Content[..^1] : Content;
                _buttonDelay = new TimeSpan();
            }
        }

        //build sprite
        BuildSprite();
    }

    private void BuildSprite()
    {
        //input body
        Sprite body;

        Content = Content.PadLeft(_length);

        if(_simple)
        {
            var color = Selected ? (short)((_foregroundColor << 4) + _backgroundColor) : (short)((_backgroundColor << 4) + _foregroundColor);

            switch(_tagPosition)
            {
                case ObjectPosition.Top:

                case ObjectPosition.Bottom:

                case ObjectPosition.Left:

                case ObjectPosition.Right: break;
            }

            body = new Sprite(_length + 2, 4); //length of input + 2 for frame; height for tag, frame and content
            //frame
            for(var i = 1; i < body.Width - 1; i++)
            {
                body.SetPixel(i, 1, (char)PIXELS.LINE_STRAIGHT_HORIZONTAL, color);
                body.SetPixel(i, body.Height - 1, (char)PIXELS.LINE_STRAIGHT_HORIZONTAL, color);
                for (var j = 1; j < body.Height - 1; j++)
                {
                    body.SetPixel(0, j, (char)PIXELS.LINE_STRAIGHT_VERTICAL, color);
                    body.SetPixel(body.Width - 1, j, (char)PIXELS.LINE_STRAIGHT_VERTICAL, color);
                }
            }
            //corners
            body.SetPixel(0, 1, (char)PIXELS.LINE_CORNER_TOP_LEFT, color);
            body.SetPixel(0, body.Height - 1, (char)PIXELS.LINE_CORNER_BOTTOM_LEFT, color);
            body.SetPixel(body.Width - 1, 1, (char)PIXELS.LINE_CORNER_TOP_RIGHT, color);
            body.SetPixel(body.Width - 1, body.Height, (char)PIXELS.LINE_CORNER_BOTTOM_RIGHT, color);

            for(var i = 0; i < Content.Length; i++)
                body.SetPixel(i+1,2, Content[i], color);

            for (var i = 0; i < _tag.Length; i++)
                body.SetPixel(i, 0, _tag[i], color);
        }
        else
        {
            var contentSprite = !Selected
                ? TextWriter.GenerateTextSprite(Content, TextWriter.Textalignment.Right, 1, backgroundColor:_backgroundColor, foregroundColor:_foregroundColor)
                : TextWriter.GenerateTextSprite(Content, TextWriter.Textalignment.Right, 1, backgroundColor: _foregroundColor, foregroundColor: _backgroundColor);
            var tagSprite = TextWriter.GenerateTextSprite(_tag, TextWriter.Textalignment.Right, 1, backgroundColor: _backgroundColor, foregroundColor: _foregroundColor);

            body = new Sprite(contentSprite.Width > tagSprite.Width ? contentSprite.Width : tagSprite.Width, contentSprite.Height + tagSprite.Height + 3);
            body.AddSpriteToSprite(1, 1, tagSprite);
            body.AddSpriteToSprite(1, tagSprite.Height + 1, contentSprite);

            //frame
            for(var i = 0; i < body.Width; i++)
            {
                body.SetPixel(i, 0, '█', Selected ? (short)COLOR.FG_RED : (short)_foregroundColor);
                body.SetPixel(i, tagSprite.Height, '█', Selected ? (short)COLOR.FG_RED : (short)_foregroundColor);
                body.SetPixel(i, body.Height - 1, '█', Selected ? (short)COLOR.FG_RED : (short)_foregroundColor);
                for (var j = 0; j < body.Height; j++)
                {
                    body.SetPixel(0, j, '█', Selected ? (short)COLOR.FG_RED : (short)_foregroundColor);
                    body.SetPixel(body.Width - 1, j, '█', Selected ? (short)COLOR.FG_RED : (short)_foregroundColor);
                }
            }
        }

        _inputFieldWidth = body.Width;
        _inputFieldHeight = body.Height;
        OutputSprite = body;
    }

    public enum ObjectPosition
    {
        Top, Bottom, Left, Right,
    }
}
