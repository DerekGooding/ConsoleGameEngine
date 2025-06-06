using static ConsoleGameEngine.GameConsole;

namespace ConsoleGameEngine;

public class Sprite
{
    private Plane<char> _spritedata;
    private Plane<short> _spritecolors;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Sprite(string[] spriteData, short[] spriteColors = null)
    {
        if (spriteData.Length == 0)
            throw new ArgumentException(null, nameof(spriteData));
        if (spriteData.Any(s => s.Length != spriteData[0].Length))
            throw new ArgumentException(null, nameof(spriteData));
        Width = spriteData[0].Length;
        Height = spriteData.Length;

        if (spriteColors != null && spriteColors.Length != Width * Height)
            throw new ArgumentException(null, nameof(spriteColors));

        _spritedata = new Plane<char>(Width, Height);
        _spritecolors = new Plane<short>(Width, Height);

        var i = 0;

        foreach (var s in spriteData)
        {
            foreach (var c in s)
                _spritedata.SetData(i++, c);
        }

        for (i = 0; i < _spritecolors.Data.Length; i++)
            _spritecolors.SetData(i, spriteColors == null ? (short)COLOR.FG_GREY : spriteColors[i]);
    }

    public Sprite(string[] spriteData, COLOR[] spriteColors = null)
        : this(spriteData, spriteColors?.Select(c => (short)c).ToArray())
    { }

    public Sprite(string file)
    {
        if (!Load(file)) Create(8, 8);
    }

    public Sprite(int w, int h) => Create(w, h);
    public Sprite(int w, int h, COLOR color) => Create(w, h, color);

    public Sprite(int w, int h, char c, COLOR color) => Create(w, h, c, color);

    public Sprite(string file, int w, int h, int startRow, int count)
    {
        if(!Load(file, w, h , startRow, count)) Create(8,8);
    }

    public Sprite ReturnPartialSprite(int x, int y, int w, int h)
    {
        var returnSprite = new Sprite(w, h);

        for(var i = x; i < x + w; i++)
        {
            for(var j = y; j < y +h; j++)
            {
                returnSprite.SetPixel(i - x, j - y, _spritedata.GetData(i, j), _spritecolors.GetData(i, j));
            }
        }
        return returnSprite;
    }

    public Sprite ReturnPartialSpriteInverted(int x, int y, int w, int h)
    {
        var returnSprite = new Sprite(w, h);

        for (var i = x; i < x + w; i++)
        {
            for (var j = y; j < y + h; j++)
            {
                var color = _spritecolors.GetData(i, j);
                var invertedColor = (short)((short)(color >> 4) + (short)((color & 0x0F) << 4));

                returnSprite.SetPixel(i - x, j - y, _spritedata.GetData(i, j), invertedColor);
            }
        }
        return returnSprite;
    }

