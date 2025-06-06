namespace ConsoleGameEngine;

public class Plane<T>
{
    public T[] Data { get; }
    public int Width { get; }
    public int Height { get; }

    public Plane(int width, int height)
    {
        Width = width;
        Height = height;
        Data = new T[width * height];
    }

    public int GetOffset(int x, int y) => (GameConsole.Clamp(y, 0, Height - 1) * Width) + GameConsole.Clamp(x, 0, Width - 1);

    private int EnsureValidOffset(int offset) => GameConsole.Clamp(offset, 0, Data.Length);

    public void SetData(int offset, T data) => Data[EnsureValidOffset(offset)] = data;

    public void SetData(int x, int y, T data) => Data[GetOffset(x, y)] = data;

    public T GetData(int offset) => Data[EnsureValidOffset(offset)];

    public T GetData(int x, int y) => Data[GetOffset(x, y)];
}
