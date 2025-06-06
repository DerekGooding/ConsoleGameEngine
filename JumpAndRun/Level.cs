namespace JumpAndRun;

class Level
{
    public List<Plattform> Plattforms;
    public List<Plattform> Walls;
    public int Points;

    TimeSpan _elapsedTime;
    readonly TimeSpan _updateDelay = new(0, 0, 0, 0, 40);
    const int _maxPlattformcount = 7;
    readonly Random _random = Random.Shared;
    Rect _boundaries = new(0,9,200,111);

    public Level()
    {
        Plattforms = [];
        Walls = [];

        //ground-plattform
        Plattforms.Add(new Plattform (0, 120, 200 ));
        //intro plattforms
        Plattforms.Add(new Plattform (50, 70, 70));
        Plattforms.Add(new Plattform (90, 100, 70));
        Plattforms.Add(new Plattform (150, 40, 35));
        Plattforms.Add(new Plattform (130, 55, 20));

        //walls
        Walls.Add(new Plattform (0, 0, 120));
        Walls.Add(new Plattform (200, 0, 120));
    }

    public void Update(TimeSpan elapsedTime)
    {
        _elapsedTime += elapsedTime;

        if( _elapsedTime > _updateDelay )
        {
            _elapsedTime = new TimeSpan();
            Points++;

            //move plattforms down
            var updatedPlattforms = new List<Plattform>();
            for(var i = 0; i < Plattforms.Count; i++)
            {
                var p = Plattforms[i];
                p.Y++;

                if(p.Y <= 120) updatedPlattforms.Add(p);
            }
            Plattforms = updatedPlattforms;
            //check if new plattforms can be added
            for(var x = Plattforms.Count; x < _maxPlattformcount; x++)
            {
                Plattforms.Add(new Plattform (_random.Next(0, 200),
                                              _random.Next(_boundaries.Top, 50),
                                              _random.Next(20, 70)));
            }
        }
    }

    public record struct Plattform(int X, int Y, int L)
    {
        public readonly (int left, int right, int y) Bounds() => (X, X + L, Y);
    }
}
