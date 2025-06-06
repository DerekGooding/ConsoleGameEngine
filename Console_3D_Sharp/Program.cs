using System.Text;
using ConsoleGameEngine;

namespace Console_3D_Sharp;

static class Program
{
    const int RAND_MAX = 2;

    class ConsoleFPS : GameConsole
    {
        public ConsoleFPS()
            : base(200, 120, "Shooter", fontwidth: 4, fontheight: 4)
        { }

        private readonly int _nMapWidth = 32;              // World Dimensions
        private readonly int _nMapHeight = 32;
        private double _fPlayerX = 13.7;          // Player Start Position
        private double _fPlayerY = 5.09;
        private double _fPlayerA = -26.7;           // Player Start Rotation
        private readonly double _fFOV = 3.14159 / 4.0;    // Field of View
        private readonly double _fDepth = 16.0;            // Maximum rendering distance
        private readonly double _fSpeed = 5.0;             // Walking Speed
        private string _map = "";
        private Sprite _wall, _lamp, _fireball;
        private double[] _fDepthBuffer;
        private record struct SObject (double X, double Y, double VX, double VY, bool BRemove, Sprite Sprite);

        List<SObject> _listObjects;
        private Animation _coinAnim;
        private readonly Animation _diddy;

        public override bool OnUserCreate()
        {
            #region MAP
            _map += "#########.......#########.......";
            _map += "#...............#...............";
            _map += "#.......#########.......########";
            _map += "#..............##..............#";
            _map += "#......##......##......##......#";
            _map += "#......##..............##......#";
            _map += "#..............##..............#";
            _map += "###............####............#";
            _map += "##.............###.............#";
            _map += "#............####............###";
            _map += "#..............................#";
            _map += "#..............##..............#";
            _map += "#..............##..............#";
            _map += "#...........#####...........####";
            _map += "#..............................#";
            _map += "###..####....########....#######";
            _map += "####.####.......######..........";
            _map += "#...............#...............";
            _map += "#.......#########.......##..####";
            _map += "#..............##..............#";
            _map += "#......##......##.......#......#";
            _map += "#......##......##......##......#";
            _map += "#..............##..............#";
            _map += "###............####............#";
            _map += "##.............###.............#";
            _map += "#............####............###";
            _map += "#..............................#";
            _map += "#..............................#";
            _map += "#..............##..............#";
            _map += "#...........##..............####";
            _map += "#..............##..............#";
            _map += "################################";
            #endregion

            _wall = new Sprite("wall2.txt");//("FPSSprites\\fps_wall1.spr");
            _lamp = new Sprite("Imp.txt");
            _fireball = new Sprite("FireBall.txt");
            _fDepthBuffer = new double[Width];

            _coinAnim = new Animation(["Coin1.txt", "Coin2.txt", "Coin3.txt", "Coin4.txt"], new TimeSpan(0, 0, 0, 0, 500));
            _listObjects =
            [
            new SObject(8.5f, 8.5f, 0.0f, 0.0f, false, _lamp ),
            new SObject(7.5f, 7.5f, 0.0f, 0.0f, false, _lamp),
            new SObject(10.5f, 3.5f, 0.0f, 0.0f, false, _lamp),
            new SObject(9.5f, 2.5f, 0.0f, 0.0f, false, _coinAnim.outputSprite),
        ];

            return true;
        }

