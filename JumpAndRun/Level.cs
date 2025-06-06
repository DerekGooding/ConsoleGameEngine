namespace JumpAndRun;

class Level
{
    public List<Plattform> plattforms;
    public List<Plattform> walls;
    TimeSpan _elapsedTime = new();
    readonly TimeSpan updateDelay = new(0, 0, 0, 0, 40);
    const int MAXplattformcount = 7;
    readonly Random random = new();
    Rect boundaries = new(0,9,200,111);
    public int points = 0;

    public Level()
    {
        plattforms = [];
        walls = [];

        //ground-plattform
        plattforms.Add(new Plattform (0, 120, 200 ));
        //intro plattforms
        plattforms.Add(new Plattform (50, 70, 70));
        plattforms.Add(new Plattform (90, 100, 70));
        plattforms.Add(new Plattform (150, 40, 35));
        plattforms.Add(new Plattform (130, 55, 20));

        //walls
        walls.Add(new Plattform (0, 0, 120));
        walls.Add(new Plattform (200, 0, 120));
    }

    public void Update(TimeSpan elapsedTime)
    {
        _elapsedTime += elapsedTime;

        if( _elapsedTime > updateDelay )
        {
            _elapsedTime = new TimeSpan();
            points++;

            //move plattforms down
            var updatedPlattforms = new List<Plattform>();
            for(var i = 0; i < plattforms.Count; i++)
            {
                var p = plattforms[i];
                p.Y++;

                if(p.Y <= 120) updatedPlattforms.Add(p);
            }
            plattforms = updatedPlattforms;
            //check if new plattforms can be added
            for(var x = plattforms.Count; x < MAXplattformcount; x++)
            {
                plattforms.Add(new Plattform (random.Next(0, 200),
                                              random.Next(boundaries.Top, 50),
                                              random.Next(20, 70)));
            }
        }
    }

    public record struct Plattform(int X, int Y, int L)
    {
        public readonly (int left, int right, int y) Bounds() => (X, X + L, Y);
    }
}
