using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Interface;

namespace Incursio
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Incursio : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;                //draws our images

        //players
        Player computerPlayer;
        Player humanPlayer;

        //game information
        State.GameState currentState = State.GameState.Initializing;

        //interface
        SpriteFont font;
        Vector2 FontPos;
        HeadsUpDisplay hud;
        Cursor cursor;
        GameMenuButton gameMenuButton;

        public Incursio()
        {
            graphics = new GraphicsDeviceManager(this);
            hud = new HeadsUpDisplay();
            Content.RootDirectory = "Content";

            //set the window size to 1024x768
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
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
            font = Content.Load<SpriteFont>("Courier New");

            FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            // create cursor
            cursor = new Cursor(new Vector2(0, 0), Content.Load<Texture2D>(@"cursor"), Content.Load<Texture2D>(@"cursor_click"));

            // load the HUD texture 
            hud.loadHeadsUpDisplay(Content.Load<Texture2D>(@"utilityBarUnderlay"));

            // load paused game menu components
            gameMenuButton = new GameMenuButton(new Vector2(465, 738), Content.Load<Texture2D>(@"gameMenuButton"), Content.Load<Texture2D>(@"gameMenuButtonPressed"),
                Content.Load<Texture2D>(@"resumeButton"), Content.Load<Texture2D>(@"resumeButtonPressed"), 
                Content.Load<Texture2D>(@"exitGameButton"), Content.Load<Texture2D>(@"exitGameButtonPressed"),
                Content.Load<Texture2D>(@"pauseMenu"));

            
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
            // Allows the game to exit
            // this.Exit();

            // TODO: Add your update logic here

            //Check game state!
            this.checkState();

            cursor.Update();
            gameMenuButton.Update(cursor, spriteBatch);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            //draw the HUD
            hud.drawHeadsUpDisplay(spriteBatch, Window.ClientBounds.Height);

            //draw the button
            gameMenuButton.Draw(spriteBatch);

            //this is how to draw simple text onto the screen
            //spriteBatch.DrawString(font, "hello world", FontPos, Color.DarkBlue, 0, font.MeasureString("hello world") / 2, 1.0f, SpriteEffects.None, 0.5f);

            //draw the cursor
            cursor.Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// This function checks the currentState value agains the State.GameState enums
        /// 
        /// It will also perform the neccessary computations dependent upon the state
        /// </summary>
        private void checkState(){
            switch(this.currentState){
                case (State.GameState.Initializing): 
                    //TODO: perform initializing actions
                    break;

                case (State.GameState.InPlay):
                    //TODO: perform InPlay actions
                    break;

                case (State.GameState.Menu):
                    //TODO: perform Menu actions
                    break;

                case (State.GameState.Credits):
                    //TODO: perform Credits actions
                    break;

                case (State.GameState.Defeat):
                    //TODO: perform Defeat actions
                    break;

                case (State.GameState.Victory):
                    //TODO: perform Victory actions
                    break;

                case (State.GameState.PausedPlay):
                    //TODO: perform PausedPlay actions
                    break;

                case (State.GameState.None): 
                    break;

                default: break;
            }
        }
    }
}
