using System;
using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine;

public class TextBox
{
    public int x, y;
    public Sprite outputSprite;
    readonly int length; //character-count

    public string content = "";
    readonly string tag;
    public bool selected = false;
    readonly bool simple = true; // simple - ascii-charcters, advanced - sprites
    readonly short foregroundColor, backgroundColor;
    readonly ObjectPosition tagPosition;

    int inputFieldWidth, inputFieldHeight;

    TimeSpan buttonDelay = new TimeSpan();
    readonly TimeSpan buttonTime = new TimeSpan(0, 0, 0, 0, 120);

    public TextBox(int x, int y, int length, string tag, bool simple = true, ObjectPosition tagPosition = ObjectPosition.Top, short backgroundColor = (short)COLOR.FG_BLACK, short foregroundColor = (short)COLOR.FG_WHITE, string content = "")
    {
        this.x = x;
        this.y = y;
        this.length = length;
        this.tag = tag;
        this.simple = simple;
        outputSprite = new Sprite(1,1);
        this.foregroundColor = foregroundColor;
        this.backgroundColor = backgroundColor;
        this.tagPosition = tagPosition;
        this.content = content;
    }

    public void UpdateSelection(MOUSE_EVENT_RECORD r)
    {
        int mouseX = r.dwMousePosition.X, mouseY = r.dwMousePosition.Y;
        var mouseState = r.dwButtonState;

        if (mouseState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED)
            selected = mouseX <= x + inputFieldWidth && mouseX >= x && mouseY <= y + inputFieldHeight && mouseY > y;
    }
    public void UpdateInput(KeyState[] KeyStates, TimeSpan elapsedTime)
    {
        buttonDelay += elapsedTime;

        //check for keyboard inputs if selected
        if(selected)
        {
            if (content.Length < length)
            {
                //A-Z
                for (var i = 65; i <= 90; i++)
                {
                    if (GetKeyState((ConsoleKey)i).Held && buttonDelay >= buttonTime)
                    {
                        content += Console.CapsLock ? (char)i : (char)(i + 32);
                        buttonDelay = new TimeSpan();
                    }
                }

                //0 - 9 - ignores capslock
                for (var i = 48; i <= 57; i++)
                {
                    if (GetKeyState((ConsoleKey)i).Held && buttonDelay >= buttonTime)
                    {
                        content += (char)i;
                        buttonDelay = new TimeSpan();
                    }
                }

                //seperators (,.;:-)
                if (KeyStates[108].Held && buttonDelay >= buttonTime)
                {
                    content += Console.CapsLock ? ':' : '.';
                    buttonDelay = new TimeSpan();
                }
                if (KeyStates[109].Held && buttonDelay >= buttonTime)
                {
                    content += Console.CapsLock ? '_' : '-';
                    buttonDelay = new TimeSpan();
                }
                if (KeyStates[110].Held && buttonDelay >= buttonTime)
                {
                    content += Console.CapsLock ? ';' : ',';
                    buttonDelay = new TimeSpan();
                }

                if (KeyStates[32].Held && buttonDelay >= buttonTime) //space
                {
                    content += ' ';
                    buttonDelay = new TimeSpan();
                }
            }

            //(back-)space / enter
            if (KeyStates[13].Held && buttonDelay >= buttonTime) //enter
                selected = false;

            if (KeyStates[8].Held && buttonDelay >= buttonTime) //backspace
            {
                content = content.Length > 0 ? content[..^1] : content;
                buttonDelay = new TimeSpan();
            }
        }

        //build sprite
        BuildSprite();
    }

    private void BuildSprite()
    {
        //input body
        Sprite body;

        content.PadLeft(length);

        if(simple)
        {
            var color = selected ? (short)((foregroundColor << 4) + backgroundColor) : (short)((backgroundColor << 4) + foregroundColor);

            switch(tagPosition)
            {
                case ObjectPosition.Top:

                case ObjectPosition.Bottom:

                case ObjectPosition.Left:

                case ObjectPosition.Right: break;
            }

            body = new Sprite(length + 2, 4); //length of input + 2 for frame; height for tag, frame and content
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

            for(var i = 0; i < content.Length; i++)
                body.SetPixel(i+1,2, content[i], color);

            for (var i = 0; i < tag.Length; i++)
                body.SetPixel(i, 0, tag[i], color);
        }
        else
        {
            var contentSprite = !selected
                ? TextWriter.GenerateTextSprite(content, TextWriter.Textalignment.Right, 1, backgroundColor:backgroundColor, foregroundColor:foregroundColor)
                : TextWriter.GenerateTextSprite(content, TextWriter.Textalignment.Right, 1, backgroundColor: foregroundColor, foregroundColor: backgroundColor);
            var tagSprite = TextWriter.GenerateTextSprite(tag, TextWriter.Textalignment.Right, 1, backgroundColor: backgroundColor, foregroundColor: foregroundColor);

            body = new Sprite(contentSprite.Width > tagSprite.Width ? contentSprite.Width : tagSprite.Width, contentSprite.Height + tagSprite.Height + 3);
            body.AddSpriteToSprite(1, 1, tagSprite);
            body.AddSpriteToSprite(1, tagSprite.Height + 1, contentSprite);

            //frame
            for(var i = 0; i < body.Width; i++)
            {
                body.SetPixel(i, 0, '█', selected ? (short)COLOR.FG_RED : (short)foregroundColor);
                body.SetPixel(i, tagSprite.Height, '█', selected ? (short)COLOR.FG_RED : (short)foregroundColor);
                body.SetPixel(i, body.Height - 1, '█', selected ? (short)COLOR.FG_RED : (short)foregroundColor);
                for (var j = 0; j < body.Height; j++)
                {
                    body.SetPixel(0, j, '█', selected ? (short)COLOR.FG_RED : (short)foregroundColor);
                    body.SetPixel(body.Width - 1, j, '█', selected ? (short)COLOR.FG_RED : (short)foregroundColor);
                }
            }
        }

        inputFieldWidth = body.Width;
        inputFieldHeight = body.Height;
        outputSprite = body;
    }

    public enum ObjectPosition
    {
        Top, Bottom, Left, Right,
    }
}
