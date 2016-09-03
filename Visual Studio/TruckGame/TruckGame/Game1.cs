using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player player;
        public Timer timer;

        Button GameStart, GameExit, GameRestart;

        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public List<GameObject> objectsInScene;

        Texture2D background;
        Texture2D gsBackground;
        Texture2D goBackground;

        public GamePadState currentGamePadState;
        public GamePadState previousGamePadState;
        Song bgmusic;

        MouseState currentMouseState;
        MouseState previousMouseState;

        GameState _state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            //graphics.IsFullScreen = true;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            objectsInScene = new List<GameObject>();
            player = new Player();
            timer = new Timer(this);
            
            objectsInScene.Add(player);
            objectsInScene.Add(timer);
            this.IsMouseVisible = true;
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            GameStart = new Button("Start", 100, 200);
            GameExit = new Button("Exit", 100, 300);
            GameRestart = new Button("Restart", 100, 400);
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
            

            
            this.bgmusic = Content.Load<Song>("BG Music");
            MediaPlayer.Play(bgmusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("player_walk");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 31, 44, 2, 300, Color.White, 1f, true );

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Start(this, playerAnimation, playerPosition);

            GameStart.texture = Content.Load<Texture2D>("monster_truck");
            GameExit.texture = Content.Load<Texture2D>("monster_truck");
            GameRestart.texture = Content.Load<Texture2D>("mario");
            background = Content.Load<Texture2D>("bg_arena");
            gsBackground = Content.Load<Texture2D>("bg_arena");
            goBackground = Content.Load<Texture2D>("bg_arena");

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
        protected override void Update(GameTime gameTime)
        {
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
          if(GameStart.enterButton()&& Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _state = GameState.GamePlay;
            }
          else if(GameExit.enterButton() && Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Exit();
            }
        }

        protected void UpdateGamePlay(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
             //   Exit();

            // TODO: Add your update logic here
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            foreach (GameObject go in objectsInScene)
            {
                go.Update(gameTime);
            }

            if(CheckCollisions())
            {
                _state = GameState.EndOfGame;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Z) && previousKeyboardState.IsKeyUp(Keys.Z))
            {
                Vector2 center = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                Vector2 topLeft = new Vector2(0f, 0f);
                Vector2 distance = center - topLeft;
                
                distance *= 3 / 2;
                Random rand = new Random();
                Matrix rotMatrix = Matrix.CreateRotationZ((float) rand.Next(360));
                distance = Vector2.Transform(distance, rotMatrix);
                Vector2 truckSpawnPoint = center - distance;

                Truck truck = new Truck(this, truckSpawnPoint);
                truck.startPosition = truckSpawnPoint;
                truck.targetPosition = player.Position;
                truck.Rotation = (float) (3 * Math.PI / 2 + VectorToAngle(truck.startPosition - truck.targetPosition));
                objectsInScene.Add(truck);
                //Debug.WriteLine(objectsInScene.Count);
                //Debug.WriteLine("Spawn Point: " + truckSpawnPoint.X + ", " + truckSpawnPoint.Y + "... " + truck.targetPosition.X + " ," + truck.targetPosition.Y);
            }

            //UpdatePlayer(gameTime);


            base.Update(gameTime);
        }

        private Boolean CheckCollisions()
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


                    if (possibleCollideable.BoundingBox.Intersects(secondCollideable.BoundingBox))
                    {
                        if(possibleCollideable.Collided(objectsInScene[j])|| secondCollideable.Collided(objectsInScene[i]))
                        {
                            return true;
                        }
                    }                   

                }
            }
            return false;
        }

        protected void UpdateEndOfGame(GameTime gameTime)
        {
            if(GameRestart.enterButton() && Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _state = GameState.GamePlay;
            }
            else
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
            //base.Draw(gameTime);
            spriteBatch.Begin();
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
            
            //spriteBatch.Begin();
            //spriteBatch.Begin(SpriteSortMode.BackToFront);
            spriteBatch.Draw(gsBackground, new Rectangle(0, 0, background.Width, background.Height), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            spriteBatch.Draw(GameStart.texture, new Rectangle(400, 800, GameStart.texture.Width, GameStart.texture.Height),null,Color.White,0.5f,Vector2.Zero, SpriteEffects.None,1.0f);
            //base.Draw(gameTime);
        }


        void DrawGameplay(GameTime gameTime)
        { 

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront);
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            // player.Draw(spriteBatch);

            foreach (GameObject go in objectsInScene)
            {
                // if (go.tag == "Player") continue;
                go.Draw(spriteBatch);
                
            }

            // player.Draw(spriteBatch);
            
            

            
        }


        void DrawEndOfGame(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);
            spriteBatch.Draw(goBackground, new Rectangle(0, 0, background.Width, background.Height), null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            spriteBatch.Draw(GameExit.texture, new Rectangle(800, 450 + GameStart.texture.Width, GameExit.texture.Width, GameExit.texture.Height), null, Color.White, 0.5f, Vector2.Zero, SpriteEffects.None, 1.0f);
            spriteBatch.Draw(GameRestart.texture, new Rectangle(800, 400, GameStart.texture.Width, GameStart.texture.Height), null, Color.White, 0.5f, Vector2.Zero, SpriteEffects.None, 1.0f);
            //base.Draw(gameTime);
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
            timer.playerTime = 0f;
            player.Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            // Figure out how to reset game here
        }

        public float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }
    }
}
