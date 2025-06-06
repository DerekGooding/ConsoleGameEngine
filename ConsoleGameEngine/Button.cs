﻿using System;
using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine
{
    public class Button
    {
        public int x, y, width, height;
        public Sprite outputSprite, sprite, feedbackSprite, hooverSprite;
        readonly string text;
        Func<bool> method;
        readonly bool simple = false;
        readonly short foregroundColor, backgroundColor, hooverColor;

        public Button(int x, int y, int width, int height, Sprite sprite, Sprite feedbackSprite = null, Sprite hooverSprite = null, Func<bool> method = null)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.sprite = sprite;
            outputSprite = this.sprite;
            this.feedbackSprite = feedbackSprite;
            this.hooverSprite = hooverSprite;
            this.method = method;
        }
        public Button(int x, int y, Sprite sprite, Sprite feedbackSprite = null, Sprite hooverSprite = null, Func<bool> method = null)
        {
            this.x = x;
            this.y = y;
            this.sprite = sprite;
            width = sprite.Width;
            height = sprite.Height;
            outputSprite = this.sprite;
            this.feedbackSprite = feedbackSprite;
            this.hooverSprite= hooverSprite;
            this.method = method;
        }
        public Button(int x, int y, string text, short backgroundColor = (short)COLOR.FG_BLACK, short foregroundColor = (short)COLOR.FG_WHITE, short hooverColor = (short)COLOR.FG_DARK_GREY, Func<bool> method = null)
        {
            this.x = x;
            this.y = y;
            this.text = text;
            width = text.Length + 2;
            height = 3;
            simple = true;
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
            this.hooverColor = hooverColor;
            this.method = method;

            outputSprite = BuildSimpleSprite(false, false);
        }

        public void OnButtonClicked(Func<bool> method) => this.method = method;

        public void Update(MOUSE_EVENT_RECORD r)
        {
            int mouseX = r.dwMousePosition.X, mouseY = r.dwMousePosition.Y;
            var mouseState = r.dwButtonState;

            if (mouseX <= x + width && mouseX >= x && mouseY <= y + height && mouseY > y)
            {
                if (mouseState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED)
                {
                    if (!simple)
                    {
                        if (feedbackSprite != null)
                            outputSprite = feedbackSprite;
                    }
                    else
                    {
                        outputSprite = BuildSimpleSprite(true, false);
                    }

                    method();
                }
                else
                {
                    if (!simple)
                    {
                        if(hooverSprite != null)
                            outputSprite = hooverSprite;
                    }
                    else
                    {
                        outputSprite = BuildSimpleSprite(false, true);
                    }
                }
            }
            else
            {
                outputSprite = !simple ? sprite : BuildSimpleSprite(false, false);
            }
        }

        private Sprite BuildSimpleSprite(bool clicked, bool hoovered)
        {
            var retSprite = new Sprite(text.Length + 2, 3);

            var color = clicked ? (short)((foregroundColor << 4) + backgroundColor) : (short)((backgroundColor << 4) + foregroundColor);

            color = hoovered ? (short)((hooverColor << 4) + backgroundColor) : color;

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
            for(var i = 0; i < text.Length; i++)
            {
                retSprite.SetPixel(1+i,1, text[i], color);
            }

            return retSprite;
        }
    }
}
