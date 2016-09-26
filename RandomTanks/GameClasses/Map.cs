using System.IO;

namespace RandomTanks.GameClasses
{
    class Map
    {
        public int mapSizeX { get { return 1000; }}
        public int mapSizeY { get { return 850; } }
        public int mapCubeSIze { get { return mapSizeX / 20; } }

        public AreaType[,] mass;

        public Map(string mapFileName)
        {
            int x = mapSizeX / mapCubeSIze;
            int y = mapSizeY / mapCubeSIze;
            mass = new AreaType[x, y];
            string s = "";
            StreamReader file = new StreamReader(mapFileName);
            int i = 0;
            while ((s = file.ReadLine()) != null)
            {
                if (i >= mass.GetLength(1))
                {
                    break;
                }
                string[] l = s.Split(' ');
                int j = 0;
                foreach(string elem in l)
                {
                    if (j >= mass.GetLength(0)) { break; }
                    int ar = int.Parse(elem);
                    mass[j, i] = (AreaType) ar;
                    j++;
                }
                i++;
            }

        }
    }

    enum AreaType { Road, Wall }
}
