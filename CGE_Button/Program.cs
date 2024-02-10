﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGameEngine;
using static ConsoleGameEngine.NativeMethods;

namespace CGE_Button
{

    class CGE_Button : GameConsole
    {

        IntPtr inHandle;
        delegate void MyDelegate();

        Button button;
        int counter = 0;

        public CGE_Button()
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

            

            TextWriter.LoadFont("fontsheet.txt", 7, 9);

            button = new Button(40, 40, TextWriter.GenerateTextSprite("Click me!", TextWriter.Textalignment.Left, 1), feedbackSprite: TextWriter.GenerateTextSprite("Click me!", TextWriter.Textalignment.Left, 1, backgroundColor: 0, foregroundColor:15), hooverSprite: TextWriter.GenerateTextSprite("Click me!", TextWriter.Textalignment.Left, 1, backgroundColor: (short)COLOR.FG_DARK_GREY, foregroundColor: 15));
            button.OnButtonClicked(ButtonClicked);

            ConsoleListener.MouseEvent += ConsoleListener_MouseEvent;

            ConsoleListener.Start();

            return true;
        }
        public override bool OnUserUpdate(TimeSpan elapsedTime)
        {
            Clear();

            DrawSprite(button.x, button.y, button.outputSprite);

            DrawSprite(0, 0, TextWriter.GenerateTextSprite($"Counter: {counter}", TextWriter.Textalignment.Left, 1, backgroundColor: (short)COLOR.TRANSPARENT, foregroundColor: (short)COLOR.FG_WHITE));

            return true;
        }

        private void ConsoleListener_MouseEvent(MOUSE_EVENT_RECORD r)
        {
            button.Update(r);
        }

        public bool ButtonClicked()
        {
            counter++;

            return true;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(437);

            using (var f = new CGE_Button())
                f.Start();
        }
    }
}
