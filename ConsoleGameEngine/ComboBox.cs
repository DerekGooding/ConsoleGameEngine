using System.Collections.Generic;
using static ConsoleGameEngine.GameConsole;
using static ConsoleGameEngine.NativeMethods;

namespace ConsoleGameEngine;

public class ComboBox
{
    public int x, y;
    public List<string> entries = [];
    public Sprite outputSprite = null;

    private readonly bool simple;
    private bool lbShown = false;
    readonly short foregroundColor, backgroundColor;
    private readonly int w;
    private readonly int h;
    private readonly int entriesToShow;
    private int shownEntry = 0;
    readonly string tag;

    readonly TextBlock tb_Selection;
    readonly Button btn_Select;
    readonly ListBox lb_Entries;

    public ComboBox(int x, int y, int w, int h, string tag, List<string> entries, int entriesToShow = 5, bool simple = true, short backgroundColor = (short)COLOR.FG_BLACK, short foregroundColor = (short)COLOR.FG_WHITE)
    {
        this.x = x;
        this.y = y;
        this.tag = tag;
        this.entries = entries;
        this.backgroundColor = backgroundColor;
        this.foregroundColor = foregroundColor;
        this.entriesToShow = entriesToShow;
        this.simple = simple;
        this.w = w;
        this.h = h;

        btn_Select = new Button(x + w - 3, y + 1, "v", method: BtnSelectClicked);
        tb_Selection = new TextBlock(x, y, w - btn_Select.width, tag, tagPosition: TextBox.ObjectPosition.Top, backgroundColor: backgroundColor, foregroundColor: foregroundColor, content: entries[shownEntry]);
        lb_Entries = new ListBox(x, y + tb_Selection.height, w, entriesToShow + 2, entries, simple: simple, backgroundColor: backgroundColor, foregroundColor: foregroundColor);

        outputSprite = BuildSprite();
    }

    public void UpdateMouseInput(MOUSE_EVENT_RECORD r)
    {
        btn_Select.Update(r);

        if (lbShown)
        {
            lb_Entries.Update(r);

            if(lb_Entries.selectedEntry != shownEntry)
            {
                shownEntry = lb_Entries.selectedEntry;
                tb_Selection.SetContent(entries[shownEntry]);
                lbShown = false;
            }
        }
        outputSprite = BuildSprite();
    }

    private Sprite BuildSprite()
    {
        Sprite retSprite;

        if (lbShown)
        {
            retSprite = new Sprite(btn_Select.width + tb_Selection.width, tb_Selection.height + lb_Entries.h);

            retSprite.AddSpriteToSprite(0, 0, tb_Selection.outputSprite);
            retSprite.AddSpriteToSprite(w - 3, 1, btn_Select.outputSprite);
            retSprite.AddSpriteToSprite(0, tb_Selection.height, lb_Entries.outputSprite);
        }
        else
        {
            retSprite = new Sprite(btn_Select.width + tb_Selection.width, tb_Selection.height);

            retSprite.AddSpriteToSprite(0, 0, tb_Selection.outputSprite);
            retSprite.AddSpriteToSprite(w-3, 1, btn_Select.outputSprite);
        }

        return retSprite;
    }

    private bool BtnSelectClicked()
    {
        lbShown = !lbShown;
        outputSprite = BuildSprite();
        return true;
    }
}
