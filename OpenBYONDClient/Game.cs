using System;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using log4net;
using Microsoft.Xna.Framework.Input;

namespace OpenBYOND.Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OpenBYONDGame : Game
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Utils));
        private int view_range = 7; // starting from the tile you're standing on, 7 tiles in each diretion is the default.
        public Mob mob;
        public Camera eye;
        public int VIEWRANGE
        {
            get { return view_range; }
            set
            {
                if (value < 1)
                    value = 1;
                view_range = value;
            }
        }
        public SpriteBatch spriteBatch;
        public int drawCount = 0;
        GraphicsDeviceManager graphics;
        int cdir = 0;
        uint tick = 0;
        Direction[] Wiggle = new[] {
            Direction.WEST,
            Direction.SOUTH,
            Direction.EAST,
            Direction.SOUTH
        };

        public OpenBYONDGame()
        {
            log.Info("BYONDGame Starting.");
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            mob = new Mob(this, new Vector2(0F, 0F));
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);
            IsMouseVisible = true;
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
            eye = new Camera(base.GraphicsDevice.Viewport);
            eye.Pos = mob.Position;
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            log.Info("LoadContent()");
            spriteBatch = new SpriteBatch(GraphicsDevice);
 
            // Create a new SpriteBatch, which can be used to draw textures.
            DMIManager.Preload("TestFiles/human.dmi");
 
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // TODO: Add your update logic here

            // Every 30 ticks...
            // TODO: Make it every 3 seconds (BYOND: 1 tick = 0.1 second)
            if ((tick++%30) == 0)
            {
                cdir = (cdir + 1)%Wiggle.Length;
            }
            mob.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            Texture2D texture1px = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture1px.SetData(new Color[] {Color.Black});
            var adjustedrange = (view_range*2 + 1)*32;
            //GraphicsDevice.Viewport = new Viewport(0,0,adjustedrange,adjustedrange);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, eye.viewMatrix);
            
            var yy = 1;
            var ii = 1;
            string[] states = new string[]{"floor", "white", "plating", "bar"};
            Vector2 drawfield = new Vector2(32 * 32, 32*32);
            for (var i = 1; i <= 10000; i++)
            {
                if (i % 256 == 0)
                {
                    yy++;
                    ii = 1;
                }
                else
                {
                    ii++;
                }
                if (ii > drawfield.X)
                {
                    if (yy > drawfield.Y)
                    {
                        break;
                    }
                    continue;
                }
                //DMIManager.GetSpriteBatch(this, "TestFiles/human.dmi", "fatbody_s", new Vector2(32f, 32f), dir: Wiggle[cdir]);
                //DMIManager.GetSpriteBatch(this, "TestFiles/spacerat.dmi", "rat_brown", new Vector2(64f, 32f), dir: Wiggle[cdir]);
                //DMIManager.DrawSpriteBatch(spriteBatch, this, "TestFiles/robots.dmi", "mommi", new Vector2(32f * (float)ii, 32f * (float)yy), dir: Wiggle[cdir]);
                //DMIManager.DrawSpriteBatch(spriteBatch, this, "TestFiles/robots.dmi", "mommi", new Vector2(32f * (float)ii, 32f * (float)yy), dir: Wiggle[cdir]);
                var rand = new Random();
                int state = rand.Next(states.Length);
                
                DMIManager.DrawSpriteBatch(spriteBatch,this,"TestFiles/floors.dmi","floor",new Vector2(32f * (float)ii, 32f * (float)yy), 0);
                drawCount++;

            }
            for (float x = -300; x < 300; x++)
            {
                Rectangle rectangle = new Rectangle((int)(0 + x * 32), 0, 1, 800);
                spriteBatch.Draw(texture1px, rectangle, Color.Black);
                drawCount++;
            }
            for (float y = -300; y < 300; y++)
            {
                Rectangle rectangle = new Rectangle(0, (int)(0 + y * 32), 800, 1);
                spriteBatch.Draw(texture1px, rectangle, Color.Black);
                drawCount++;
            }
            mob.Draw(spriteBatch,gameTime);
            
            spriteBatch.End();
            //Console.WriteLine("SpriteBatch n = " + n + " had " + drawCount[n] + " calls this draw.");
            drawCount = 0;
            base.Draw(gameTime);
        }
    }
}
