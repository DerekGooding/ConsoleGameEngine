﻿using System;
using System.Collections.Generic;

namespace ConsoleGameEngine
{
    public class animation
    {
        List<Sprite> sprites;
        public Sprite outputSprite;
        TimeSpan frameDelay;
        DateTime lastUpdate;
        int shownFrame;
        int frameWidth, frameHeight;
        private bool animationFromOneFrame = false;
        private int frameCount = 0;

        public animation(List<Sprite> sprites, TimeSpan frameDelay)
        {
            this.sprites = sprites;
            this.frameDelay = frameDelay;
            this.shownFrame = 0;
            lastUpdate = DateTime.Now;
        }
        public animation(Sprite sprite, TimeSpan frameDelay, int frameWidth, int frameHeight, int frameCount)
        {
            animationFromOneFrame = true;
            sprites = new List<Sprite> { sprite };
            this.frameDelay = frameDelay;
            this.shownFrame = 0;
            lastUpdate = DateTime.Now;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            outputSprite = sprites[0].ReturnPartialSprite(shownFrame * frameWidth, 0, frameWidth, frameHeight);
            this.frameCount = frameCount;
        }

        public animation(List<string> sprites, TimeSpan frameDelay)
        {
            this.sprites = new List<Sprite>();
            foreach (string sprite in sprites)
            {
                this.sprites.Add(new Sprite(sprite));
            }
            outputSprite = this.sprites[this.shownFrame];
            this.frameDelay = frameDelay;
            this.shownFrame = 0;
            lastUpdate = DateTime.Now;
        }

        public animation(string sprite, TimeSpan frameDelay, int frameWidth, int frameHeight)
        {
            animationFromOneFrame = true;
            sprites = new List<Sprite> { new Sprite(sprite) };
            this.frameDelay = frameDelay;
            this.shownFrame = 0;
            lastUpdate = DateTime.Now;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            outputSprite = sprites[0].ReturnPartialSprite(shownFrame * frameWidth, frameHeight, frameWidth, frameHeight);
            this.frameCount = sprites[0].Width / frameWidth;
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
