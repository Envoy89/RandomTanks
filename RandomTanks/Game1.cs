using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RandomTanks.GameClasses;

namespace RandomTanks
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int playerTankId = 0;
        int tankStep = 5;
        Texture2D tankTextureFirstTeam;
        Texture2D tankTextureSecondTeam;
        Texture2D mapWallArea;
        Texture2D mapRoadArea;
        Texture2D bulletTexture;
        int ko = 0;

        private SpriteFont font;
        private int score = 0;

        Level level1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000; //ширина экрана 
            graphics.PreferredBackBufferHeight = 850; //его высота  
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            level1 = new Level();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tankTextureFirstTeam = Content.Load<Texture2D>("tank1");
            tankTextureSecondTeam = Content.Load<Texture2D>("tank2");
            mapWallArea = Content.Load<Texture2D>("Wall1");
            mapRoadArea = Content.Load<Texture2D>("Road1");
            bulletTexture = Content.Load<Texture2D>("bullet");
            font = Content.Load<SpriteFont>("Score");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            int dx = 0, dy = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                dy = -tankStep;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                dy = tankStep;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                dx = -tankStep;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                dx = tankStep;
            }
            if (dx != 0)
            {
                level1.MoveTankX(playerTankId, dx);
            }
            else if(dy != 0)
            {
                level1.MoveTankY(playerTankId, dy);
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Space) && ko == 0)
            {
                level1.Fire(playerTankId);
                ko = 10;
            }

            if(ko != 0)
            {
                ko--;
            }
            level1.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            AreaType[,] mass = level1.map.mass;
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

            foreach (Tank t in level1.tanks)
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

            for (int i = 0; i < level1.bullets.Count; i++)
            {
                Bullet b = level1.bullets[i];
                spriteBatch.Draw(bulletTexture, new Vector2(b.x, b.y), color: Color.Black);
            }

            string s = string.Format("x = {0} y = {1} z = {2}", level1.tanks[0].x, level1.tanks[0].y, 50);
            spriteBatch.DrawString(font, s, new Vector2(100, 100), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