    public Sprite FlipHorizontally()
    {
        var returnSprite = new Sprite(Width, Height);

        for(var x = 0; x < Width; x++)
        {
            for(var y = 0; y < Height; y++)
            {
                returnSprite.SetChar(x,y, _spritedata.GetData(Width - 1 - x, y));
                returnSprite.SetColor(x, y, _spritecolors.GetData(Width - 1 - x, y));
            }
        }
        return returnSprite;
    }
    public Sprite FlipVertically()
    {
        var returnSprite = new Sprite(Width, Height);

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                returnSprite.SetChar(x, y, _spritedata.GetData(x, Height - 1 - y));
                returnSprite.SetColor(x, y, _spritecolors.GetData(x, Height - 1 - y));
            }
        }
        return returnSprite;
    }

    public List<Sprite> ReturnTileList(int w, int h, int rows, int columns)
    {
        var retList = new List<Sprite>();

        for(var r = 0; r < rows; r++)
        {
            for(var c = 0; c < columns; c++)
            {
                retList.Add(ReturnPartialSprite(c * w, r * h, w, h));
            }
        }

        return retList;
    }

    public void AddSpriteToSprite(int x, int y, Sprite sprite)
    {
        for(var i = x; i < x + sprite.Width; i++)
        {
            for(var j = y; j < y + sprite.Height; j++)
            {
                SetPixel(i,j, sprite.GetChar(i - x, j - y), sprite.GetColor(i - x, j - y));
            }
        }
    }

    #region setter/getter
    public char GetChar(int x, int y) => _spritedata.GetData(x, y);
    public short GetColor(int x, int y) => _spritecolors.GetData(x, y);

    public void SetChar(int x, int y, char c) => _spritedata.SetData(x, y, c);

    public void SetColor(int x, int y, short c) => _spritecolors.SetData(x, y, c);

    public void SetPixel(int x, int y, char c, short col)
    {
        _spritedata.SetData(x, y, c);
        _spritecolors.SetData(x, y, col);
    }

    public void SetBlock(int x, int y, int w, int h, char c, short col)
    {
        for(var i = 0; i < w; i++)
        {
            for(var j = 0; j < h; j++)
            {
                SetPixel(x + i, y + j, c, col);
            }
        }
    }
    #endregion

    #region load/save/create
    public bool Load(string file)
    {
        Width = 0; Height = 0;

        using var sr = new StreamReader(file);
        var content = sr.ReadToEnd();

        var splits = content.Split(';');

        Width = int.Parse(splits[0]);
        Height = int.Parse(splits[1]);

        _spritedata = new Plane<char>(Width, Height);
        _spritecolors = new Plane<short>(Width, Height);

        var i = 0;
        foreach (var pixel in splits[2].Split(','))
        {
            if (pixel != "")
                _spritedata.SetData(i, pixel[0]);
            i++;
        }
        i = 0;
        foreach (var pixel in splits[3].Split(','))
        {
            if (pixel != "")
                _spritecolors.SetData(i, Convert.ToByte(pixel));
            i++;
        }
        return true;
    }

    public bool Load(string file, int w, int h, int startRow, int count)
    {
        Width = w; Height = h;

        using var sr = new StreamReader(file);
        var content = sr.ReadToEnd();

        var splits = content.Split(';');

        Width = int.Parse(splits[0]);

        _spritedata = new Plane<char>(Width, Height);
        _spritecolors = new Plane<short>(Width, Height);

        var i = 0;

        var pixelSplits = splits[2].Split(',');
        var colorSplits = splits[3].Split(',');

        for (var j = startRow * w; j < (startRow + h) * w; j++)
        {
            if (pixelSplits[j] != "")
            {
                _spritedata.SetData(i, pixelSplits[j][0]);
                _spritecolors.SetData(i, Convert.ToByte(colorSplits[j]));
            }
            i++;
        }
        return true;
    }
    public bool Save(string file)
    {
        var sw = new BinaryWriter(File.Open(file, FileMode.OpenOrCreate));
        sw.Write(Width);
        sw.Write(Height);
        foreach (var c in _spritedata.Data)
            sw.Write(c);
        foreach (var s in _spritecolors.Data)
            sw.Write(s);
        sw.Close();

        return true;
    }
    private void Create(int w, int h)
    {
        Width = w;
        Height = h;

        _spritedata = new Plane<char>(Width, Height);
        _spritecolors = new Plane<short>(Width, Height);

        for (var i = 0; i < Width * Height; i++)
            _spritedata.SetData(i, ' ');//m_Glyphs.Add(br.ReadChar());
        for (var i = 0; i < Height * Width; i++)
            _spritecolors.SetData(i, (short)COLOR.BG_BLACK);//m_Colours.Add(br.ReadSByte());
    }

    private void Create(int w, int h, COLOR color)
    {
        Width = w;
        Height = h;

        _spritedata = new Plane<char>(Width, Height);
        _spritecolors = new Plane<short>(Width, Height);

        for (var i = 0; i < Width * Height; i++)
            _spritedata.SetData(i, ' ');
        for (var i = 0; i < Height * Width; i++)
            _spritecolors.SetData(i, (short)color);
    }

    public void Create(int w, int h, char c, COLOR color)
    {
        Width = w;
        Height = h;

        _spritedata = new Plane<char>(Width, Height);
        _spritecolors = new Plane<short>(Width, Height);

        for (var i = 0; i < Width * Height; i++)
            _spritedata.SetData(i, c);
        for (var i = 0; i < Height * Width; i++)
            _spritecolors.SetData(i, (short)color);
    }
    #endregion

    #region sampling
    public char SampleGlyph(double x, double y)
    {
        var sx = (int)(x * (double)Width);
        var sy = (int)((y * (double)Height) - 1.0);

        return sx < 0 || sx >= Width || sy < 0 || sy >= Height ? ' ' : _spritedata.GetData((sy * Width) + sx);
    }
    public short SampleColor(double x, double y)
    {
        var sx = (int)(x * (double)Width);
        var sy = (int)((y * (double)Height) - 1.0);

        return sx < 0 || sx >= Width || sy < 0 || sy >= Height ? (short)COLOR.BG_BLACK : _spritecolors.GetData((sy * Width) + sx);
    }
    #endregion
}
