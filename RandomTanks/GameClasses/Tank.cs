using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTanks.GameClasses
{
    class Tank
    {
        public static int tankSize = 50;
        public static int tankSpeed = 5;
        public int x, y;
        public TeamType team;
        public double life;
        public Orientation or;
        public double damage = 50;

        public Tank(int x, int y, TeamType team, double life)
        {
            this.x = x + Tank.tankSize / 2;
            this.y = y + Tank.tankSize / 2;
            this.team = team;
            this.life = life;
            or = Orientation.North;
        }
    }

    enum TeamType
    {
        FirstTeam, SecondTeam
    }

    enum Orientation { North, South, West, East}
}
