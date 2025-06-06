namespace ConsoleGameEngine;

public class Animation
{
    readonly List<Sprite> _sprites;
    public Sprite outputSprite;
    readonly TimeSpan _frameDelay;
    DateTime _lastUpdate;
    int _shownFrame;
    readonly int _frameWidth, _frameHeight;
    private readonly bool _animationFromOneFrame;
    private readonly int _frameCount;

    public Animation(List<Sprite> sprites, TimeSpan frameDelay)
    {
        _sprites = sprites;
        _frameDelay = frameDelay;
        _shownFrame = 0;
        _lastUpdate = DateTime.Now;
    }
    public Animation(Sprite sprite, TimeSpan frameDelay, int frameWidth, int frameHeight, int frameCount)
    {
        _animationFromOneFrame = true;
        _sprites = [sprite];
        _frameDelay = frameDelay;
        _shownFrame = 0;
        _lastUpdate = DateTime.Now;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        outputSprite = _sprites[0].ReturnPartialSprite(_shownFrame * frameWidth, 0, frameWidth, frameHeight);
        _frameCount = frameCount;
    }

    public Animation(List<string> sprites, TimeSpan frameDelay)
    {
        _sprites = [];
        foreach (var sprite in sprites)
        {
            _sprites.Add(new Sprite(sprite));
        }
        outputSprite = _sprites[_shownFrame];
        _frameDelay = frameDelay;
        _shownFrame = 0;
        _lastUpdate = DateTime.Now;
    }

    public Animation(string sprite, TimeSpan frameDelay, int frameWidth, int frameHeight)
    {
        _animationFromOneFrame = true;
        _sprites = [new(sprite)];
        _frameDelay = frameDelay;
        _shownFrame = 0;
        _lastUpdate = DateTime.Now;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        outputSprite = _sprites[0].ReturnPartialSprite(_shownFrame * frameWidth, frameHeight, frameWidth, frameHeight);
        _frameCount = _sprites[0].Width / frameWidth;
    }

    public void Update()
    {
        if(_frameDelay < DateTime.Now - _lastUpdate)
        {
            _lastUpdate = DateTime.Now;
            _shownFrame++;

            if (!_animationFromOneFrame)
            {
                if (_shownFrame >= _sprites.Count)
                    _shownFrame = 0;
                outputSprite = _sprites[_shownFrame];
            }
            else
            {
                if (_shownFrame >= _sprites[0].Width / _frameWidth || _shownFrame >= _frameCount)
                    _shownFrame = 0;
                outputSprite = _sprites[0].ReturnPartialSprite(_shownFrame * _frameWidth, 0, _frameWidth, _frameHeight);
            }
        }
    }
}
