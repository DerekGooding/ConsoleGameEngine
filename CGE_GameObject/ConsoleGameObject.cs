﻿using ConsoleGameEngine;

namespace CGE_GameObject;

class ConsoleGameObject : GameConsole
{
    GameObject playerCharacter;
    Sprite spriteSheet;

    public ConsoleGameObject()
      : base(200, 120, "GameObject", fontwidth: 4, fontheight: 4)
    { }
    public override bool OnUserCreate()
    {
        spriteSheet = new Sprite("animationsheet.txt");
        playerCharacter = new GameObject(spriteSheet, 32, 32, new TimeSpan(0, 0, 0, 0, 100), [13, 8, 10, 10, 10, 6, 4, 7]);
        return true;
    }

    public override bool OnUserUpdate(TimeSpan elapsedTime)
    {
        if (GetKeyState(ConsoleKey.A).Released)
        {
            playerCharacter.DecAnimationIndex();
        }
        if (GetKeyState(ConsoleKey.D).Released)
        {
            playerCharacter.IncAnimationIndex();
        }

        playerCharacter.Update();

        Clear();
        DrawSprite(100, 58, playerCharacter.outputSprite, '\0', 0x0000);
        return true;
    }
}
