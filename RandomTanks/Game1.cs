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
        int tankStep = 3;
        Texture2D tankTextureFirstTeam;
        Texture2D tankTextureSecondTeam;


        Level level1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            // TODO: use this.Content to load your game content here
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

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
