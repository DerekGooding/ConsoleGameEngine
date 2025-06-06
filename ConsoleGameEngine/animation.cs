using System;
using System.Collections.Generic;

namespace ConsoleGameEngine
{
    public class Animation
    {
        readonly List<Sprite> sprites;
        public Sprite outputSprite;
        readonly TimeSpan frameDelay;
        DateTime lastUpdate;
        int shownFrame;
        readonly int frameWidth, frameHeight;
        private readonly bool animationFromOneFrame = false;
        private readonly int frameCount = 0;

        public Animation(List<Sprite> sprites, TimeSpan frameDelay)
        {
            this.sprites = sprites;
            this.frameDelay = frameDelay;
            shownFrame = 0;
            lastUpdate = DateTime.Now;
        }
        public Animation(Sprite sprite, TimeSpan frameDelay, int frameWidth, int frameHeight, int frameCount)
        {
            animationFromOneFrame = true;
            sprites = new List<Sprite> { sprite };
            this.frameDelay = frameDelay;
            shownFrame = 0;
            lastUpdate = DateTime.Now;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            outputSprite = sprites[0].ReturnPartialSprite(shownFrame * frameWidth, 0, frameWidth, frameHeight);
            this.frameCount = frameCount;
        }

        public Animation(List<string> sprites, TimeSpan frameDelay)
        {
            this.sprites = new List<Sprite>();
            foreach (var sprite in sprites)
            {
                this.sprites.Add(new Sprite(sprite));
            }
            outputSprite = this.sprites[shownFrame];
            this.frameDelay = frameDelay;
            shownFrame = 0;
            lastUpdate = DateTime.Now;
        }

        public Animation(string sprite, TimeSpan frameDelay, int frameWidth, int frameHeight)
        {
            animationFromOneFrame = true;
            sprites = new List<Sprite> { new Sprite(sprite) };
            this.frameDelay = frameDelay;
            shownFrame = 0;
            lastUpdate = DateTime.Now;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            outputSprite = sprites[0].ReturnPartialSprite(shownFrame * frameWidth, frameHeight, frameWidth, frameHeight);
            frameCount = sprites[0].Width / frameWidth;
        }

        public void Update()
        {
            if(frameDelay < DateTime.Now - lastUpdate)
            {
                lastUpdate = DateTime.Now;
                shownFrame++;

                if (!animationFromOneFrame)
                {
                    if (shownFrame >= sprites.Count)
                        shownFrame = 0;
                    outputSprite = sprites[shownFrame];
                }
                else
                {
                    if (shownFrame >= sprites[0].Width / frameWidth || shownFrame >= frameCount)
                        shownFrame = 0;
                    outputSprite = sprites[0].ReturnPartialSprite(shownFrame * frameWidth, 0, frameWidth, frameHeight);
                }
            }
        }
    }
}
