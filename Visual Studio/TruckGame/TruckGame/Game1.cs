#define DEBUG

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;



namespace TruckGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public GameState _state;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player player;
        public Timer timer;

        
        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public List<GameObject> objectsInScene;
        public List<GameObject> objectsToRemove;
        public List<GameObject> objectsToAdd;

        Texture2D background;
        Texture2D gsBackground;
        Texture2D goBackground;

        public GamePadState currentGamePadState;
        public GamePadState previousGamePadState;

        Song bgmusic;
        private SoundEffect playerDeath;

        MouseState currentMouseState;
        MouseState previousMouseState;
        
        Button GameStart, GameExit, GameRestart;

        public float initialTruckSpawnTime = 3;
        private float spawnTimer = 0f;
        private int trucksToSpawn = 1;

        private float spawnIncreaseRateTimer = 10f;

        Random rand = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            this.IsMouseVisible = true;
            // graphics.IsFullScreen = true;
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
            objectsInScene = new List<GameObject>();
            objectsToRemove = new List<GameObject>();
            objectsToAdd = new List<GameObject>();
            player = new Player();
            timer = new Timer(this);
            
            objectsInScene.Add(player);
            objectsInScene.Add(timer);

            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            GameStart = new Button("Start", new Vector2(1650, 950));
            GameExit = new Button("Exit", new Vector2(900, 400));
            GameRestart = new Button("Restart", new Vector2(975, 950));

            // I think this always goes last?
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

            
            this.bgmusic = Content.Load<Song>("BG Music");
            MediaPlayer.Play(bgmusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            playerDeath = Content.Load<SoundEffect>("PlayerDeath");

            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("player_walk");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 31, 44, 2, 300, Color.White, 1.5f, true );

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Start(this, playerAnimation, playerPosition);

            GameStart.texture = Content.Load<Texture2D>("start_but");
            GameExit.texture = Content.Load<Texture2D>("esc_but");
            GameRestart.texture = Content.Load<Texture2D>("play_again_but");

            background = Content.Load<Texture2D>("bg_arena");
            gsBackground = Content.Load<Texture2D>("home_bg");
            goBackground = Content.Load<Texture2D>("gameOver_bg");

            // TODO: use this.Content to load your game content here
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(bgmusic);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        protected override void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);


            base.Update(gameTime);
            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;

                case GameState.GamePlay:
                    UpdateGamePlay(gameTime);
                    break;

                case GameState.EndOfGame:
                    UpdateEndOfGame(gameTime);
                    break;

            }
        }

        protected void UpdateMainMenu(GameTime gameTime)
        {
            if ((GameStart.enterButton(currentMouseState) && currentMouseState.LeftButton == ButtonState.Pressed) || (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter)))
            {
                _state = GameState.GamePlay;
                UpdateGamePlay(gameTime);
            }
            //  else if(GameExit.enterButton(currentMouseState) && Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            //  {
            //     Exit();
            // }
        }

        protected void UpdateGamePlay(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnTimer -= deltaTime;
            spawnIncreaseRateTimer -= deltaTime;

            foreach (GameObject go in objectsToRemove)
            {
                if (objectsInScene.Contains(go))
                {
                    objectsInScene.Remove(go);
                }
            }

            objectsToRemove.Clear();

            foreach (GameObject go in objectsToAdd)
            {
                objectsInScene.Add(go);
            }

            objectsToAdd.Clear();

            foreach (GameObject go in objectsInScene)
            {
                go.Update(gameTime);
            }

            CheckCollisions();

            /*
            if (currentKeyboardState.IsKeyDown(Keys.A) && previousKeyboardState.IsKeyUp(Keys.A)) {
                SpawnTruck();
            }
            */

            
            if (spawnTimer < 0f)
            {

                    SpawnTruck();


                double time = rand.NextDouble();
                time *= this.initialTruckSpawnTime;
                spawnTimer = (float) time;

                if (spawnIncreaseRateTimer < 0f)
                {
                    if (rand.NextDouble() < 0.6)
                    {
                        if (initialTruckSpawnTime > 0.5f)
                        {
                            initialTruckSpawnTime -= 0.2f;
                            spawnIncreaseRateTimer = 1f;
                            Debug.WriteLine("Spawn rate incrased");
                        }
                        
                    }
                }
            }
            

                //UpdatePlayer(gameTime);


                base.Update(gameTime);
        }

        public void SpawnTruck()
        {
            Vector2 center = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Vector2 topLeft = new Vector2(0f, 0f);
            Vector2 distance = center - topLeft;

            distance *= 3 / 2;
            Random rand = new Random();
            Matrix rotMatrix = Matrix.CreateRotationZ((float)rand.Next(360));
            distance = Vector2.Transform(distance, rotMatrix);
            Vector2 truckSpawnPoint = center - distance;

            Truck truck = new Truck(this, truckSpawnPoint);
            truck.startPosition = truckSpawnPoint;
            truck.targetPosition = player.Position;
            truck.Rotation = (float)(1 * Math.PI / 2 + VectorToAngle(truck.startPosition - truck.targetPosition));
            objectsToAdd.Add(truck);
        }

        private void CheckCollisions()
        {
            // Detect collisions
            for (int i = 0; i < objectsInScene.Count; i++)
            {
                ICollideable possibleCollideable = objectsInScene[i] as ICollideable;

                if (possibleCollideable == null ) continue;
                if (!possibleCollideable.IsCurrentlyCollideable) continue;

                for (int j = i + 1; j < objectsInScene.Count; j++)
                {
                    
                    ICollideable secondCollideable = objectsInScene[j] as ICollideable;
                    if (secondCollideable == null) continue;
                    if (!secondCollideable.IsCurrentlyCollideable) continue;

                    float distanceBetweenObjects = (float) (Math.Pow(objectsInScene[i].position.X - objectsInScene[j].position.X, 2) + Math.Pow(objectsInScene[i].position.Y - objectsInScene[j].position.Y, 2));
                    float sumOfRadii = (float)(Math.Pow(possibleCollideable.Radius + secondCollideable.Radius, 2));
                    if (distanceBetweenObjects < sumOfRadii)
                    {
                        possibleCollideable.Collided(objectsInScene[j]);
                        secondCollideable.Collided(objectsInScene[i]);
                    }
                    

                }
            }
        }

        protected void UpdateEndOfGame(GameTime gameTime)
        {
            

            if ((GameRestart.enterButton(currentMouseState) && Mouse.GetState().LeftButton == ButtonState.Pressed) || (previousKeyboardState.IsKeyUp(Keys.Enter) && currentKeyboardState.IsKeyDown(Keys.Enter)))
            {
                Reset();
                _state = GameState.GamePlay;

            }
            /*if (GameExit.isInsideCircle(currentMouseState) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Exit();
            }*/
            if(currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                Exit();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);
            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;

                case GameState.GamePlay:
                    DrawGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    DrawEndOfGame(gameTime);
                    break;

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        void DrawMainMenu(GameTime gameTime)
        {            
            spriteBatch.Draw(gsBackground, new Rectangle(0, 0, background.Width, background.Height), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            spriteBatch.Draw(GameStart.texture, new Rectangle((int)GameStart.position.X, (int)GameStart.position.Y, GameStart.texture.Width, GameStart.texture.Height), null, Color.White, 0f, new Vector2(GameStart.texture.Width / 2, GameStart.texture.Height / 2), SpriteEffects.None, 1.0f);
            
        }

        void DrawGameplay(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            // player.Draw(spriteBatch);

            foreach (GameObject go in objectsInScene)
            {                
                go.Draw(spriteBatch);                
            }
                       
        }

        void DrawEndOfGame(GameTime gameTime)
        {
            timer.position = new Vector2(925, 200);
            spriteBatch.Draw(goBackground, new Rectangle(0, 0, background.Width, background.Height), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            //spriteBatch.Draw(GameExit.texture, new Rectangle((int)GameExit.position.X, (int)GameExit.position.Y, GameExit.texture.Width, GameExit.texture.Height), null, Color.White, 0.0f, new Vector2(GameExit.texture.Width / 2, GameStart.texture.Height / 2), SpriteEffects.None, 1.0f);
            spriteBatch.Draw(GameRestart.texture, new Rectangle((int)GameRestart.position.X, (int)GameRestart.position.Y, GameRestart.texture.Width, GameRestart.texture.Height), null, Color.White, 0f, new Vector2(GameRestart.texture.Width / 2, GameStart.texture.Height / 2), SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(timer.font,"Score", new Vector2(timer.position.X - 200, 200), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            
            timer.Draw(spriteBatch);
        }


        public GameObject FindGameObjectByTag(string tag)
        {
            foreach (GameObject go in objectsInScene)
            {
                if (go.tag == tag)
                {
                    return go;
                }
            }

            return null;
        }

        public List<GameObject> FindGameObjectsByTag(string tag)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (GameObject go in objectsInScene)
            {
                if (go.tag == tag)
                {
                    objects.Add(go);
                }
            }

            return objects;
        }

        public void Reset()
        {
            playerDeath.Play();
            _state = GameState.EndOfGame;
            timer.points = 0;
            timer.position = new Vector2(10,10);
            player.Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            initialTruckSpawnTime = 3f;
            spawnTimer = initialTruckSpawnTime;
            trucksToSpawn = 1;
            spawnIncreaseRateTimer = 10f;
            Truck.CrashedTrucks = 0;
            // Figure out how to reset game here
            foreach (GameObject go in objectsInScene)
            {
                if (go.tag == "Timer" || go.tag == "Player" || go.tag == "Bloodstain") continue;
                else objectsToRemove.Add(go);
            }
        }

        public float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }
    }
}
