using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Player : GameObject, IDamagable
    {
        public Player()
        {
            dead = false;
            shootingCooldown = 0.5f;
            timeToNextBullet = 0.0f;
            pointsScored = 0;

            try
            {
                this.SetPosition(40, 23.0f);
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Something went wrong.");
            }
            
            this.SetSymbol('^');
            
        }

        public bool DealDamage(EOwner owner)
        {
            if(owner == EOwner.ENEMY) //jesli gracza dosiegla wroga kula, ma umrzec. 
            {
                dead = true;
                return true; //zwracamy prawde jako sygnal ze pocisk mozna usunac
            }
            else
            {
                return false; 
                //raczej niepotrzebne, gracza nie trafi nigdy jego wlasna kula
                //glownie zeby kompilator poszedl plumkac sie gdzie indziej
            }
        }

        public bool IsDead()
        {
            return dead;
        }

        public bool Shoot()
        {
            bool shotsFired;
            if (timeToNextBullet <= 0)
            {
                shotsFired = true;
                timeToNextBullet = shootingCooldown;
            }
            else shotsFired = false;

            return shotsFired;
        }

        private void Move(float x, float y)
        {
            try
            {
                this.SetPosition(x, y);
            }
            catch(Exception ex)
            {
                //gracz nie moze sie tam ruszyc bo wyjdzie za mape;
                //nie musimy nic robic
            }
        }

        public override void Tick(float timeDelta)
        {
            shoot = false;
            if (timeToNextBullet > 0) timeToNextBullet -= timeDelta;

            if(Console.KeyAvailable)
            {
                var key = Console.ReadKey();

                if (key.KeyChar == 'a')
                {
                    Move(X - 1, Y);
                }
                else if (key.KeyChar == 'd')
                {
                    Move(X + 1, Y);
                }
                else if (key.KeyChar == ' ')
                {
                    shoot = Shoot();
                }

                //wywalic inne przyciski z bufora
                //bez tego po przytrzymaniu przycisku dluzszym jeszcze przez 10 min bedzie go powtarzal
                while (Console.KeyAvailable) Console.ReadKey();
            }
        }

        public bool PlayerShoot
        {
            get { return shoot; }
            //read only
        }

        public int pointsScored;
        private bool dead; //czy gracz zywy
        private bool shoot;
        private float shootingCooldown; //zeby gracz nie strzelal co wyswietlenie planszy tylko z okreslona czestotliwoscia
        private float timeToNextBullet; //ile zostalo do odczekania zeby mozna bylo znowu strzelic
    }
}
