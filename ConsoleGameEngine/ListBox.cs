using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine;

public class ListBox
{
    public int X, Y, W, H;
    public List<string> Entries = [];
    public Sprite OutputSprite = new(1, 1);
    public int SelectedEntry;

    readonly bool _simple;
    readonly short _foregroundColor, _backgroundColor;
    int _firstEntry;
    readonly Button _btn_MoveUP, _btn_MoveDOWN;

    public ListBox(int x, int y, int w, int h, List<string> entries, bool simple = false, short backgroundColor = (short)COLOR.FG_BLACK, short foregroundColor = (short)COLOR.FG_WHITE)
    {
        X = x;
        Y = y;
        W = w;
        H = h;
        Entries = entries;
        _simple = simple;
        _backgroundColor = backgroundColor;
        _foregroundColor = foregroundColor;

        _btn_MoveDOWN = new Button(x+ w - 3, y + h - 4, "v", _backgroundColor, _foregroundColor);
        _btn_MoveUP = new Button(x +w - 3, y + 1, "^", _backgroundColor, _foregroundColor);

        _btn_MoveDOWN.OnButtonClicked(Btn_MoveDOWNClicked);
        _btn_MoveUP.OnButtonClicked(Btn_MoveUPClicked);

        OutputSprite = BuildSprite();
    }

    public void Update(MOUSE_EVENT_RECORD r)
    {
        int mouseX = r.dwMousePosition.X, mouseY = r.dwMousePosition.Y;
        var mouseState = r.dwButtonState;

        if (mouseState == MOUSE_EVENT_RECORD.FROM_LEFT_1ST_BUTTON_PRESSED)
        {
            if(mouseX >= X && mouseY >= Y && mouseX <= X + W - 4 && mouseY <= Y + H - 2)
            {
                SelectedEntry = mouseY - Y - 1 + _firstEntry;
            }
        }

        _btn_MoveDOWN.Update(r);
        _btn_MoveUP.Update(r);

        OutputSprite = BuildSprite();
    }

    private Sprite BuildSprite()
    {
        var retSprite = new Sprite(W, H);

        var color = (short)((_foregroundColor << 4) + _backgroundColor);

        if (_simple)
        {
            //frame
            for (var i = 1; i < retSprite.Width - 1; i++)
            {
                retSprite.SetPixel(i, 0, (char)PIXELS.LINE_STRAIGHT_HORIZONTAL, color); //top
                retSprite.SetPixel(i, retSprite.Height - 1, (char)PIXELS.LINE_STRAIGHT_HORIZONTAL, color); //bottom
                for (var j = 1; j < retSprite.Height - 1; j++)
                {
                    retSprite.SetPixel(0, j, (char)PIXELS.LINE_STRAIGHT_VERTICAL, color); //left
                    if (Entries.Count > retSprite.Height - 2)
                        retSprite.SetPixel(retSprite.Width - 3, j, (char)PIXELS.LINE_STRAIGHT_VERTICAL, color); //border between entries and scrollbar
                    retSprite.SetPixel(retSprite.Width - 1, j, (char)PIXELS.LINE_STRAIGHT_VERTICAL, color); //right
                }
            }
            retSprite.SetPixel(0, 0, (char)PIXELS.LINE_CORNER_TOP_LEFT, color);
            retSprite.SetPixel(0, retSprite.Height - 1, (char)PIXELS.LINE_CORNER_BOTTOM_LEFT, color);
            retSprite.SetPixel(retSprite.Width , 0, (char)PIXELS.LINE_CORNER_TOP_RIGHT, color);
            retSprite.SetPixel(retSprite.Width - 1, retSprite.Height, (char)PIXELS.LINE_CORNER_BOTTOM_RIGHT, color);

            //scrollbar
            if (Entries.Count > retSprite.Height - 2)
            {
                retSprite.AddSpriteToSprite(W - 3, 1, _btn_MoveUP.OutputSprite);
                retSprite.AddSpriteToSprite(W - 3, H - 4, _btn_MoveDOWN.OutputSprite);

                var scrollbarHeight = H - 8;

                var scrollbarPosition = (int)((double)scrollbarHeight * ((double)_firstEntry / (((double)Entries.Count) - ((double)H - 2.0))));

                retSprite.SetPixel(W - 2, scrollbarPosition + 4, '█', (short)COLOR.FG_DARK_GREY);
            }

            var entryCount = 0;
            //entrys
            for(var i = _firstEntry; i < Entries.Count && (i - _firstEntry) < H - 2; i++)
            {
                color = i == SelectedEntry ? (short)((_foregroundColor << 4) + _backgroundColor) : (short)((_backgroundColor << 4) + _foregroundColor);

                for (var j = 0; j < Entries[i].Length && j < W - 4; j++)
                {
                    retSprite.SetPixel(j + 1, entryCount + 1, Entries[i][j], color);
                }

                entryCount++;
            }
        }
        else
        {
            //frame

            //scrollbar

            //entrys
        }

        return retSprite;
    }

    private bool Btn_MoveUPClicked()
    {
        _firstEntry--;
        if(_firstEntry < 0)
            _firstEntry = 0;

        OutputSprite = BuildSprite();
        return true;
    }
    private bool Btn_MoveDOWNClicked()
    {
        _firstEntry++;
        if( _firstEntry > Entries.Count - (H - 2) )
            _firstEntry = Entries.Count - (H - 2);

        OutputSprite = BuildSprite();
        return true;
    }
}
