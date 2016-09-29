using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTanks.GameClasses
{
    class Bullet
    {
        public int x, y, sizeX = 10, sizeY = 10, speed = 7;
        public Orientation or;
        public TeamType team;
        public Tank owner;

        public Bullet(int x, int y, Orientation or, TeamType team, Tank owner)
        {
            this.x = x;
            this.y = y;
            this.or = or;
            this.team = team;
            this.owner = owner;
        }
    }
}
