using System;
using System.Collections.Generic;

namespace ConsoleGameEngine
{
    public class GameObject
    {
        List<Animation> animations = new List<Animation>();
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
            List<Animation> animations = new List<Animation>();

            int width = spriteSheet.Width;
            int height = spriteSheet.Height;

            for(int i = 0; i < height / h; i++)
            {
                Sprite newSprite = new Sprite(width, h);

                for(int x = 0; x < width; x++)
                {
                    for(int y = i * h; y < i * h + h; y++)
                    {
                        newSprite.SetPixel(x,y - i*h,spriteSheet.GetChar(x,y), spriteSheet.GetColor(x,y));
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
}
