using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RandomTanks.GameClasses;
using System.Collections.Generic;

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
        int score = 0;
        int ko = 0;
        int ko2 = 0;
        int levelCount = 1;

        private SpriteFont font;
        private SpriteFont bigfont;

        Level level;
        List<Level> levels;
        GameState currentGameState = GameState.MainMenu;

        MenuButton btnPlay;
        MenuButton btnExit;
        MenuButton btnContinue;

        enum GameState { MainMenu, PlayingGame, Payse, NextLevel }

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
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Texture2D btnPlayTexture = Content.Load<Texture2D>("PlayButton");
            Texture2D btnExitTexture = Content.Load<Texture2D>("ExitButton"); 
            Texture2D btnContinueTexture = Content.Load<Texture2D>("ContinueButton");
            font = Content.Load<SpriteFont>("Score");
            bigfont = Content.Load<SpriteFont>("BigText");

            btnPlay = new MenuButton(btnPlayTexture, GraphicsDevice);
            btnPlay.setPosition(new Vector2(400, 200));
            btnExit = new MenuButton(btnExitTexture, GraphicsDevice);
            btnExit.setPosition(new Vector2(400, 250));
            btnContinue = new MenuButton(btnContinueTexture, GraphicsDevice);
            btnContinue.setPosition(new Vector2(400, 150));
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
            {
                currentGameState = GameState.Payse;
                btnPlay.isClicked = false;
            }

            MouseState mouse = Mouse.GetState();

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    IsMouseVisible = true;
                    if(btnPlay.isClicked)
                    {
                        NewGame();
                        currentGameState = GameState.PlayingGame;
                    }
                    if(btnExit.isClicked) { Exit(); }
                    btnPlay.Update(mouse);
                    btnExit.Update(mouse);
                    break;
                case GameState.PlayingGame:
                    IsMouseVisible = false;
                    GameLevelUpdate();
                    break;
                case GameState.Payse:
                    IsMouseVisible = true;
                    if (btnContinue.isClicked) { currentGameState = GameState.PlayingGame; }
                    if (btnPlay.isClicked) { NewGame(); currentGameState = GameState.PlayingGame; }
                    if (btnExit.isClicked) { Exit(); }
                    btnPlay.Update(mouse);
                    btnExit.Update(mouse);
                    btnContinue.Update(mouse);
                    break;
                case GameState.NextLevel:
                    if(ko2 == 0 && IsGameOver()) { btnPlay.isClicked = false; currentGameState = GameState.MainMenu; }
                    else if (ko2 == 0) { currentGameState = GameState.PlayingGame; }
                    else { ko2--; }
                    break;
            }

            base.Update(gameTime);
        }

        private void GameLevelUpdate()
        {
            if (!IsGameOver())
            {
                int dx = 0, dy = 0, tankStep = Tank.tankSpeed;
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
                    level.MoveTankX(playerTankId, dx);
                }
                else if (dy != 0)
                {
                    level.MoveTankY(playerTankId, dy);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && ko == 0)
                {
                    level.Fire(playerTankId);
                    level.tanks[playerTankId].fireDown = 5;
                }

                if (level.tanks[playerTankId].fireDown != 0)
                {
                    level.tanks[playerTankId].fireDown--;
                }

                level.Update();
                score = level.playerScore;
            }

            if (IsLevelOver() && !IsGameOver()) 
            {
                NextLevel();
            }
            if (IsGameOver())
            {
                ko2--;
            }
            if (ko2 == 0 && IsGameOver())
            {
                currentGameState = GameState.MainMenu;
                btnPlay.isClicked = false;
            }
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

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Menu2"), new Rectangle(0,0,graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    break;
                case GameState.PlayingGame:
                    if (ko2 == 0)
                    {
                        level.Draw(spriteBatch);
                        string s = string.Format("Level: {2} Score: {0}   Life: {1}", score, level.tanks[playerTankId].life, levelCount);
                        spriteBatch.DrawString(font, s, new Vector2(800, 820), Color.Black);
                    }
                    else
                    {
                        string se2 = string.Format("You lose\nYour scope: {0}", score);
                        spriteBatch.Draw(Content.Load<Texture2D>("fon"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                        spriteBatch.DrawString(bigfont, se2, new Vector2(400, 300), Color.Black);
                    }
                    break;
                case GameState.Payse:
                    spriteBatch.Draw(Content.Load<Texture2D>("Menu2"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    btnContinue.Draw(spriteBatch);
                    break;
                case GameState.NextLevel:
                    string se = string.Format("You win\nYour scope: {0}", score); 
                    spriteBatch.Draw(Content.Load<Texture2D>("fon"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    spriteBatch.DrawString(bigfont, se, new Vector2(400, 300), Color.Black);
                    break;
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void NewGame()
        {
            levels = new List<Level>();
            level = new Level(@"Content\firstMap.txt");
            levels.Add(level);
            levels.Add(new Level(@"Content\secondMap.txt"));
            levels.Add(new Level(@"Content\thirdMap.txt"));
            levels.Add(new Level(@"Content\secondMap.txt"));
            levels.Add(new Level(@"Content\firstMap.txt"));
            levels.Add(new Level(@"Content\fourthMap.txt"));
            levels.Add(new Level(@"Content\secondMap.txt"));
            levels.Add(new Level(@"Content\firstMap.txt"));
            levels.Add(new Level(@"Content\fifthMap.txt"));
            LoadLevelContent();

            playerTankId = 0;
            score = 0;
            ko = 0;
            ko2 = 0;
            levelCount = 1;
        }

        private void LoadLevelContent()
        {
            Texture2D tankTextureFirstTeam = Content.Load<Texture2D>("tank13");
            Texture2D tankTextureSecondTeam = Content.Load<Texture2D>("tank23");
            Texture2D mapWallArea = Content.Load<Texture2D>("Wall12");
            Texture2D mapRoadArea = Content.Load<Texture2D>("Road1");
            Texture2D bulletTexture = Content.Load<Texture2D>("bullet");

            level.LoadContent(tankTextureFirstTeam, tankTextureSecondTeam, mapWallArea, mapRoadArea, bulletTexture);
        }

        private bool IsLevelOver()
        {
            bool f = true;
            TeamType team = level.tanks[0].team;
            foreach(Tank t in level.tanks)
            {
                if(t.team != team) { f = false; break; }
            }
            return f;
        }

        private bool IsGameOver()
        {
            if((IsLevelOver() && levels.Count == 0) || level.gameOver)
            {
                if (ko2 == 0) { ko2 = 50; }
                return true;
            }
            return false;
        }

        private void NextLevel()
        {
            levels.Remove(level);
            ko2 = 100;
            currentGameState = GameState.NextLevel;
            if(levels.Count != 0)
            {
                level = levels[0];
                LoadLevelContent();
                levelCount++;
            }
        }
    }
}
