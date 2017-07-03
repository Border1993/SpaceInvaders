using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    abstract class GameObject : IDrawable
    {
        public GameObject()
        {
            SetPosition(0, 0);
            SetSymbol('x');
        }

        public void Draw(char[] tab)
        {
            ApproximatePosition();
            tab[mapX + mapY * 79] = symbol;
        }

        public void SetPosition(float x, float y)
        {
            if(x < 0 || x > 78 || y < 0 || y > 23)
            {
                throw new Exception("Pawn outside of map");
            }
            else
            {
                this.x = x;
                this.y = y;
            }
        }

        public void SetSymbol(char c)
        {
            this.symbol = c;
        }

        private void ApproximatePosition()
        {
            if (x - (int)x > 0.5f) mapX = (int)x + 1;
            else mapX = (int)x;

            if (y - (int)y > 0.5f) mapY = (int)y + 1;
            else mapY = (int)y;
        }

        public float X
        {
            get { return x; }
            // read only
        }

        public float Y
        {
            get { return y; }
            // read only
        }

        public bool CheckCollision(GameObject other)
        {
            this.ApproximatePosition();
            other.ApproximatePosition();

            if (this.mapX == other.mapX && this.mapY == other.mapY) return true;
            else return false;
        }

        public abstract void Tick(float timeDelta);


        public EOwner owner;
        private float x;
        private float y;

        private char symbol;
        private int mapX;
        private int mapY;
    }
}
