using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace RandomTanks.GameClasses
{
    class Level
    {
        Map map;
        public List<Tank> tanks;

        public Level()
        {
            map = new Map();
            tanks = new List<Tank>();
            tanks.Add(new Tank(0, 0, TeamType.FirstTeam, 100));
            tanks.Add(new Tank(64, 0, TeamType.SecondTeam, 100));
        }

        public void MoveTankX(int tankId, int dx)
        {
            if (CanMoveX(tanks[tankId], dx))
            {
                tanks[tankId].x += dx;
            }
            if (dx < 0)
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
            if (CanMoveY(tanks[tankId], dy))
            {
                tanks[tankId].y += dy;
            }
            if (dy < 0)
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
            if(tank.x + dx < 0 || tank.x + dx > map.mapSizeX)
            {
                return false;
            }
            foreach(Tank t in tanks)
            {
                if (t == tank)
                {
                    continue;
                }
                if ( (t.y > tank.y - Tank.tankSize && t.y < tank.y + Tank.tankSize) && (tank.x + dx > t.x - Tank.tankSize && tank.x + dx < t.x + Tank.tankSize))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanMoveY(Tank tank, int dy)
        {
            if (tank.y + dy < 0 || tank.y + dy > map.mapSizeY)
            {
                return false;
            }
            foreach (Tank t in tanks)
            {
                if (t == tank)
                {
                    continue;
                }
                if ((t.x > tank.x - Tank.tankSize && t.x < tank.x + Tank.tankSize) && (tank.y + dy > t.y - Tank.tankSize && tank.y + dy < t.y + Tank.tankSize))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
