using System;
using System.Collections.Generic;

namespace ConsoleGameEngine;

public class GameObject
{
    readonly List<Animation> animations = new List<Animation>();
    int activeAnimationIndex {get; set;}
    public Sprite outputSprite = null;

    public GameObject()
    {
        outputSprite = new Sprite(16,16);
        AddAnimation(new Animation(outputSprite, new TimeSpan(0, 0, 0, 0, 100), 16, 16, 0));
        activeAnimationIndex = 0;
    }
    public GameObject(Sprite spriteSheet, int w, int h, TimeSpan timeSpan, List<int> frameCounts)
    {
        animations = new List<Animation>();
        animations = LoadSpriteSheet(spriteSheet, w, h, timeSpan, frameCounts);
    }

    public void Update()
    {
        animations[activeAnimationIndex].Update();
        outputSprite = animations[activeAnimationIndex].outputSprite;
    }

    public void AddAnimation(Animation animation) => animations.Add(animation);

    private List<Animation> LoadSpriteSheet(Sprite spriteSheet, int w, int h, TimeSpan timeSpan, List<int> frameCounts)
    {
        var animations = new List<Animation>();

        var width = spriteSheet.Width;
        var height = spriteSheet.Height;

        for(var i = 0; i < height / h; i++)
        {
            var newSprite = new Sprite(width, h);

            for(var x = 0; x < width; x++)
            {
                for(var y = i * h; y < (i * h) + h; y++)
                {
                    newSprite.SetPixel(x,y - (i * h), spriteSheet.GetChar(x,y), spriteSheet.GetColor(x,y));
                }
            }

            animations.Add(new Animation(newSprite, timeSpan, w, h, frameCounts[i]));
        }
        return animations;
    }

    public void DecAnimationIndex()
    {
        activeAnimationIndex--;

        if (activeAnimationIndex < 0)
            activeAnimationIndex = animations.Count - 1;
    }
    public void IncAnimationIndex()
    {
        activeAnimationIndex++;

        if(activeAnimationIndex > animations.Count - 1)
            activeAnimationIndex = 0;
    }
}
