using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Blackboard
    {
        public Blackboard()
        {
            buffer = new char[79 * 24]; //konsola ma wymiary 80 szer x 25 wys, dolny wiersz ma byc wolny
        }

        public void Clear()
        {
            for(int i = 0; i < 79 * 24; i++)
            {
                buffer[i] = ' ';
            }
        }

        public void Add(IDrawable drawable)
        {
            drawable.Draw(buffer);
        }

        public void DrawAll()
        {
            string str = "";

            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < 24; i++)
            {
                for(int j = 0; j < 79; j++)
                {
                    builder.Append(buffer[j + i * 79]);
                }
                builder.Append('\n');
            }

            str = builder.ToString();

            Console.SetCursorPosition(0, 0);
            //Console.Clear();
            Console.Write(str);
        }

        private char[] buffer;

        

    }
}
