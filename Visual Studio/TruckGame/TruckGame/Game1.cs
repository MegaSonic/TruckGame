#define DEBUG

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

        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public List<GameObject> objectsInScene;
        public List<GameObject> objectsToRemove;

        Texture2D background;

        public GamePadState currentGamePadState;
        public GamePadState previousGamePadState;
        Song bgmusic;

        MouseState currentMouseState;
        MouseState previousMouseState;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

#if (!DEBUG)
            graphics.IsFullScreen = true;
#endif
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
            player = new Player();
            timer = new Timer(this);
            
            objectsInScene.Add(player);
            objectsInScene.Add(timer);

            TouchPanel.EnabledGestures = GestureType.FreeDrag;


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

            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("player_walk");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 31, 44, 2, 300, Color.White, 1f, true );

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Start(this, playerAnimation, playerPosition);
            
            background = Content.Load<Texture2D>("bg_arena");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            foreach (GameObject go in objectsToRemove)
            {
                if (objectsInScene.Contains(go))
                {
                    objectsInScene.Remove(go);
                }
            }

            foreach (GameObject go in objectsInScene)
            {
                go.Update(gameTime);
            }

            CheckCollisions();

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


                    if (possibleCollideable.BoundingBox.Intersects(secondCollideable.BoundingBox))
                    {
                        possibleCollideable.Collided(objectsInScene[j]);
                        secondCollideable.Collided(objectsInScene[i]);
                    }
                    

                }
            }
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
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
            spriteBatch.End();
            

            base.Draw(gameTime);
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
            foreach (GameObject go in objectsInScene)
            {
                if (go.tag == "Timer" || go.tag == "Player") continue;
                else objectsToRemove.Add(go);
            }
        }

        public float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }
    }
}
