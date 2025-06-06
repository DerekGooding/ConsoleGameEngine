using System;
using System.Collections.Generic;

namespace JumpAndRun;

class Level
{
    public List<Plattform> plattforms;
    public List<Plattform> walls;
    TimeSpan _elapsedTime = new TimeSpan();
    TimeSpan updateDelay = new TimeSpan(0, 0, 0, 0, 40);
    const int MAXplattformcount = 7;
    Random random = new Random();
    Rect boundaries = new Rect(0,9,200,111);
    public int points = 0;

    public Level()
    {
        plattforms = new List<Plattform>();
        walls = new List<Plattform>();

        //ground-plattform
        plattforms.Add(new Plattform { x = 0, y = 120, l = 200 });
        //intro plattforms
        plattforms.Add(new Plattform { x = 50, y = 70, l = 70 });
        plattforms.Add(new Plattform { x = 90, y = 100, l = 70 });
        plattforms.Add(new Plattform { x = 150, y = 40, l = 35 });
        plattforms.Add(new Plattform { x = 130, y = 55, l = 20 });

        //walls
        walls.Add(new Plattform { x = 0, y = 0, l = 120 });
        walls.Add(new Plattform { x = 200, y = 0, l = 120 });
    }

    public void Update(TimeSpan elapsedTime)
    {
        _elapsedTime += elapsedTime;

        if( _elapsedTime > updateDelay )
        {
            _elapsedTime = new TimeSpan();
            points++;

            //move plattforms down
            List<Plattform> updatedPlattforms = new List<Plattform>();
            for(int i = 0; i < plattforms.Count; i++)
            {
                Plattform p = plattforms[i];
                p.y += 1;

                if(p.y <= 120) updatedPlattforms.Add(p);
            }
            plattforms = updatedPlattforms;
            //check if new plattforms can be added
            for(int x = plattforms.Count; x < MAXplattformcount; x++)
            {
                plattforms.Add(new Plattform { x = random.Next(0,200), y = random.Next((int)boundaries.Top, 50), l = random.Next(20,70) });
            }
        }
    }

    public struct Plattform
    {
        public int x;
        public int y;
        public int l;

        public (int left, int right, int y) Bounds() => (x, x + l, y);
    }
}
