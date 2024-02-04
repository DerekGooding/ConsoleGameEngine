﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;

namespace CGE_Mouse
{
    internal class Program
    {
        class CGE_Mouse : GameConsole
        {

            IntPtr inHandle;
            delegate void MyDelegate();

            int cursorX = 0, cursorY = 0;


            public CGE_Mouse()
              : base(200, 120, "Fonts", fontwidth: 4, fontheight: 4)
            { }
            public override bool OnUserCreate()
            {
                inHandle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);
                uint mode = 0;
                NativeMethods.GetConsoleMode(inHandle, ref mode);
                mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE; //disable
                mode |= NativeMethods.ENABLE_WINDOW_INPUT; //enable (if you want)
                mode |= NativeMethods.ENABLE_MOUSE_INPUT; //enable
                NativeMethods.SetConsoleMode(inHandle, mode);

                
                ConsoleListener.MouseEvent += ConsoleListener_MouseEvent;

                ConsoleListener.Start();

                TextWriter.LoadFont("fontsheet.txt", 7, 9);

                return true;
            }

            public override bool OnUserUpdate(TimeSpan elapsedTime)
            {
                Clear();

                DrawSprite(0, 0, TextWriter.GenerateTextSprite($"X: {cursorX}; Y: {cursorY}", TextWriter.Textalignment.Left, 1));

                return true;
            }

            private void ConsoleListener_MouseEvent(MOUSE_EVENT_RECORD r)
            {
                cursorX = r.dwMousePosition.X;
                cursorY = r.dwMousePosition.Y;
            }
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(437);

            using (var f = new CGE_Mouse())
                f.Start();
        }
    }
}
