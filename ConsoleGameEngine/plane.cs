namespace ConsoleGameEngine;

public class Plane<T>(int width, int height)
{
    public T[] Data { get; } = new T[width * height];
    public int Width { get; } = width;
    public int Height { get; } = height;

    public int GetOffset(int x, int y) => (GameConsole.Clamp(y, 0, Height - 1) * Width) + GameConsole.Clamp(x, 0, Width - 1);

    private int EnsureValidOffset(int offset) => GameConsole.Clamp(offset, 0, Data.Length);

    public void SetData(int offset, T data) => Data[EnsureValidOffset(offset)] = data;

    public void SetData(int x, int y, T data) => Data[GetOffset(x, y)] = data;

    public T GetData(int offset) => Data[EnsureValidOffset(offset)];

    public T GetData(int x, int y) => Data[GetOffset(x, y)];
}
