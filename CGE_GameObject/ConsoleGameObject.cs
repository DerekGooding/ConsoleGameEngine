using ConsoleGameEngine;

namespace CGE_GameObject;

class ConsoleGameObject : GameConsole
{
    GameObject _playerCharacter;
    Sprite _spriteSheet;

    public ConsoleGameObject()
      : base(200, 120, "GameObject", fontwidth: 4, fontheight: 4)
    { }
    public override bool OnUserCreate()
    {
        _spriteSheet = new Sprite("animationsheet.txt");
        _playerCharacter = new GameObject(_spriteSheet, 32, 32, new TimeSpan(0, 0, 0, 0, 100), [13, 8, 10, 10, 10, 6, 4, 7]);
        return true;
    }

    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        if (GetKeyState(ConsoleKey.A).Released)
        {
            _playerCharacter.DecAnimationIndex();
        }
        if (GetKeyState(ConsoleKey.D).Released)
        {
            _playerCharacter.IncAnimationIndex();
        }

        _playerCharacter.Update();

        Clear();
        DrawSprite(100, 58, _playerCharacter.outputSprite, '\0', 0x0000);
        return true;
    }
}
