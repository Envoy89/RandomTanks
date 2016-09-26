using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTanks.GameClasses
{
    class Map
    {
        public int mapSizeX { get { return 1000; }}
        public int mapSizeY { get { return 850; } }
        public int mapCubeSIze { get { return mapSizeX / 20; } }

        public AreaType[,] mass;

        public Map()
        {
            int x = mapSizeX / mapCubeSIze;
            int y = mapSizeY / mapCubeSIze;
            mass = new AreaType[x, y];
            for (int i = 0; i < mass.GetLength(0); i++)
            {
                if (i >= 2 && i < 5 || i >= 15 && i < 18)
                {
                    mass[i, 2] = AreaType.Wall;
                    mass[i, 12] = AreaType.Wall;
                }
                if (i >= 7 && i < 13)
                {
                    mass[i, 3] = AreaType.Wall;
                    mass[i, 11] = AreaType.Wall;
                }
                if(i >= 8 && i < 10)
                {
                    mass[i, 6] = AreaType.Wall;
                }
                if (i >= 10 && i < 12)
                {
                    mass[i, 9] = AreaType.Wall;
                }
                mass[i, 15] = AreaType.Wall;
            }

            for (int i = 0; i < mass.GetLength(1); i++)
            {
                //
                if (i >= 3 && i < 5 || i >= 10 && i < 12)
                {
                    mass[2, i] = AreaType.Wall;
                    mass[17, i] = AreaType.Wall;
                }
                if (i >= 5 && i < 10)
                {
                    mass[5, i] = AreaType.Wall;
                    mass[14, i] = AreaType.Wall;
                }
            }

            mass[2, 7] = AreaType.Wall;
            mass[17, 7] = AreaType.Wall;
            mass[8, 7] = AreaType.Wall;
            mass[11, 8] = AreaType.Wall;

        }
    }

    enum AreaType { Road, Wall }
}
