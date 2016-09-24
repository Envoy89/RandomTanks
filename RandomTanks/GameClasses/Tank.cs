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
        public int x, y;
        public TeamType team;
        double life;
        public Orientation or; 

        public Tank(int x, int y, TeamType team, double life)
        {
            this.x = x;
            this.y = y;
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
