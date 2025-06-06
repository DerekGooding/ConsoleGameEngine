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

        private readonly int nMapWidth = 32;              // World Dimensions
        private readonly int nMapHeight = 32;
        private double fPlayerX = 13.7;          // Player Start Position
        private double fPlayerY = 5.09;
        private double fPlayerA = -26.7;           // Player Start Rotation
        private readonly double fFOV = 3.14159 / 4.0;    // Field of View
        private readonly double fDepth = 16.0;            // Maximum rendering distance
        private readonly double fSpeed = 5.0;             // Walking Speed
        private string map = "";
        private Sprite wall, lamp, fireball;
        private double[] fDepthBuffer;
        private struct SObject
        {
            public double x;
            public double y;
            public double vx;
            public double vy;
            public bool bRemove;
            public Sprite sprite;
        };

        List<SObject> listObjects;
        private Animation coinAnim;
        private readonly Animation diddy;

        public override bool OnUserCreate()
        {
            #region MAP
            map += "#########.......#########.......";
            map += "#...............#...............";
            map += "#.......#########.......########";
            map += "#..............##..............#";
            map += "#......##......##......##......#";
            map += "#......##..............##......#";
            map += "#..............##..............#";
            map += "###............####............#";
            map += "##.............###.............#";
            map += "#............####............###";
            map += "#..............................#";
            map += "#..............##..............#";
            map += "#..............##..............#";
            map += "#...........#####...........####";
            map += "#..............................#";
            map += "###..####....########....#######";
            map += "####.####.......######..........";
            map += "#...............#...............";
            map += "#.......#########.......##..####";
            map += "#..............##..............#";
            map += "#......##......##.......#......#";
            map += "#......##......##......##......#";
            map += "#..............##..............#";
            map += "###............####............#";
            map += "##.............###.............#";
            map += "#............####............###";
            map += "#..............................#";
            map += "#..............................#";
            map += "#..............##..............#";
            map += "#...........##..............####";
            map += "#..............##..............#";
            map += "################################";
            #endregion

            wall = new Sprite("wall2.txt");//("FPSSprites\\fps_wall1.spr");
            lamp = new Sprite("Imp.txt");
            fireball = new Sprite("FireBall.txt");
            fDepthBuffer = new double[Width];

            coinAnim = new Animation(["Coin1.txt", "Coin2.txt", "Coin3.txt", "Coin4.txt"], new TimeSpan(0, 0, 0, 0, 500));
            listObjects =
            [
                new SObject() { x = 8.5f, y = 8.5f, vx = 0.0f, vy = 0.0f, bRemove = false, sprite = lamp },
            new SObject() { x = 7.5f, y = 7.5f, vx = 0.0f, vy = 0.0f, bRemove = false, sprite = lamp },
            new SObject() { x = 10.5f, y = 3.5f, vx = 0.0f, vy = 0.0f, bRemove = false, sprite = lamp },
            new SObject() { x = 9.5f, y = 2.5f, vx = 0.0f, vy = 0.0f, bRemove = false, sprite = coinAnim.outputSprite },
        ];

            return true;
        }

        public override bool OnUserUpdate(TimeSpan fElapsedTime)
        {
            #region inputs
            if (GetKeyState(ConsoleKey.A).Held)// Keys[((int)'A')].held)
                fPlayerA -= fSpeed * 0.5f * fElapsedTime.TotalSeconds;

            if (GetKeyState(ConsoleKey.D).Held)//(Keys[((int)'D')].held)
                fPlayerA += fSpeed * 0.5f * fElapsedTime.TotalSeconds;

            if (GetKeyState(ConsoleKey.W).Held) //(Keys[((int)'W')].held)
            {
                fPlayerX += Math.Sin(fPlayerA) * fSpeed * (fElapsedTime.TotalMilliseconds / 1000);
                fPlayerY += Math.Cos(fPlayerA) * fSpeed * (fElapsedTime.TotalMilliseconds / 1000);

                if (map[(int)(fPlayerX * nMapWidth) + (int)fPlayerY] == '@')
                {
                    fPlayerX -= Math.Sin(fPlayerA) * fSpeed * (fElapsedTime.TotalMilliseconds / 1000);
                    fPlayerY -= Math.Cos(fPlayerA) * fSpeed * (fElapsedTime.TotalMilliseconds / 1000);
                }
            }

            if (GetKeyState(ConsoleKey.S).Held) //(Keys[((int)'S')].held)
            {
                fPlayerX -= Math.Sin(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                fPlayerY -= Math.Cos(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                if (map[(int)(fPlayerX * nMapWidth) + (int)fPlayerY] == '@')
                {
                    fPlayerX += Math.Sin(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                    fPlayerY += Math.Cos(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                }
            }

            if (GetKeyState(ConsoleKey.E).Held) //(Keys[((int)'E')].held)
            {
                fPlayerX += Math.Cos(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                fPlayerY -= Math.Sin(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                if (map[(int)(fPlayerX * nMapWidth) + (int)fPlayerY] == '@')
                {
                    fPlayerX -= Math.Cos(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                    fPlayerY += Math.Sin(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                }
            }

            if (GetKeyState(ConsoleKey.Q).Held) //(Keys[((int)'Q')].held)
            {
                fPlayerX -= Math.Cos(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                fPlayerY += Math.Sin(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                if (map[(int)(fPlayerX * nMapWidth) + (int)fPlayerY] == '@')
                {
                    fPlayerX += Math.Cos(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                    fPlayerY -= Math.Sin(fPlayerA) * fSpeed * fElapsedTime.TotalSeconds;
                }
            }

            if (GetKeyState(ConsoleKey.Spacebar).Released)//(Keys[32].released)
            {
                SObject o;
                o.x = fPlayerX;
                o.y = fPlayerY;
                var fNoise = (new Random().Next(RAND_MAX) - 0.5f) * 0.1f;
                o.vx = Math.Sin(fPlayerA + fNoise) * 8.0f;
                o.vy = Math.Cos(fPlayerA + fNoise) * 8.0f;
                o.sprite = fireball;
                o.bRemove = false;
                listObjects.Add(o);
            }
            #endregion

            #region Main-View
            for (var x = 0; x < Width; x++)
            {
                // For each column, calculate the projected ray angle into world space
                var fRayAngle = fPlayerA - (fFOV / 2.0f) + (x / (float)Width * fFOV);

                // Find distance to wall
                double fStepSize = 0.01f;      // Increment size for ray casting, decrease to increase	
                double fDistanceToWall = 0.0f; //                                      resolution

                var bHitWall = false;      // Set when ray hits wall block
                var bBoundary = false;     // Set when ray hits boundary between two wall blocks

                var fEyeX = Math.Sin(fRayAngle); // Unit vector for ray in player space
                var fEyeY = Math.Cos(fRayAngle);

                double fSampleX = 0.0f;

                var bLit = false;

                while (!bHitWall && fDistanceToWall < fDepth)
                {
                    fDistanceToWall += fStepSize;
                    var nTestX = (int)(fPlayerX + (fEyeX * fDistanceToWall));
                    var nTestY = (int)(fPlayerY + (fEyeY * fDistanceToWall));

                    // Test if ray is out of bounds
                    if (nTestX < 0 || nTestX >= nMapWidth || nTestY < 0 || nTestY >= nMapHeight)
                    {
                        bHitWall = true;            // Just set distance to maximum depth
                        fDistanceToWall = fDepth;
                    }
                    else
                    {
                        // Ray is inbounds so test to see if the ray cell is a wall block
                        if (map[(nTestX * nMapWidth) + nTestY] == '#')
                        {
                            // Ray has hit wall
                            bHitWall = true;

                            // Determine where ray has hit wall. Break Block boundary
                            // int 4 line segments
                            var fBlockMidX = (double)nTestX + 0.5f;
                            var fBlockMidY = (double)nTestY + 0.5f;

                            var fTestPointX = fPlayerX + (fEyeX * fDistanceToWall);
                            var fTestPointY = fPlayerY + (fEyeY * fDistanceToWall);

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
                fDepthBuffer[x] = fDistanceToWall;

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
                        if (fDistanceToWall < fDepth)
                        {
                            var fSampleY = (y - (double)nCeiling) / (nFloor - (double)nCeiling);
                            SetChar(x, y, (char)(PIXELS)(int)wall.SampleGlyph(fSampleX, fSampleY), wall.SampleColor(fSampleX, fSampleY));
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

            coinAnim.Update();
            var coin = listObjects[3];
            coin.sprite = coinAnim.outputSprite;
            listObjects[3] = coin;

            // Update & Draw Objects	
            for (var i = 0; i < listObjects.Count; i++)
            {
                var obj = listObjects[i];
                // Update Object Physics
                obj.x += obj.vx * fElapsedTime.TotalSeconds;
                obj.y += obj.vy * fElapsedTime.TotalSeconds;

                // Check if object is inside wall - set flag for removal
                if (map[((int)obj.x * nMapWidth) + (int)obj.y] == '#')
                    obj.bRemove = true;

                // Can object be seen?
                var fVecX = obj.x - fPlayerX;
                var fVecY = obj.y - fPlayerY;
                var fDistanceFromPlayer = Math.Sqrt((fVecX * fVecX) + (fVecY * fVecY));

                var fEyeX = Math.Sin(fPlayerA);
                var fEyeY = Math.Cos(fPlayerA);

                // Calculate angle between lamp and players feet, and players looking direction
                // to determine if the lamp is in the players field of view
                var fObjectAngle = Math.Atan2(fEyeY, fEyeX) - Math.Atan2(fVecY, fVecX);
                if (fObjectAngle < -3.14159f)
                    fObjectAngle += 2.0f * 3.14159f;
                if (fObjectAngle > 3.14159f)
                    fObjectAngle -= 2.0f * 3.14159f;

                var bInPlayerFOV = Math.Abs(fObjectAngle) < fFOV / 2.0f;

                if (bInPlayerFOV && fDistanceFromPlayer >= 0.5f && fDistanceFromPlayer < fDepth && !obj.bRemove)
                {
                    var fObjectCeiling = (Height / 2.0) - ((double)Height / (float)fDistanceFromPlayer);
                    var fObjectFloor = Height - fObjectCeiling;
                    var fObjectHeight = fObjectFloor - fObjectCeiling;
                    var fObjectAspectRatio = obj.sprite.Height / (double)obj.sprite.Width;
                    var fObjectWidth = fObjectHeight / fObjectAspectRatio;
                    var fMiddleOfObject = ((0.5f * (fObjectAngle / (fFOV / 2.0f))) + 0.5f) * Width;

                    if (obj.sprite == lamp || true)
                    {
                        // Draw Lamp
                        for (double lx = 0; lx < fObjectWidth; lx++)
                        {
                            for (double ly = 0; ly < fObjectHeight; ly++)
                            {
                                var fSampleX = lx / fObjectWidth;
                                var fSampleY = ly / fObjectHeight;
                                var c = obj.sprite.SampleGlyph(fSampleX, fSampleY);
                                var nObjectColumn = (int)(fMiddleOfObject + lx - (fObjectWidth / 2.0f));
                                if (nObjectColumn >= 0 && nObjectColumn < Width)
                                {
                                    if (c != ' ' && fDepthBuffer[nObjectColumn] >= fDistanceFromPlayer)
                                    {
                                        SetChar(nObjectColumn, (int)(fObjectCeiling + ly), c, obj.sprite.SampleColor(fSampleX, fSampleY));
                                        fDepthBuffer[nObjectColumn] = fDistanceFromPlayer;
                                    }
                                }
                            }
                        }
                    }
                }

                listObjects[i] = obj;
            }

            // Remove dead objects from object list
            listObjects.RemoveAll(s => s.bRemove);
            #endregion

            #region GUI
            // Display Map & Player
            for (var nx = 0; nx < nMapWidth; nx++)
            {
                for (var ny = 0; ny < nMapWidth; ny++)
                    SetChar(nx + 1, ny + 1, map[(ny * nMapWidth) + nx]);
            }

            SetChar(1 + (int)fPlayerY, 1 + (int)fPlayerX, 'P', (short)COLOR.BG_RED);
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