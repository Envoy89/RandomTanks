using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace RandomTanks.GameClasses
{
    class Level
    {
        public Map map;
        public List<Tank> tanks;
        public List<Bullet> bullets;
        public int playerScore = 0;
        Random rand;
        public bool gameOver = false;
        private const int koConst = 200;
        private int ko = koConst;

        Texture2D tankTextureFirstTeam;
        Texture2D tankTextureSecondTeam;
        Texture2D mapWallArea;
        Texture2D mapRoadArea;
        Texture2D bulletTexture;

        public Level(string mapFileName)
        {
            map = new Map(mapFileName);
            bullets = new List<Bullet>();
            tanks = new List<Tank>();
            rand = new Random();
            tanks.Add(new Tank(9 * 50, 5 * 50, TeamType.FirstTeam, 100, rand));
            tanks.Add(new Tank(3*50, 50, TeamType.FirstTeam, 100, rand));
            tanks.Add(new Tank(7*50, 50, TeamType.FirstTeam, 100, rand));
            tanks.Add(new Tank(12*50, 50, TeamType.FirstTeam, 100, rand));
            tanks.Add(new Tank(16*50, 50, TeamType.FirstTeam, 100, rand));

            tanks.Add(new Tank(10 * 50, 10 * 50, TeamType.SecondTeam, 100, rand));
            tanks.Add(new Tank(3 * 50, 13 * 50, TeamType.SecondTeam, 100, rand));
            tanks.Add(new Tank(7 * 50, 13 * 50, TeamType.SecondTeam, 100, rand));
            tanks.Add(new Tank(12 * 50, 13 * 50, TeamType.SecondTeam, 100, rand));
            tanks.Add(new Tank(16 * 50, 13 * 50, TeamType.SecondTeam, 100, rand));
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
            if(tanks[tankId].fireDown != 0) { return; }
            else { tanks[tankId].fireDown = 50; }
            int x = tanks[tankId].x, y = tanks[tankId].y;
            Orientation or = tanks[tankId].or;
            TeamType t = tanks[tankId].team;
            bullets.Add(new Bullet(x, y, or, t, tanks[tankId]));
        }

        public void Update()
        {
            if(ko == 0)
            {
                RandomTeam();
                ko = koConst;
            }
            else { ko--; }
            for (int i = 1; i < tanks.Count; i++)
            {
                tankAct(i);
                if(tanks[i].fireDown != 0) { tanks[i].fireDown--; }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                //двигаем пулю
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
                //проверяем выход пули за границы
                if(b.x < 0 || b.y < 0 || b.x > map.mapSizeX || b.y > map.mapSizeY)
                {
                    bullets.Remove(b);
                    return;
                }
                //проверяем столкновение пули со стенами
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
                //проверяем столкновение пули с танками
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
                            if (j == 0)
                            {
                                gameOver = true;
                            }
                            if (b.owner == tanks[0])
                            {
                                playerScore += 1;
                            }
                        }
                        bullets.Remove(b);
                        break;
                    }
                }
            }
        }

        public void LoadContent(Texture2D tankTextureFirstTeam, Texture2D tankTextureSecondTeam, Texture2D mapWallArea, Texture2D mapRoadArea, Texture2D bulletTexture)
        {
            this.tankTextureFirstTeam = tankTextureFirstTeam;
            this.tankTextureSecondTeam = tankTextureSecondTeam;
            this.mapWallArea = mapWallArea;
            this.mapRoadArea = mapRoadArea;
            this.bulletTexture = bulletTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            AreaType[,] mass = map.mass;
            for (int i = 0; i < mass.GetLength(0); i++)
            {
                for (int j = 0; j < mass.GetLength(1); j++)
                {
                    if (mass[i, j] == AreaType.Wall)
                    {
                        spriteBatch.Draw(mapWallArea, new Vector2(i * 50, j * 50), color: Color.White);
                    }
                    else if (mass[i, j] == AreaType.Road)
                    {
                        spriteBatch.Draw(mapRoadArea, new Vector2(i * 50, j * 50), color: Color.White);
                    }
                }
            }

            foreach (Tank t in tanks)
            {
                float rotation = 0;
                Texture2D texture = null;
                switch (t.or)
                {
                    case Orientation.East:
                        rotation = 1.5708f;
                        break;
                    case Orientation.South:
                        rotation = 3.14159f;
                        break;
                    case Orientation.West:
                        rotation = 4.71239f;
                        break;
                    default:
                        rotation = 0;
                        break;
                }
                switch (t.team)
                {
                    case TeamType.FirstTeam:
                        texture = tankTextureFirstTeam;
                        break;
                    case TeamType.SecondTeam:
                        texture = tankTextureSecondTeam;
                        break;
                }

                spriteBatch.Draw(texture, new Vector2(t.x, t.y), rotation: rotation, origin: new Vector2((Tank.tankSize / 2), (Tank.tankSize / 2)), color: Color.White);
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                spriteBatch.Draw(bulletTexture, new Vector2(b.x, b.y), color: Color.Black);
            }
        }

        private void tankAct(int tankId)
        {
            Tank tank = tanks[tankId];
            if(tank.fireDown == 0 && IsFire(tank))
            {
                Fire(tankId);
                return;
            }
            int dx = 0, dy = 0;
            switch (tank.or)
            {
                case Orientation.North:
                    dy = -Tank.tankSpeed;
                    break;
                case Orientation.South:
                    dy = Tank.tankSpeed;
                    break;
                case Orientation.West:
                    dx = -Tank.tankSpeed;
                    break;
                case Orientation.East:
                    dx = Tank.tankSpeed;
                    break;
            }
            if (dx != 0)
            {
                MoveTankX(tankId, dx);
                if (!CanMoveX(tank, dx))
                {
                    int n = rand.Next(0, 4);
                    tank.or = (Orientation)n;
                }
            }
            else if (dy != 0)
            {
                MoveTankY(tankId, dy);
                if (!CanMoveY(tank, dy))
                {
                    int n = rand.Next(0, 4);
                    tank.or = (Orientation)n;
                }
            }
        }

        private bool IsFire(Tank tank)
        {
            foreach(Tank t in tanks)
            {
                if (t.team == tank.team) { continue; }
                bool df = false;
                if(tank.x >= t.x - Tank.tankSize / 2 && tank.x <= t.x + Tank.tankSize / 2)
                {
                    int d = tank.y > t.y ? -1 : 1;
                    for (int j = tank.y; j != t.y; j += d)
                    {
                        int cubeY = j / map.mapCubeSIze;
                        int cubeX = tank.x / map.mapCubeSIze;
                        if (map.mass[cubeX, cubeY] == AreaType.Wall)
                        {
                            df = true;
                        }
                    }
                    if (df) { continue; }
                    if(tank.y > t.y)
                    {
                        tank.or = Orientation.North;
                        return true;
                    }
                    else
                    {
                        tank.or = Orientation.South;
                        return true;
                    }
                }
                else if(tank.y >= t.y - Tank.tankSize / 2 && tank.y <= t.y + Tank.tankSize / 2 )
                {
                    int d = tank.x > t.x ? -1 : 1;
                    for (int j = tank.x; j != t.x; j += d)
                    {
                        int cubeY = tank.y / map.mapCubeSIze;
                        int cubeX = j / map.mapCubeSIze;
                        if (map.mass[cubeX, cubeY] == AreaType.Wall) { df = true; }
                    }
                    if (df) { continue; }
                    if (tank.x > t.x)
                    {
                        tank.or = Orientation.West;
                        return true;
                    }
                    else
                    {
                        tank.or = Orientation.East;
                        return true;
                    }
                }
            }
            return false;
        }

        private void RandomTeam()
        {
            int firstTeamCount = 0, secondTeamCount = 0;
            foreach (var tank in tanks)
            {
                if(tank.team == TeamType.FirstTeam)
                {
                    firstTeamCount++;
                }
                else
                {
                    secondTeamCount++;
                }
            }
            for (int i = 0; i < tanks.Count; i++)
            {
                tanks[i].team = TeamType.SecondTeam;
            }
            HashSet<int> t = new HashSet<int>();
            for (int i = 0; i < firstTeamCount; i++)
            {
                int id = rand.Next(tanks.Count);
                while (t.Contains(id))
                {
                    id = rand.Next(tanks.Count);
                }
                tanks[id].team = TeamType.FirstTeam;
                t.Add(id);
            }

        }
    }
}
