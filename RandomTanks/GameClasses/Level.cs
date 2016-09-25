using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace RandomTanks.GameClasses
{
    class Level
    {
        public Map map;
        public List<Tank> tanks;

        public Level()
        {
            map = new Map();
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
    }
}
