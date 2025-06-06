using System.Collections.Generic;
using System.Linq;

namespace ConsoleGameEngine;

public static class TextWriter
{
    static Sprite spriteSheet, spriteSheetSMALL;
    public static FontPadding fontPadding = new(0, 0, 0, 0);
    static int width, height, widthSMALL, heightSMALL;
    static Dictionary<char, Coords> dictionary = new()
    { { ' ', new Coords {X= 0, Y=0 } },
                                                                                { '!', new Coords {X= 1, Y=0 } },
                                                                                { '"', new Coords {X= 2, Y=0 } },
                                                                                { '#', new Coords {X= 3, Y=0 } },
                                                                                { '$', new Coords {X= 4, Y=0 } },
                                                                                { '%', new Coords {X= 5, Y=0 } },
                                                                                { '&', new Coords {X= 6, Y=0 } },
                                                                                { '\'', new Coords {X= 7, Y=0 } },
                                                                                { '(', new Coords {X= 8, Y=0 } },
                                                                                { ')', new Coords {X= 9, Y=0 } },
                                                                                { '*', new Coords {X= 10, Y=0 } },
                                                                                { '+', new Coords {X= 11, Y=0 } },
                                                                                { ',', new Coords {X= 12, Y=0 } },
                                                                                { '-', new Coords {X= 13, Y=0 } },
                                                                                { '.', new Coords {X= 14, Y=0 } },
                                                                                { '/', new Coords {X= 15, Y=0 } },
                                                                                { '0', new Coords {X= 16, Y=0 } },
                                                                                { '1', new Coords {X= 17, Y=0 } },
                                                                                { '2', new Coords {X= 0, Y=1 } },
                                                                                { '3', new Coords {X= 1, Y=1 } },
                                                                                { '4', new Coords {X= 2, Y=1 } },
                                                                                { '5', new Coords {X= 3, Y=1 } },
                                                                                { '6', new Coords {X= 4, Y=1 } },
                                                                                { '7', new Coords {X= 5, Y=1 } },
                                                                                { '8', new Coords {X= 6, Y=1 } },
                                                                                { '9', new Coords {X= 7, Y=1 } },
                                                                                { ':', new Coords {X= 8, Y=1 } },
                                                                                { ';', new Coords {X= 9, Y=1 } },
                                                                                { '<', new Coords {X= 10, Y=1 } },
                                                                                { '=', new Coords {X= 11, Y=1 } },
                                                                                { '>', new Coords {X= 12, Y=1 } },
                                                                                { '?', new Coords {X= 13, Y=1 } },
                                                                                { '@', new Coords {X= 14, Y=1 } },
                                                                                { 'A', new Coords {X= 15, Y=1 } },
                                                                                { 'B', new Coords {X= 16, Y=1 } },
                                                                                { 'C', new Coords {X= 17, Y=1 } },
                                                                                { 'D', new Coords {X= 0, Y=2 } },
                                                                                { 'E', new Coords {X= 1, Y=2 } },
                                                                                { 'F', new Coords {X= 2, Y=2 } },
                                                                                { 'G', new Coords {X= 3, Y=2 } },
                                                                                { 'H', new Coords {X= 4, Y=2 } },
                                                                                { 'I', new Coords {X= 5, Y=2 } },
                                                                                { 'J', new Coords {X= 6, Y=2 } },
                                                                                { 'K', new Coords {X= 7, Y=2 } },
                                                                                { 'L', new Coords {X= 8, Y=2 } },
                                                                                { 'M', new Coords {X= 9, Y=2 } },
                                                                                { 'N', new Coords {X= 10, Y=2 } },
                                                                                { 'O', new Coords {X= 11, Y=2 } },
                                                                                { 'P', new Coords {X= 12, Y=2 } },
                                                                                { 'Q', new Coords {X= 13, Y=2 } },
                                                                                { 'R', new Coords {X= 14, Y=2 } },
                                                                                { 'S', new Coords {X= 15, Y=2 } },
                                                                                { 'T', new Coords {X= 16, Y=2 } },
                                                                                { 'U', new Coords {X= 17, Y=2 } },
                                                                                { 'V', new Coords {X= 0, Y=3 } },
                                                                                { 'W', new Coords {X= 1, Y=3 } },
                                                                                { 'X', new Coords {X= 2, Y=3 } },
                                                                                { 'Y', new Coords {X= 3, Y=3 } },
                                                                                { 'Z', new Coords {X= 4, Y=3 } },
                                                                                { '[', new Coords {X= 5, Y=3 } },
                                                                                { '\\', new Coords {X= 6, Y=3 } },
                                                                                { ']', new Coords {X= 7, Y=3 } },
                                                                                { '^', new Coords {X= 8, Y=3 } },
                                                                                { '_', new Coords {X= 9, Y=3 } },
                                                                                { '´', new Coords {X= 10, Y=3 } },
                                                                                { 'a', new Coords {X= 11, Y=3 } },
                                                                                { 'b', new Coords {X= 12, Y=3 } },
                                                                                { 'c', new Coords {X= 13, Y=3 } },
                                                                                { 'd', new Coords {X= 14, Y=3 } },
                                                                                { 'e', new Coords {X= 15, Y=3 } },
                                                                                { 'f', new Coords {X= 16, Y=3 } },
                                                                                { 'g', new Coords {X= 17, Y=3 } },
                                                                                { 'h', new Coords {X= 0, Y=4 } },
                                                                                { 'i', new Coords {X= 1, Y=4 } },
                                                                                { 'j', new Coords {X= 2, Y=4 } },
                                                                                { 'k', new Coords {X= 3, Y=4 } },
                                                                                { 'l', new Coords {X= 4, Y=4 } },
                                                                                { 'm', new Coords {X= 5, Y=4 } },
                                                                                { 'n', new Coords {X= 6, Y=4 } },
                                                                                { 'o', new Coords {X= 7, Y=4 } },
                                                                                { 'p', new Coords {X= 8, Y=4 } },
                                                                                { 'q', new Coords {X= 9, Y=4 } },
                                                                                { 'r', new Coords {X= 10, Y=4 } },
                                                                                { 's', new Coords {X= 11, Y=4 } },
                                                                                { 't', new Coords {X= 12, Y=4 } },
                                                                                { 'u', new Coords {X= 13, Y=4 } },
                                                                                { 'v', new Coords {X= 14, Y=4 } },
                                                                                { 'w', new Coords {X= 15, Y=4 } },
                                                                                { 'x', new Coords {X= 16, Y=4 } },
                                                                                { 'y', new Coords {X= 17, Y=4 } },
                                                                                { 'z', new Coords {X= 0, Y=5 } },
                                                                                { '{', new Coords {X= 1, Y=5 } },
                                                                                { '|', new Coords {X= 2, Y=5 } },
                                                                                { '}', new Coords {X= 3, Y=5 } },
                                                                                { '~', new Coords {X= 4, Y=5 } }};
    static readonly Dictionary<char, Coords> dictionarySMALL = new()
    { {'A', new Coords {X= 0, Y=0 }},
            {'B', new Coords {X= 1, Y=0 } },
            {'C', new Coords {X= 2, Y=0 } },
            {'D', new Coords {X= 3, Y=0 } },
            {'E', new Coords {X= 4, Y=0 } },
            {'F', new Coords {X= 5, Y=0 } },
            {'G', new Coords {X= 6, Y=0 } },
            {'H', new Coords {X= 7, Y=0 } },
            {'I', new Coords {X= 8, Y=0 } },
            {'J', new Coords {X= 9, Y=0 } },
            {'K', new Coords {X= 10, Y=0 } },
            {'L', new Coords {X= 11, Y=0 } },
            {'M', new Coords {X= 12, Y=0 } },
            {'N', new Coords {X= 0, Y=1 } },
            {'O', new Coords {X= 1, Y=1 } },
            {'P', new Coords {X= 2, Y=1 } },
            {'Q', new Coords {X= 3, Y=1 } },
            {'R', new Coords {X= 4, Y=1 }},
            {'S', new Coords {X= 5, Y=1 }},
            {'T', new Coords {X= 6, Y=1 }},
            {'U', new Coords {X= 7, Y=1 }},
            {'V', new Coords {X= 8, Y=1 }},
            {'W', new Coords {X= 9, Y=1 }},
            {'X', new Coords {X= 10, Y=1 }},
            {'Y', new Coords {X= 11, Y=1 }},
            {'Z', new Coords {X= 12, Y=1 }},
            {'a', new Coords {X= 0, Y=2 }},
            {'b', new Coords {X= 1, Y=2 }},
            {'c', new Coords {X= 2, Y=2 }},
            {'d', new Coords {X= 3, Y=2 }},
            {'e', new Coords {X= 4, Y=2 }},
            {'f', new Coords {X= 5, Y=2 }},
            {'g', new Coords {X= 6, Y=2 }},
            {'h', new Coords {X= 7, Y=2 }},
            {'i', new Coords {X= 8, Y=2 }},
            {'j', new Coords {X= 9, Y=2 }},
            {'k', new Coords {X= 10, Y=2 }},
            {'l', new Coords {X= 11, Y=2 }},
            {'m', new Coords {X= 12, Y=2 }},
            {'n', new Coords {X= 0, Y=3 }},
            {'o', new Coords {X= 1, Y=3 }},
            {'p', new Coords {X= 2, Y=3 }},
            {'q', new Coords {X= 3, Y=3 }},
            {'r', new Coords {X= 4, Y=3 }},
            {'s', new Coords {X= 5, Y=3 }},
            {'t', new Coords {X= 6, Y=3 }},
            {'u', new Coords {X= 7, Y=3 }},
            {'v', new Coords {X= 8, Y=3 }},
            {'w', new Coords {X= 9, Y=3 }},
            {'x', new Coords {X= 10, Y=3 }},
            {'y', new Coords {X= 11, Y=3 }},
            {'z', new Coords {X= 12, Y=3 }},

            {'.', new Coords {X= 0, Y=4 }},
            {',', new Coords {X= 1, Y=4 }},
            {'!', new Coords {X= 2, Y=4 }},
            {'?', new Coords {X= 3, Y=4 }},
            {':', new Coords {X= 4, Y=4 }},
            {';', new Coords {X= 5, Y=4 }},
            {'(', new Coords {X= 6, Y=4 }},
            {')', new Coords {X= 7, Y=4 }},
            {'/', new Coords {X= 8, Y=4 }},
            {'+', new Coords {X= 9, Y=4 }},
            {'-', new Coords {X= 10, Y=4 }},
            {' ', new Coords {X= 11, Y=4 }},
            {'_', new Coords {X= 12, Y=4 }},
            };

