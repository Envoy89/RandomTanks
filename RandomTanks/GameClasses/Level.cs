using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace RandomTanks.GameClasses
{
    class Level
    {
        public Map map;
        public List<Tank> tanks;
        public List<Bullet> bullets;

        public Level()
        {
            map = new Map();
            bullets = new List<Bullet>();
            tanks = new List<Tank>();
            tanks.Add(new Tank(9 * 50, 5 * 50, TeamType.FirstTeam, 100));
            tanks.Add(new Tank(3*50, 50, TeamType.FirstTeam, 100));
            tanks.Add(new Tank(7*50, 50, TeamType.FirstTeam, 100));
            tanks.Add(new Tank(12*50, 50, TeamType.FirstTeam, 100));
            tanks.Add(new Tank(16*50, 50, TeamType.FirstTeam, 100));

            tanks.Add(new Tank(10 * 50, 10 * 50, TeamType.SecondTeam, 100));
            tanks.Add(new Tank(3 * 50, 13 * 50, TeamType.SecondTeam, 100));
            tanks.Add(new Tank(7 * 50, 13 * 50, TeamType.SecondTeam, 100));
            tanks.Add(new Tank(12 * 50, 13 * 50, TeamType.SecondTeam, 100));
            tanks.Add(new Tank(16 * 50, 13 * 50, TeamType.SecondTeam, 100));
        }

        public void MoveTankX(int tankId, int dx)
        {
            int d = dx > 0 ? 1 : -1;
            while (CanMoveX(tanks[tankId], dx) && dx != 0)
            {
                tanks[tankId].x += d;
                dx -= d;
            }
            if (d < 0)
            {
                tanks[tankId].or = Orientation.West;
            }
            else
            {
                tanks[tankId].or = Orientation.East;
            }
        }

        public void MoveTankY(int tankId, int dy)
        {
            int d = dy > 0 ? 1 : -1;
            while (CanMoveY(tanks[tankId], dy) && dy != 0)
            {
                tanks[tankId].y += d;
                dy -= d;
            }
            if (d < 0)
            {
                tanks[tankId].or = Orientation.North;
            }
            else
            {
                tanks[tankId].or = Orientation.South;
            }
        }

        private bool CanMoveX(Tank tank, int dx)
        {
            if(tank.x - Tank.tankSize / 2 + dx < 0 || tank.x + Tank.tankSize / 2 + dx >= map.mapSizeX)
            {
                return false;
            }
            int d = dx > 0 ? 1 : -1;
            Rectangle tankRect = new Rectangle(tank.x - Tank.tankSize / 2 + d, tank.y - Tank.tankSize / 2, Tank.tankSize, Tank.tankSize);
            foreach (Tank t in tanks)
            {
                if (t == tank)
                {
                    continue;
                }
                Rectangle tRect = new Rectangle(t.x - Tank.tankSize / 2, t.y - Tank.tankSize / 2, Tank.tankSize, Tank.tankSize);
                
                if (tRect.Intersects(tankRect))
                {
                    return false;
                }
            }

            int cubeX = (tank.x + d * Tank.tankSize / 2 + d) / map.mapCubeSIze;

            for (int i = 0; i < map.mass.GetLength(1); i++)
            {
                int y = i * 50;
                int x = cubeX * 50;
                Rectangle cubeRect = new Rectangle(x, y, map.mapCubeSIze, map.mapCubeSIze);
                if (map.mass[cubeX, i] == AreaType.Wall && cubeRect.Intersects(tankRect))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanMoveY(Tank tank, int dy)
        {
            if (tank.y - Tank.tankSize / 2 + dy < 0 || tank.y + Tank.tankSize / 2 + dy >= map.mapSizeY)
            {
                return false;
            }
            int d = dy > 0 ? 1 : -1;
            Rectangle tankRect = new Rectangle(tank.x - Tank.tankSize / 2, tank.y - Tank.tankSize / 2 + d, Tank.tankSize, Tank.tankSize);
            foreach (Tank t in tanks)
            {
                if (t == tank)
                {
                    continue;
                }
                Rectangle tRect = new Rectangle(t.x - Tank.tankSize / 2, t.y - Tank.tankSize / 2, Tank.tankSize, Tank.tankSize);
                if (tRect.Intersects(tankRect))
                {
                    return false;
                }
            }
            //это тупо, но мне лень думать что-то нормально
            int cubeY = (tank.y + d * Tank.tankSize/2 + d) / map.mapCubeSIze;
            for (int i = 0; i < map.mass.GetLength(0); i++)
            {
                int y = cubeY * 50;
                int x = i * 50;
                Rectangle cubeRect = new Rectangle(x, y, map.mapCubeSIze, map.mapCubeSIze);
                if (map.mass[i, cubeY] == AreaType.Wall && cubeRect.Intersects(tankRect))
                {
                    return false;
                }
            }

            return true;
        }

        public void Fire(int tankId)
        {
            int x = tanks[tankId].x, y = tanks[tankId].y;
            Orientation or = tanks[tankId].or;
            TeamType t = tanks[tankId].team;
            bullets.Add(new Bullet(x, y, or, t, tanks[tankId]));
        }

        public void Update()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                int dx = 0, dy = 0;
                Bullet b = bullets[i];
                switch (b.or)
                {
                    case Orientation.North:
                        dy = -b.speed;
                        break;
                    case Orientation.South:
                        dy = b.speed;
                        break;
                    case Orientation.West:
                        dx = -b.speed;
                        break;
                    case Orientation.East:
                        dx = b.speed;
                        break;
                }
                bullets[i].x += dx;
                bullets[i].y += dy;

                if(b.x < 0 || b.y < 0 || b.x > map.mapSizeX || b.y > map.mapSizeY)
                {
                    bullets.Remove(b);
                    return;
                }

                int cubeX = (b.x + dx) / map.mapCubeSIze;
                int cubeY = (b.y + dy) / map.mapCubeSIze;
                int y = cubeY * 50;
                int x = cubeX * 50;
                Rectangle cubeRect = new Rectangle(x, y, map.mapCubeSIze, map.mapCubeSIze);
                Rectangle bulletRect = new Rectangle(b.x, b.y, b.sizeX, b.sizeY);
                if ((cubeX >= 0 && cubeX < map.mass.GetLength(0) && cubeY >= 0 && cubeY < map.mass.GetLength(1)) && map.mass[cubeX, cubeY] == AreaType.Wall && cubeRect.Intersects(bulletRect))
                {
                    bullets.Remove(b);
                    return;
                }

                for (int j = 0; j < tanks.Count; j++)
                {
                    Tank t = tanks[j];
                    if(t == b.owner)
                    {
                        continue;
                    }
                    x = t.x;
                    y = t.y;
                    Rectangle tankReact = new Rectangle(x - Tank.tankSize / 2, y - Tank.tankSize / 2, Tank.tankSize, Tank.tankSize);
                    if (t.team == b.team && bulletRect.Intersects(tankReact))
                    {
                        bullets.Remove(b);
                        break;
                    }
                    if (bulletRect.Intersects(tankReact))
                    {
                       
                        t.life -= b.owner.damage;
                        if(t.life <= 0)
                        {
                            tanks.Remove(t);
                        }
                        bullets.Remove(b);
                        break;
                    }
                }
            }
        }

    }
}
