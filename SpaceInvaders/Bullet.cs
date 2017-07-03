using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Bullet : GameObject
    {
        public Bullet(float x, float y, EOwner owner, bool directionUp)
        {
            this.directionUp = directionUp; // domyslna wartosc
            this.owner = owner;
            this.deleteThis = false;
            this.speed = 10.0f;
            this.SetSymbol('+');
            
            try
            {
                SetPosition(x, y);
            }
            catch(Exception ex)
            {
                deleteThis = true;
            }
        }

        public void Move(float x, float y)
        {
            try
            {
                SetPosition(x, y);
            }
            catch(Exception ex)
            {
                //kula wypadla za plansze lub cos poszlo bardzo nie tak;
                //zaznaczamy ze kula do skasowania

                this.deleteThis = true;
            }
        }

        //jesli kula zada obrazenia to ona tez znika;
        //jesli nie to nic sie nie stanie, kula przeniknie przez cel
        //jesli cel nie jest wrogi to tez nic sie nie stanie
        public void Kill(IDamagable target)
        {
            if (target.DealDamage(this.owner))
            {
                //kula trafila przeciwnika
                this.deleteThis = true;
            }

        }

        public override void Tick(float timeDelta)
        {
            if(directionUp == true)
            {
                Move(X, Y - speed * timeDelta);
            }
            else
            {
                Move(X, Y + speed * timeDelta);
            }
        }

        public bool deleteThis;
        public float speed;
        private bool directionUp;

    }
}
