using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;

namespace CGE_UIElements_Simple;

class CGE_UIElements_Simple : GameConsole
{
    //Mouse-Controll
    IntPtr _inHandle;

    //UI-Elements
    TextBlock _yourTextBlock;
    TextBox _yourTextBox;
    Button _yourButton;
    ListBox _yourListBox;
    ComboBox _yourComboBox;

    readonly List<string> _entries = ["Entry 1", "Entry 2", "Entry 3", "Entry 4", "Entry 5", "Entry 6"];

    public CGE_UIElements_Simple()
      : base(80, 50, "Fonts", fontwidth: 10, fontheight: 10)
    { }
    public override bool OnUserCreate()
    {
        _yourTextBlock = new TextBlock(2, 2, 15, "i display text!", content:"Like this one");
        _yourTextBox = new TextBox(2, 6, 15, "i take inputs");
        _yourButton = new Button(20, 2, "Click me", method: OnButtonClick);
        _yourListBox = new ListBox(20, 6, 15, 10, _entries, simple: true);
        _yourComboBox = new ComboBox(40, 2, 15, 20, "I take selections", _entries, entriesToShow: 8);

        //Mouse-Input-Init
        _inHandle = GetStdHandle(STD_INPUT_HANDLE);
        uint mode = 0;
        GetConsoleMode(_inHandle, ref mode);
        mode &= ~ENABLE_QUICK_EDIT_MODE; //disable
        mode |= ENABLE_WINDOW_INPUT; //enable (if you want)
        mode |= ENABLE_MOUSE_INPUT; //enable
        SetConsoleMode(_inHandle, mode);
        ConsoleListener.MouseEvent += ConsoleListener_MouseEvent;
        ConsoleListener.Start();

        return true;
    }
    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        Clear();

        _yourTextBox.UpdateInput(KeyStates, elapsedTime);

        DrawSprite(_yourTextBlock.X, _yourTextBlock.Y, _yourTextBlock.OutputSprite);
        DrawSprite(_yourTextBox.X, _yourTextBox.Y, _yourTextBox.OutputSprite);
        DrawSprite(_yourButton.X, _yourButton.Y, _yourButton.OutputSprite);
        DrawSprite(_yourListBox.X, _yourListBox.Y, _yourListBox.OutputSprite);
        DrawSprite(_yourComboBox.X, _yourComboBox.Y, _yourComboBox.OutputSprite);

        return true;
    }
    private void ConsoleListener_MouseEvent(MOUSE_EVENT_RECORD r)
    {
        _yourTextBox.UpdateSelection(r);
        _yourButton.Update(r);
        _yourListBox.Update(r);
        _yourComboBox.UpdateMouseInput(r);
    }

    private bool OnButtonClick() => true;
}