    public static void LoadFont(string fileName, int w, int h)
    {
        spriteSheet = new Sprite(fileName);
        width = w;
        height = h;
    }
    public static void LoadSmallFont(string fileName, int w, int h)
    {
        spriteSheetSMALL = new Sprite(fileName);
        widthSMALL = w;
        heightSMALL = h;
    }
    public static void LoadFont(string fileName, int w, int h, FontPadding _fontPadding)
    {
        spriteSheet = new Sprite(fileName);
        width = w; height = h;
        fontPadding = _fontPadding;
    }
    public static void SetDictionary(Dictionary<char, Coords> _dictionary) => dictionary = _dictionary;

    public static Sprite GenerateTextSprite(string text, Textalignment textalignment, int fontSize, short backgroundColor = (short)GameConsole.COLOR.FG_WHITE, short foregroundColor = (short)GameConsole.COLOR.FG_BLACK, FontType fontType = FontType.standard)
    {
        Sprite sprite;

        var numberOfLines = text.Split('\n').Length;
        var longesLineLength = text.Split('\n').OrderByDescending(s => s.Length).First().Length;

        int _width = 0, _height = 0;
        var _dictonary = new Dictionary<char, Coords>();

        if (fontType == FontType.standard)
        {
            _width = width;
            _height = height;
            _dictonary = dictionary;
        }
        else if (fontType == FontType.small)
        {
            _width = widthSMALL;
            _height = heightSMALL;
            _dictonary = dictionarySMALL;
        }

        sprite = new Sprite(width * fontSize * longesLineLength, height * numberOfLines * fontSize);

        var row = 0;
        foreach (var str in text.Split('\n'))
        {
            var allignmentOffset = 0;
            //offsets for right and center
            if (textalignment == Textalignment.Center)
            {
                allignmentOffset = (longesLineLength - str.Length) * width * fontSize / 2;
            }
            else if (textalignment == Textalignment.Right)
            {
                allignmentOffset = (longesLineLength - str.Length) * width * fontSize;
            }

            for (var i = 0; i < str.Length; ++i)
            {
                var coords = _dictonary[str[i]];
                var letter = new Sprite(1,1);

                if (fontType == FontType.small)
                    letter = spriteSheetSMALL.ReturnPartialSprite((coords.X * _width) + fontPadding.Left + coords.X + 1, (coords.Y * _height) + fontPadding.Top + coords.Y + 1, _width, _height);
                else if (fontType == FontType.standard)
                    letter = spriteSheet.ReturnPartialSprite((coords.X * _width) + fontPadding.Left + coords.X + 1, (coords.Y * _height) + fontPadding.Top + coords.Y + 1, _width, _height);

                for (var x = 0; x < letter.Width; x++)
                {
                    for (var y = 0; y < letter.Height; y++)
                    {
                        var spritesColor = letter.GetColor(x, y);
                        var spritesChar = '\0';
                        if (spritesColor == (short)GameConsole.COLOR.FG_BLACK)
                        {
                            if (foregroundColor != (short)GameConsole.COLOR.TRANSPARENT)
                                spritesChar = letter.GetChar(x, y);
                            spritesColor = foregroundColor;
                        }
                        else if (spritesColor == (short)GameConsole.COLOR.FG_GREY)
                        {
                            if (backgroundColor != (short)GameConsole.COLOR.TRANSPARENT)
                                spritesChar = letter.GetChar(x, y);
                            spritesColor = backgroundColor;
                        }

                        sprite.SetBlock((i * _width * fontSize) + (x * fontSize) + allignmentOffset, (row *_height*fontSize) + (y *fontSize), fontSize, fontSize, spritesChar, spritesColor);
                    }
                }
            }
            row++;
        }

        return sprite;
    }

    public struct Coords
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum Textalignment
    {
        Left = 0,
        Center = 1,
        Right = 2,
    }

    public enum FontType
    {
        standard,
        small
    }

    public record struct FontPadding(int Top, int Left, int Right, int Bottom);
}
