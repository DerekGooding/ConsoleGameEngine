namespace ConsoleGameEngine
{
    public class Plane<T>
    {
        private readonly T[] _data;

        public T[] Data { get { return _data; } }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Plane(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new T[width * height];
        }

        public int GetOffset(int x, int y) => GameConsole.Clamp(y, 0, Height - 1) * Width + GameConsole.Clamp(x, 0, Width - 1);

        private int EnsureValidOffset(int offset) => GameConsole.Clamp(offset, 0, _data.Length);

        public void SetData(int offset, T data) => _data[EnsureValidOffset(offset)] = data;

        public void SetData(int x, int y, T data) => _data[GetOffset(x, y)] = data;

        public T GetData(int offset) => _data[EnsureValidOffset(offset)];

        public T GetData(int x, int y) => _data[GetOffset(x, y)];
    }
}
