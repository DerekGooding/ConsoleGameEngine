using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.TextBox;

namespace ConsoleGameEngine;

public class TextBlock
{
    public int X, Y;
    public int Width, Height;
    public Sprite OutputSprite;
    public string Content = "";

    readonly int _length; //character-count
    readonly string _tag;
    readonly bool _simple = true; // simple - ascii-charcters, advanced - sprites
    readonly short _foregroundColor, _backgroundColor;
    readonly ObjectPosition _tagPosition;

    public TextBlock(int x, int y, int length, string tag, bool simple = true, ObjectPosition tagPosition = ObjectPosition.Top, short backgroundColor = (short)COLOR.FG_BLACK, short foregroundColor = (short)COLOR.FG_WHITE, string content = "")
    {
        X = x;
        Y = y;
        _length = length;
        Content = content;
        _tag = tag;
        _simple = simple;
        _foregroundColor = foregroundColor;
        _backgroundColor = backgroundColor;
        _tagPosition = tagPosition;

        OutputSprite = BuildSprite();
    }

    private Sprite BuildSprite()
    {
        //input body
        Sprite body;

        Content = Content.PadLeft(_length);

        if (_simple)
        {
            var color = (short)((_backgroundColor << 4) + _foregroundColor);

            switch (_tagPosition)
            {
                case ObjectPosition.Top:

                case ObjectPosition.Bottom:

                case ObjectPosition.Left:

                case ObjectPosition.Right: break;
            }

            body = new Sprite(_length + 2, 4); //length of input + 2 for frame; height for tag, frame and content
            //frame
            for (var i = 1; i < body.Width - 1; i++)
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

            for (var i = 0; i < Content.Length; i++)
                body.SetPixel(i + 1, 2, Content[i], color);

            for (var i = 0; i < _tag.Length; i++)
                body.SetPixel(i, 0, _tag[i], color);
        }
        else
        {
            var contentSprite = TextWriter.GenerateTextSprite(Content, TextWriter.Textalignment.Right, 1, backgroundColor: _foregroundColor, foregroundColor: _backgroundColor);

            var tagSprite = TextWriter.GenerateTextSprite(_tag, TextWriter.Textalignment.Right, 1, backgroundColor: _backgroundColor, foregroundColor: _foregroundColor);

            body = new Sprite(contentSprite.Width > tagSprite.Width ? contentSprite.Width : tagSprite.Width, contentSprite.Height + tagSprite.Height + 3);
            body.AddSpriteToSprite(1, 1, tagSprite);
            body.AddSpriteToSprite(1, tagSprite.Height + 1, contentSprite);

            //frame
            for (var i = 0; i < body.Width; i++)
            {
                body.SetPixel(i, 0, '█', (short)_foregroundColor);
                body.SetPixel(i, tagSprite.Height, '█', (short)_foregroundColor);
                body.SetPixel(i, body.Height - 1, '█', (short)_foregroundColor);
                for (var j = 0; j < body.Height; j++)
                {
                    body.SetPixel(0, j, '█', (short)_foregroundColor);
                    body.SetPixel(body.Width - 1, j, '█', (short)_foregroundColor);
                }
            }
        }
        Width = body.Width; Height = body.Height;
        return body;
    }

    public void SetContent(string content)
    {
        Content = content;
        OutputSprite = BuildSprite();
    }
}
