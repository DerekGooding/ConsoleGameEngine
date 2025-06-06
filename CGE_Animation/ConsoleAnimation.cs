using ConsoleGameEngine;

namespace CGE_Animation;

class ConsoleAnimation : GameConsole
{
    Animation _diddysAnimation;

    public ConsoleAnimation()
      : base(200, 120, "Animation", fontwidth: 4, fontheight: 4)
    { }
    public override bool OnUserCreate()
    {
        _diddysAnimation = new Animation("diddy_idle.txt", new TimeSpan(0, 0, 0, 0, 100), 37, 48);

        return true;
    }

    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        _diddysAnimation.Update();

        DrawSprite(0, 0, _diddysAnimation.outputSprite);

        return true;
    }
}
