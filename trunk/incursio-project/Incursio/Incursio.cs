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

        //interface components
        SpriteFont font;
        Vector2 FontPos;
        HeadsUpDisplay hud;
        Cursor cursor;
        Button gameMenuButton;
        Button resumeGameButton;
        Button exitGameToMenuButton;
        KeyboardState kbState;  //lets us know if any input is coming in through the keyboard
        Keys[] keysPressed;
        Button newGameButton;
        Button exitGameButton;

        public Incursio()
        {
            graphics = new GraphicsDeviceManager(this);
            hud = new HeadsUpDisplay();
            kbState = new KeyboardState();
            keysPressed = new Keys[15];
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
            gameMenuButton = new Button(new Vector2(465, 738), Content.Load<Texture2D>(@"gameMenuButton"), Content.Load<Texture2D>(@"gameMenuButtonPressed"));
            resumeGameButton = new Button(new Vector2(475, 349), Content.Load<Texture2D>(@"resumeButton"), Content.Load<Texture2D>(@"resumeButtonPressed"));
            exitGameToMenuButton = new Button(new Vector2(475, 384), Content.Load<Texture2D>(@"exitGameButton"), Content.Load<Texture2D>(@"exitGameButtonPressed"));

            //load the menu components
            newGameButton = new Button(new Vector2(400, 638), Content.Load<Texture2D>(@"newGameButton"), Content.Load<Texture2D>(@"newGamePressed"));
            exitGameButton = new Button(new Vector2(524, 638), Content.Load<Texture2D>(@"exitGameButton"), Content.Load<Texture2D>(@"exitGameButtonPressed"));

            //once everything is loaded up, go to the main menu
            currentState = State.GameState.Menu;
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

            cursor.Update();    //update the cursor
            kbState = Keyboard.GetState();  //get the present state of the keyboard
            keysPressed = (Keys[])kbState.GetPressedKeys(); //get all the keys that are being pressed

            //Check game state!
            this.checkState();



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

            drawState();

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
                    
                    //listener for menu button
                    gameMenuButton.Update(cursor, spriteBatch);
                    if (!gameMenuButton.getPressed() && gameMenuButton.getFocus()) //if the menu button is pressed, pause the game
                    {
                        gameMenuButton.setFocus(false);
                        currentState = State.GameState.PausedPlay;
                    }
                    //TODO: perform InPlay actions
                    break;

                case (State.GameState.Menu):

                    //listener for menu button
                    newGameButton.Update(cursor, spriteBatch);
                    exitGameButton.Update(cursor, spriteBatch);

                    if (!newGameButton.getPressed() && newGameButton.getFocus())
                    {
                        newGameButton.setFocus(false);
                        currentState = State.GameState.InPlay;
                    }

                    if (!exitGameButton.getPressed() && exitGameButton.getFocus())    //if exitGameButton is pressed, exit the game
                    {
                        UnloadContent();
                        Exit();                         //exit the game
                    }

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
                    for (int i = 0; i < keysPressed.Length; i++)    //scan through the keys being pressed down
                    {
                        if (keysPressed[i] == Keys.Escape)          //if any are the "Escape" key, go back to playing the game
                        {
                            currentState = State.GameState.InPlay;
                        }
                    }

                    exitGameToMenuButton.Update(cursor, spriteBatch);
                    resumeGameButton.Update(cursor, spriteBatch);

                    if (!resumeGameButton.getPressed() && resumeGameButton.getFocus())
                    {
                        resumeGameButton.setFocus(false);
                        currentState = State.GameState.InPlay;
                    }

                    if (!exitGameToMenuButton.getPressed() && exitGameToMenuButton.getFocus())
                    {
                        exitGameToMenuButton.setFocus(false);
                        currentState = State.GameState.Menu;
                    }

                    //TODO: perform PausedPlay actions
                    break;

                case (State.GameState.None): 
                    break;

                default: break;
            }
        }

        /// <summary>
        /// drawState draws all elements to the screen, depending on which state the game is currently in.
        /// </summary>
        public void drawState()
        {
            switch (this.currentState)
            {
                case (State.GameState.Initializing):
                    spriteBatch.DrawString(font, "Game State: INITIALIZING", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: INITIALIZING") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    //TODO: perform initializing actions
                    break;

                case (State.GameState.InPlay):

                    //draw the HUD
                    hud.drawHeadsUpDisplay(spriteBatch, Window.ClientBounds.Height);

                    //draw the button
                    gameMenuButton.Draw(spriteBatch);

                    //TODO: perform InPlay actions
                    break;

                case (State.GameState.Menu):
                    spriteBatch.DrawString(font, "INCURSIO", FontPos, Color.White, 0, font.MeasureString("INCURSIO") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);
                    
                    newGameButton.Draw(spriteBatch);
                    exitGameButton.Draw(spriteBatch);
                
                    //TODO: perform Menu actions
                    break;

                case (State.GameState.Credits):
                    spriteBatch.DrawString(font, "Game State: CREDITS", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: CREDITS") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    //TODO: perform Credits actions
                    break;

                case (State.GameState.Defeat):
                    spriteBatch.DrawString(font, "Game State: DEFEAT", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: DEFEAT") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    //TODO: perform Defeat actions
                    break;

                case (State.GameState.Victory):
                    spriteBatch.DrawString(font, "Game State: VICTORY", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: VICTORY") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    //TODO: perform Victory actions
                    break;

                case (State.GameState.PausedPlay):
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);

                    spriteBatch.DrawString(font, "Game Paused", new Vector2(520, 100), Color.White, 0, font.MeasureString("Game Paused") / 2, 1.0f, SpriteEffects.None, 0.5f);

                    exitGameToMenuButton.Draw(spriteBatch);
                    resumeGameButton.Draw(spriteBatch);
                 
                    //TODO: perform PausedPlay actions
                    break;

                case (State.GameState.None):
                    spriteBatch.DrawString(font, "Game State: NONE", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: NONE") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    break;

                default: break;
            }
        }
    }
}
