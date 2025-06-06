using ConsoleGameEngine;
using static ConsoleGameEngine.GameConsole;

namespace JumpAndRun;

class Player()
{
    public double XPosition = 100.0, YPosition = 50.0, XVelocity, YVelocity;
    public Sprite OutputSprite = new(8, 8);

    private Sprite? _spriteSheet;
    private Animation? _walkingAnimation;
    private bool _airjumpused;
    private readonly double _velocityMax = 30;
    private const double _walkSpeed = 2, _runSpeed = 5, _fallSpeed = 10,  _acceleration = 0.5, _gravity_acceleration = 2.0;
    private double _playerSpeedX, _playerSpeedY;
    private int _sign;

    public void LoadAnimation(string file)
    {
        _walkingAnimation = new Animation("running ninja.txt", new TimeSpan(0, 0, 0, 0, 100), 16, 16);
        _spriteSheet = new Sprite("running ninja.txt");
    }

    public void Update(KeyState[] KeyStates, TimeSpan elapsedTime, GameConsole gameConsole)
    {
        _walkingAnimation?.Update();
        BuildSprite();

        #region reset
        if (GetKeyState(ConsoleKey.R).Pressed)
        {
            XPosition = 100.0;
            YPosition = 50.0;
            _playerSpeedX = 0.0;
            _playerSpeedY = 0.0;
        }

        #endregion

        #region horizontal movement
        if (GetKeyState(ConsoleKey.A).Held)
        {
            _playerSpeedX -= _acceleration;
            _playerSpeedX = ClampF(_playerSpeedX, -_acceleration, _acceleration);
            _sign = -1;
        }
        else if(GetKeyState(ConsoleKey.D).Held)
        {
            _playerSpeedX += _acceleration;
            _playerSpeedX = ClampF(_playerSpeedX, -_acceleration, _acceleration);
            _sign = 1;
        }
        else if(!GetKeyState(ConsoleKey.A).Held && !GetKeyState(ConsoleKey.D).Held)
        {
            _playerSpeedX -= _playerSpeedX / 2;
            _playerSpeedX = ClampF(_playerSpeedX, -_acceleration, _acceleration);
            _sign = 0;
        }

        XPosition += _playerSpeedX;

        if(XPosition < 0) XPosition = 0;
        if (XPosition > 300) XPosition = 300;
        #endregion

        #region vertical movement
        #region gravity
        //get bottom left koordinate of player-rect
        var bottomleft_x = (int)XPosition;
        var bottomright_x = (int)XPosition + OutputSprite.Width;
        var bottom_y = (int)YPosition + OutputSprite.Height + 1;

        if (gameConsole.GetColor(bottomleft_x, bottom_y) != (short)COLOR.BG_DARK_GREEN && gameConsole.GetColor(bottomright_x, bottom_y) != (short)COLOR.BG_DARK_GREEN)
        {
            _playerSpeedY += _gravity_acceleration;
            _playerSpeedY = ClampF(_playerSpeedY, -_acceleration, _acceleration);
        }
        else
        {
            _playerSpeedY = 0.0;
            _airjumpused = false;
        }
        #endregion

        if (GetKeyState(ConsoleKey.Spacebar).Pressed)
        {
            if (gameConsole.GetColor(bottomleft_x, bottom_y) == (short)COLOR.BG_DARK_GREEN || gameConsole.GetColor(bottomright_x, bottom_y) == (short)COLOR.BG_DARK_GREEN)
            {
                _playerSpeedY = -40;
            }
            else if(!_airjumpused)
            {
                _airjumpused = true;
                _playerSpeedY = -40;
            }
        }

        YPosition += _playerSpeedY;
        #endregion
    }

    public void BuildSprite()
    {
        if(_spriteSheet == null || _walkingAnimation == null) return;
        if (_sign == 0)
        {
            OutputSprite = _spriteSheet.ReturnPartialSprite(0,0,16,16);
        }
        else if(_sign == 1)
        {
            OutputSprite = _walkingAnimation.outputSprite;
        }
        else if(_sign == -1)
        {
            OutputSprite = _walkingAnimation.outputSprite.FlipHorizontally();
        }
    }
}
