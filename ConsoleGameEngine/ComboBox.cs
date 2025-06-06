using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine;

public class ComboBox
{
    public int X, Y;
    public List<string> Entries = [];
    public Sprite OutputSprite;

    private readonly bool _simple;
    private bool _lbShown;
    readonly short _foregroundColor, _backgroundColor;
    private readonly int _w;
    private readonly int _h;
    private readonly int _entriesToShow;
    private int _shownEntry;
    readonly string _tag;

    readonly TextBlock _tb_Selection;
    readonly Button _btn_Select;
    readonly ListBox _lb_Entries;

    public ComboBox(int x, int y, int w, int h, string tag, List<string> entries, int entriesToShow = 5, bool simple = true, short backgroundColor = (short)COLOR.FG_BLACK, short foregroundColor = (short)COLOR.FG_WHITE)
    {
        X = x;
        Y = y;
        _tag = tag;
        Entries = entries;
        _backgroundColor = backgroundColor;
        _foregroundColor = foregroundColor;
        _entriesToShow = entriesToShow;
        _simple = simple;
        _w = w;
        _h = h;

        _btn_Select = new Button(x + w - 3, y + 1, "v", method: BtnSelectClicked);
        _tb_Selection = new TextBlock(x, y, w - _btn_Select.Width, tag, tagPosition: TextBox.ObjectPosition.Top, backgroundColor: backgroundColor, foregroundColor: foregroundColor, content: entries[_shownEntry]);
        _lb_Entries = new ListBox(x, y + _tb_Selection.Height, w, entriesToShow + 2, entries, simple: simple, backgroundColor: backgroundColor, foregroundColor: foregroundColor);

        OutputSprite = BuildSprite();
    }

    public void UpdateMouseInput(MOUSE_EVENT_RECORD r)
    {
        _btn_Select.Update(r);

        if (_lbShown)
        {
            _lb_Entries.Update(r);

            if(_lb_Entries.SelectedEntry != _shownEntry)
            {
                _shownEntry = _lb_Entries.SelectedEntry;
                _tb_Selection.SetContent(Entries[_shownEntry]);
                _lbShown = false;
            }
        }
        OutputSprite = BuildSprite();
    }

    private Sprite BuildSprite()
    {
        Sprite retSprite;

        if (_lbShown)
        {
            retSprite = new Sprite(_btn_Select.Width + _tb_Selection.Width, _tb_Selection.Height + _lb_Entries.H);

            retSprite.AddSpriteToSprite(0, 0, _tb_Selection.OutputSprite);
            retSprite.AddSpriteToSprite(_w - 3, 1, _btn_Select.OutputSprite);
            retSprite.AddSpriteToSprite(0, _tb_Selection.Height, _lb_Entries.OutputSprite);
        }
        else
        {
            retSprite = new Sprite(_btn_Select.Width + _tb_Selection.Width, _tb_Selection.Height);

            retSprite.AddSpriteToSprite(0, 0, _tb_Selection.OutputSprite);
            retSprite.AddSpriteToSprite(_w-3, 1, _btn_Select.OutputSprite);
        }

        return retSprite;
    }

    private bool BtnSelectClicked()
    {
        _lbShown = !_lbShown;
        OutputSprite = BuildSprite();
        return true;
    }
}
