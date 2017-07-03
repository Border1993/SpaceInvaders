using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Diagnostics;

namespace SpaceInvaders
{
    class Program
    {
        static void InitailzeEnemies(List<Enemy> enemies, int count, float speedMultiplier)
        {
            enemies.Clear();
            for (int i = 0; i < count; i++)
            {
                Enemy newEnemy = new Enemy();
                newEnemy.owner = EOwner.ENEMY;
                newEnemy.SetSymbol('#');
                newEnemy.Speed = speedMultiplier;

                //wzor zeby potworki na poczatku ukladaly sie w 'szachownice'
                float x = (i*3) % 80;
                float y = (i*3 / 80)*2 + 1; // gorny rzadek zarezerwowany na bonus - ufo


                try
                {
                    newEnemy.SetPosition(x, y);
                    enemies.Add(newEnemy);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Something went wrong with setting enemies position.");
                }

                
            }
        }

        static int CountEnemyBullets(List<Bullet> bullets)
        {
            int count = 0;

            for(int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].owner == EOwner.ENEMY) count++;
            }

            return count;
        }

        static void Tick(Player player, List<Enemy> enemies, List<Bullet> bullets, float timeDelta)
        {
            //najpierw ruch
            player.Tick(timeDelta);
            for (int i = 0; i < enemies.Count; i++) enemies[i].Tick(timeDelta);
            for (int i = 0; i < bullets.Count; i++) bullets[i].Tick(timeDelta);

            //potem polecenia wystrzalu
            if(player.PlayerShoot)
            {
                Bullet bullet = new Bullet(player.X, player.Y, EOwner.PLAYER, true);
                if (!bullet.deleteThis) bullets.Add(bullet);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].EnemyShoot)
                {
                    Bullet bullet = new Bullet(enemies[i].X, enemies[i].Y, EOwner.ENEMY, false);
                    if (!bullet.deleteThis) bullets.Add(bullet);
                }
            }

            //kolizje kul
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    if(enemies[j].CheckCollision(bullets[i]))
                    {
                        bullets[i].Kill(enemies[j]);
                    }
                }

                if(player.CheckCollision(bullets[i]))
                {
                    bullets[i].Kill(player);
                }
            }

            //sprzatamy smieci
            List<Enemy> newEnemies = new List<Enemy>();

            for(int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsDead()) 
                {
                    player.pointsScored += enemies[i].points;
                    enemies.Remove(enemies[i]);
                    i--;
                }
                
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].deleteThis)
                {
                    bullets.Remove(bullets[i]);
                    i--;
                }
            }
        }
        
        static void EnemyAI(List<Enemy> enemies, List<Bullet> bullets, int maxEnemyBullets)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                enemies[i].ClearShoot();
            }

            Random rng = new Random();
            int existingEnemyBullets = CountEnemyBullets(bullets);

            if (existingEnemyBullets < maxEnemyBullets)
            {
                int bulletsToShoot = maxEnemyBullets - existingEnemyBullets;
                List<int> enemiesWhoCanShoot = new List<int>();

                for(int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].CanShoot) enemiesWhoCanShoot.Add(i);
                }

                for(int i = 0; i < Math.Min(bulletsToShoot, enemiesWhoCanShoot.Count); i++)
                {
                    int randomIndex = rng.Next(0, enemiesWhoCanShoot.Count);
                    int index = enemiesWhoCanShoot[randomIndex];
                    enemies[index].Shoot();
                    enemiesWhoCanShoot.Remove(index);
                }
            }
        }

        static void Draw(Player player, List<Enemy> enemies, List<Bullet> bullets, Blackboard screen)
        {
            screen.Add(player);
            foreach (Enemy e in enemies) screen.Add(e);
            foreach (Bullet b in bullets) screen.Add(b);
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Game has started!");

            Blackboard screen = new Blackboard();
            Player player = new Player();
            List<Enemy> enemies = new List<Enemy>();
            List<Bullet> bullets = new List<Bullet>();
            Stopwatch watch = new Stopwatch();

            float t1 = 0;
            float t2 = 0;
            float timeDelta = 0;

            watch.Start();
            t1 = watch.ElapsedMilliseconds;

            int enemyCount = 100;
            int maxBullets = 5;
            float baseSpeed = 2.0f;

            InitailzeEnemies(enemies, enemyCount, baseSpeed);

            while (!player.IsDead())
            {
                if(enemies.Count == 0)
                {
                    maxBullets++;
                    baseSpeed *= 1.5f;
                    InitailzeEnemies(enemies, enemyCount, baseSpeed);
                }

                t2 = watch.ElapsedMilliseconds;
                timeDelta = (t2 - t1) / 1000;
                t1 = t2;

                EnemyAI(enemies, bullets, maxBullets);
                Tick(player, enemies, bullets, timeDelta);
                 
                screen.Clear();
                Draw(player, enemies, bullets, screen);

                screen.DrawAll();
                Thread.Sleep(16);
            }
            Console.Clear();
            Console.WriteLine("You are dead. Points scored : " + player.pointsScored.ToString());

            Thread.Sleep(5000);
        }

        
    }
}
