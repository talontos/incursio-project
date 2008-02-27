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
        private static Incursio instance;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;                //draws our images

        //players
        Player computerPlayer;
        Player humanPlayer;

        //Object-map///////////////////
        int nextKeyId;
        List<BaseGameEntity> entityBank;
        ///////////////////////////////

        ObjectFactory factory;

        //unit initialization
        //TODO: **move these to Player class**
        Unit[] selectedUnits;
        int numUnitsSelected;

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

        public Incursio(){

            Incursio.instance = this;

            graphics = new GraphicsDeviceManager(this);
            hud = new HeadsUpDisplay();
            kbState = new KeyboardState();
            keysPressed = new Keys[15];
            Content.RootDirectory = "Content";

            factory = new ObjectFactory(this);

            entityBank = new List<BaseGameEntity>();

            //Just some testing here.  Add a breakpoint after the creations to check it out.
            //TODO: DELETE THESE LINES
            BaseGameEntity test0 = factory.create("Incursio.Classes.Hero");
            BaseGameEntity test1 = factory.create("Incursio.Classes.ControlPoint");
            BaseGameEntity test2 = factory.create("Incursio.Classes.HeavyInfantryUnit");
            BaseGameEntity test3 = factory.create("Incursio.Classes.LightInfantryUnit");
            BaseGameEntity test4 = factory.create("Incursio.Classes.GuardTowerStructure");
            BaseGameEntity test5 = factory.create("Incursio.Classes.CampStructure");
            BaseGameEntity test6 = factory.create("Incursio.Classes.ArcherUnit");

            //TODO: instead have 1 Player[] ??
            computerPlayer = new Player();
            humanPlayer = new Player();

            selectedUnits = new Unit[12];
            numUnitsSelected = 0;

            //set the window size to 1024x768
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
        }

        public static Incursio getInstance(){
            return instance;
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
            hud.loadHeadsUpDisplay(Content.Load<Texture2D>(@"utilityBarUnderlay"), Content.Load<Texture2D>(@"lightInfantryPortrait"), 
                Content.Load<Texture2D>(@"archerPortrait"), Content.Load<Texture2D>(@"infantryIcon"), Content.Load<Texture2D>(@"archerIcon"));

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

                    selectedUnits = hud.update(cursor, selectedUnits, numUnitsSelected);
                    numUnitsSelected = hud.getNumUnits();
                    
                    //listener for menu button
                    gameMenuButton.Update(cursor, spriteBatch);
                    if (!gameMenuButton.getPressed() && gameMenuButton.getFocus()) //if the menu button is pressed, pause the game
                    {
                        gameMenuButton.setFocus(false);
                        currentState = State.GameState.PausedPlay;
                    }

                    //this is just to see if the selected unit properites works.
                    for (int i = 0; i < keysPressed.Length; i++)    //scan through the keys being pressed down
                    {
                        if (keysPressed[i] == Keys.Right)
                        {
                            selectedUnits[0] = new LightInfantryUnit();
                            selectedUnits[0].setDamage(12);
                            selectedUnits[0].setHealth(150);
                            selectedUnits[0].setArmor(5);
                            selectedUnits[0].setPlayer(humanPlayer);
                            selectedUnits[0].setState(State.UnitState.Idle);
                            selectedUnits[0].setLocation(new Coordinate(200, 200));
                            numUnitsSelected = 1;
                        }
                        else if (keysPressed[i] == Keys.Left)
                        {
                            selectedUnits[0] = new ArcherUnit();
                            selectedUnits[0].setDamage(8);
                            selectedUnits[0].setHealth(90);
                            selectedUnits[0].setArmor(2);
                            selectedUnits[0].setPlayer(humanPlayer);
                            selectedUnits[0].setState(State.UnitState.Idle);
                            selectedUnits[0].setLocation(new Coordinate(200, 200));
                            numUnitsSelected = 1;
                        }
                        else if (keysPressed[i] == Keys.Up)
                        {
                            for (int j = 0; j < 6; j++)
                            {
                                selectedUnits[j] = new ArcherUnit();
                                selectedUnits[j].setDamage(8);
                                selectedUnits[j].setHealth(90);
                                selectedUnits[j].setArmor(2);
                                selectedUnits[j].setPlayer(humanPlayer);
                                selectedUnits[j].setState(State.UnitState.Idle);
                                selectedUnits[j].setLocation(new Coordinate(200, 200));
                                numUnitsSelected = 12;
                            }

                            for (int j = 6; j < 12; j++)
                            {
                                selectedUnits[j] = new LightInfantryUnit();
                                selectedUnits[j].setDamage(12);
                                selectedUnits[j].setHealth(150);
                                selectedUnits[j].setArmor(5);
                                selectedUnits[j].setPlayer(humanPlayer);
                                selectedUnits[j].setState(State.UnitState.Idle);
                                selectedUnits[j].setLocation(new Coordinate(200, 200));
                                numUnitsSelected = 12;
                            }
                        }
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
                    hud.draw(spriteBatch, Window.ClientBounds.Height, selectedUnits, font, numUnitsSelected);

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

        public void addEntity(ref BaseGameEntity newEntity){
            newEntity.setKeyId(this.nextKeyId);
            this.entityBank.Insert(this.nextKeyId++, newEntity);
        }

        public BaseGameEntity getEntity(ref int keyId){
            return entityBank[keyId];
        }
    }
}
