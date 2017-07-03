using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Enemy : GameObject, IDamagable
    {
        public Enemy()
        {
            dead = false;

            shootingCooldown = 0.5f;
            timeToNextBullet = 0.0f;

            this.SetPosition(0, 0);
            this.SetSymbol('^');
            this.directionLeft = false;
            this.speed = 0.5f;
            points = 100;
        }

        public bool DealDamage(EOwner owner)
        {
            if (owner == EOwner.PLAYER) //jesli wroga dosiegla kula gracza, ma umrzec. 
            {
                dead = true;
                return true; //zwracamy prawde jako sygnal ze pocisk mozna usunac
            }
            else
            {
                return false; //kula nie znika po trafieniu swojego
            }
        }

        public bool IsDead()
        {
            return dead;
        }

        public void ClearShoot()
        {
            shoot = false;
        }

        public void Shoot()
        {
            if (timeToNextBullet <= 0)
            {
                shoot = true;
                timeToNextBullet = shootingCooldown;
            }            
        }

        public override void Tick(float timeDelta)
        {
            timeToNextBullet -= timeDelta;
            Move(timeDelta);
        }

        void Move(float timeDelta)
        {
            if(directionLeft)
            {
                try
                {
                    this.SetPosition(X - timeDelta * speed, Y);
                }
                catch(Exception ex)
                {
                    //zderzenie ze sciana; dajemy potworka linijke nizej
                    this.SetPosition(0, Y + 1);
                    directionLeft = false;
                }
            }
            else
            {
                try
                {
                    this.SetPosition(X + timeDelta * speed, Y);
                }
                catch (Exception ex)
                {
                    //zderzenie ze sciana; dajemy potworka linijke nizej
                    this.SetPosition(78, Y + 1);
                    directionLeft = true;
                }
            }
        }

        public float Speed
        {
            get { return speed; }
            set { this.speed *= value; }
        }

        public bool EnemyShoot
        {
            get { return shoot; }
            //ReadOnly
        }

        public bool CanShoot
        {
            get
            {
                if (timeToNextBullet <= 0) return true;
                else return false;
            }
        }


        float speed;
        public int points;
        private bool dead; //czy wrog zywy
        private bool shoot;
        public bool directionLeft;
        private float shootingCooldown; //zeby wrog nie strzelal co wyswietlenie planszy tylko z okreslona czestotliwoscia
        private float timeToNextBullet; //ile zostalo do odczekania zeby mozna bylo znowu strzelic
    }
}