        public override bool OnUserUpdate(TimeSpan fElapsedTime)
        {
            #region inputs
            if (GetKeyState(ConsoleKey.A).Held)// Keys[((int)'A')].held)
                _fPlayerA -= _fSpeed * 0.5f * fElapsedTime.TotalSeconds;

            if (GetKeyState(ConsoleKey.D).Held)//(Keys[((int)'D')].held)
                _fPlayerA += _fSpeed * 0.5f * fElapsedTime.TotalSeconds;

            if (GetKeyState(ConsoleKey.W).Held) //(Keys[((int)'W')].held)
            {
                _fPlayerX += Math.Sin(_fPlayerA) * _fSpeed * (fElapsedTime.TotalMilliseconds / 1000);
                _fPlayerY += Math.Cos(_fPlayerA) * _fSpeed * (fElapsedTime.TotalMilliseconds / 1000);

                if (_map[(int)(_fPlayerX * _nMapWidth) + (int)_fPlayerY] == '@')
                {
                    _fPlayerX -= Math.Sin(_fPlayerA) * _fSpeed * (fElapsedTime.TotalMilliseconds / 1000);
                    _fPlayerY -= Math.Cos(_fPlayerA) * _fSpeed * (fElapsedTime.TotalMilliseconds / 1000);
                }
            }

            if (GetKeyState(ConsoleKey.S).Held) //(Keys[((int)'S')].held)
            {
                _fPlayerX -= Math.Sin(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                _fPlayerY -= Math.Cos(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                if (_map[(int)(_fPlayerX * _nMapWidth) + (int)_fPlayerY] == '@')
                {
                    _fPlayerX += Math.Sin(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                    _fPlayerY += Math.Cos(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                }
            }

            if (GetKeyState(ConsoleKey.E).Held) //(Keys[((int)'E')].held)
            {
                _fPlayerX += Math.Cos(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                _fPlayerY -= Math.Sin(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                if (_map[(int)(_fPlayerX * _nMapWidth) + (int)_fPlayerY] == '@')
                {
                    _fPlayerX -= Math.Cos(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                    _fPlayerY += Math.Sin(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                }
            }

            if (GetKeyState(ConsoleKey.Q).Held) //(Keys[((int)'Q')].held)
            {
                _fPlayerX -= Math.Cos(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                _fPlayerY += Math.Sin(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                if (_map[(int)(_fPlayerX * _nMapWidth) + (int)_fPlayerY] == '@')
                {
                    _fPlayerX += Math.Cos(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                    _fPlayerY -= Math.Sin(_fPlayerA) * _fSpeed * fElapsedTime.TotalSeconds;
                }
            }

            if (GetKeyState(ConsoleKey.Spacebar).Released)//(Keys[32].released)
            {
                var fNoise = (new Random().Next(RAND_MAX) - 0.5f) * 0.1f;
                var o = new SObject(_fPlayerX,
                                    _fPlayerY,
                                    Math.Sin(_fPlayerA + fNoise) * 8.0f,
                                    Math.Cos(_fPlayerA + fNoise) * 8.0f,
                                    false,
                                    _fireball);
                _listObjects.Add(o);
            }
            #endregion

            #region Main-View
            for (var x = 0; x < Width; x++)
            {
                // For each column, calculate the projected ray angle into world space
                var fRayAngle = _fPlayerA - (_fFOV / 2.0f) + (x / (float)Width * _fFOV);

                // Find distance to wall
                double fStepSize = 0.01f;      // Increment size for ray casting, decrease to increase	
                double fDistanceToWall = 0.0f; //                                      resolution

                var bHitWall = false;      // Set when ray hits wall block
                var bBoundary = false;     // Set when ray hits boundary between two wall blocks

                var fEyeX = Math.Sin(fRayAngle); // Unit vector for ray in player space
                var fEyeY = Math.Cos(fRayAngle);

                double fSampleX = 0.0f;

                var bLit = false;

                while (!bHitWall && fDistanceToWall < _fDepth)
                {
                    fDistanceToWall += fStepSize;
                    var nTestX = (int)(_fPlayerX + (fEyeX * fDistanceToWall));
                    var nTestY = (int)(_fPlayerY + (fEyeY * fDistanceToWall));

                    // Test if ray is out of bounds
                    if (nTestX < 0 || nTestX >= _nMapWidth || nTestY < 0 || nTestY >= _nMapHeight)
                    {
                        bHitWall = true;            // Just set distance to maximum depth
                        fDistanceToWall = _fDepth;
                    }
                    else
                    {
                        // Ray is inbounds so test to see if the ray cell is a wall block
                        if (_map[(nTestX * _nMapWidth) + nTestY] == '#')
                        {
                            // Ray has hit wall
                            bHitWall = true;

                            // Determine where ray has hit wall. Break Block boundary
                            // int 4 line segments
                            var fBlockMidX = (double)nTestX + 0.5f;
                            var fBlockMidY = (double)nTestY + 0.5f;

                            var fTestPointX = _fPlayerX + (fEyeX * fDistanceToWall);
                            var fTestPointY = _fPlayerY + (fEyeY * fDistanceToWall);

                            var fTestAngle = Math.Atan2(fTestPointY - fBlockMidY, fTestPointX - fBlockMidX);

                            if (fTestAngle is >= (double)(-3.14159f * 0.25f) and < (double)(3.14159f * 0.25f))
                                fSampleX = fTestPointY - nTestY;
                            if (fTestAngle is >= (double)(3.14159f * 0.25f) and < (double)(3.14159f * 0.75f))
                                fSampleX = fTestPointX - nTestX;
                            if (fTestAngle is < (double)(-3.14159f * 0.25f) and >= (double)(-3.14159f * 0.75f))
                                fSampleX = fTestPointX - nTestX;
                            if (fTestAngle is >= (double)(3.14159f * 0.75f) or < (double)(-3.14159f * 0.75f))
                                fSampleX = fTestPointY - nTestY;
                        }
                    }
                }

                // Calculate distance to ceiling and floor
                var nCeiling = (int)((double)(Height / 2.0) - (Height / fDistanceToWall)); //(int)((Height / 2.0 - Height) / (fDistanceToWall));
                var nFloor = Height - nCeiling;

                // Update Depth Buffer
                _fDepthBuffer[x] = fDistanceToWall;

                for (var y = 0; y < Height; y++)
                {
                    // Each Row
                    if (y <= nCeiling)
                    {
                        SetChar(x, y, ' ', (short)COLOR.BG_BLACK);
                    }
                    else if (y > nCeiling && y <= nFloor)
                    {
                        // Draw Wall
                        if (fDistanceToWall < _fDepth)
                        {
                            var fSampleY = (y - (double)nCeiling) / (nFloor - (double)nCeiling);
                            SetChar(x, y, (char)(PIXELS)(int)_wall.SampleGlyph(fSampleX, fSampleY), _wall.SampleColor(fSampleX, fSampleY));
                        }
                        else
                        {
                            SetChar(x, y, (char)PIXELS.PIXEL_SOLID, 0);
                        }
                    }
                    else // Floor
                    {
                        //Commented for mode7
                        SetChar(x, y, (char)PIXELS.PIXEL_HALF, (short)COLOR.FG_DARK_GREEN); //,(char)GameConsole.PIXELS.PIXEL_SOLID //encs.GetString(new byte[1] { 176 })[0]
                    }
                }
            }
            #endregion

            #region Game-Objects

            _coinAnim.Update();
            var coin = _listObjects[3];
            coin.Sprite = _coinAnim.outputSprite;
            _listObjects[3] = coin;

            // Update & Draw Objects	
            for (var i = 0; i < _listObjects.Count; i++)
            {
                var obj = _listObjects[i];
                // Update Object Physics
                obj.X += obj.VX * fElapsedTime.TotalSeconds;
                obj.Y += obj.VY * fElapsedTime.TotalSeconds;

                // Check if object is inside wall - set flag for removal
                if (_map[((int)obj.X * _nMapWidth) + (int)obj.Y] == '#')
                    obj.BRemove = true;

                // Can object be seen?
                var fVecX = obj.X - _fPlayerX;
                var fVecY = obj.Y - _fPlayerY;
                var fDistanceFromPlayer = Math.Sqrt((fVecX * fVecX) + (fVecY * fVecY));

                var fEyeX = Math.Sin(_fPlayerA);
                var fEyeY = Math.Cos(_fPlayerA);

                // Calculate angle between lamp and players feet, and players looking direction
                // to determine if the lamp is in the players field of view
                var fObjectAngle = Math.Atan2(fEyeY, fEyeX) - Math.Atan2(fVecY, fVecX);
                if (fObjectAngle < -3.14159f)
                    fObjectAngle += 2.0f * 3.14159f;
                if (fObjectAngle > 3.14159f)
                    fObjectAngle -= 2.0f * 3.14159f;

                var bInPlayerFOV = Math.Abs(fObjectAngle) < _fFOV / 2.0f;

                if (bInPlayerFOV && fDistanceFromPlayer >= 0.5f && fDistanceFromPlayer < _fDepth && !obj.BRemove)
                {
                    var fObjectCeiling = (Height / 2.0) - ((double)Height / (float)fDistanceFromPlayer);
                    var fObjectFloor = Height - fObjectCeiling;
                    var fObjectHeight = fObjectFloor - fObjectCeiling;
                    var fObjectAspectRatio = obj.Sprite.Height / (double)obj.Sprite.Width;
                    var fObjectWidth = fObjectHeight / fObjectAspectRatio;
                    var fMiddleOfObject = ((0.5f * (fObjectAngle / (_fFOV / 2.0f))) + 0.5f) * Width;

                    if (obj.Sprite == _lamp || true)
                    {
                        // Draw Lamp
                        for (double lx = 0; lx < fObjectWidth; lx++)
                        {
                            for (double ly = 0; ly < fObjectHeight; ly++)
                            {
                                var fSampleX = lx / fObjectWidth;
                                var fSampleY = ly / fObjectHeight;
                                var c = obj.Sprite.SampleGlyph(fSampleX, fSampleY);
                                var nObjectColumn = (int)(fMiddleOfObject + lx - (fObjectWidth / 2.0f));
                                if (nObjectColumn >= 0 && nObjectColumn < Width)
                                {
                                    if (c != ' ' && _fDepthBuffer[nObjectColumn] >= fDistanceFromPlayer)
                                    {
                                        SetChar(nObjectColumn, (int)(fObjectCeiling + ly), c, obj.Sprite.SampleColor(fSampleX, fSampleY));
                                        _fDepthBuffer[nObjectColumn] = fDistanceFromPlayer;
                                    }
                                }
                            }
                        }
                    }
                }

                _listObjects[i] = obj;
            }

            // Remove dead objects from object list
            _listObjects.RemoveAll(s => s.BRemove);
            #endregion

            #region GUI
            // Display Map & Player
            for (var nx = 0; nx < _nMapWidth; nx++)
            {
                for (var ny = 0; ny < _nMapWidth; ny++)
                    SetChar(nx + 1, ny + 1, _map[(ny * _nMapWidth) + nx]);
            }

            SetChar(1 + (int)_fPlayerY, 1 + (int)_fPlayerX, 'P', (short)COLOR.BG_RED);
            #endregion

            return true;
        }
    }

    [STAThread]
    static void Main()
    {
        Console.OutputEncoding = Encoding.GetEncoding(437);

        using var f = new ConsoleFPS();
        f.Start();
    }

    static float GetElapsedTime()
    {
        var elapsed = DateTime.Now - lastTime;
        lastTime = DateTime.Now;
        return (float)elapsed.TotalSeconds;
    }

    static DateTime lastTime = DateTime.Now;
}