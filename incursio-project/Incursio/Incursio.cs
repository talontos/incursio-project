/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

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

using System.Xml;


using Incursio.Utils;
using Incursio.Interface;
using Incursio.Managers;

using IrrKlang;
using Incursio.Interface.Menus;
using Incursio.World;
using Incursio.Entities;

namespace Incursio
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Incursio : Microsoft.Xna.Framework.Game
    {
        private static Incursio instance;
        //UNCOMMENT THE RANDOM-NUMBER-SEED FOR DEBUGGING RANDOM BEHAVIOR
        public static Random rand = new Random(/*DebugUtil.RandomNumberSeed*/);

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;                //draws our images

        public MapBase currentMap;

        //Hero "shell" for storing hero attributes between games
        private BaseGameEntity hero;

        //game information
        public State.GameState currentState
        {
            get { return this.state; }
            set { this.state = value; }
        }
        private State.GameState state = State.GameState.Initializing;

        private GameResult gameResult = new GameResult();

        public GameResult GameResult{
            get {
                return gameResult;
            }

            set {
                if (value.resultState != State.GameState.None)
                    currentState = value.resultState;

                gameResult = value;
                
            }
        }
        public GameResult result = new GameResult();

        //interface components
        SpriteFont font;
        Vector2 FontPos;
        public HeadsUpDisplay hud;
        Cursor cursor;
        KeyboardState kbState;  //lets us know if any input is coming in through the keyboard
        Keys[] keysPressed;
        Button gameMenuButton;
        Texture2D clickDestination;
        int clickDestinationFader = 0;
        Coordinate cursorAtClick;

        MenuSet mainMenu;
        MenuSet mapSelectionMenu;
        MenuSet loadMenu;
        MenuSet saveMenu;
        MenuSet pauseMenu;
        MenuSet instructionsMenu;
        MenuSet creditsMenu;

        private string loadStatus = "Initializing";
        private string prevLoadStatus = "";
        private int loadIterator = -1;
        private bool game_loaded = false;

        //for game animation
        int frameTimer = 0;
        const int FRAME_LENGTH = 8;

        public Incursio(){

            Incursio.instance = this;

            loadStatus = "Initializing Incursio";
            graphics = new GraphicsDeviceManager(this);
            hud = new HeadsUpDisplay();
            kbState = new KeyboardState();
            keysPressed = new Keys[15];
            Content.RootDirectory = "Content";

            this.Components.Add(new GamerServicesComponent(this));
            
            //TODO: Make this more general
            //set the window size to 1024x768
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
        }

        public static Incursio getInstance(){
            return instance;
        }

        public void toggleFullScreen(){
            this.graphics.ToggleFullScreen();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// 
        /// Loads simple neccessary content
        /// </summary>
        protected override void LoadContent()
        {
            //Load images into textureBank
            TextureManager.initializeTextureManager(Content);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Courier New");
            FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
        }

        /// <summary>
        /// Loads all configurable game content and menus
        /// </summary>
        protected void LoadIncursioContent(){
            switch(this.loadIterator){
                case -1:
                    break;
                case 0:
                    loadStatus = "Loading Game Configuration";
                    FileManager.getInstance().loadGameConfiguration();
                break;

                case 1:
                    loadStatus = "Setting Game Font";
                    MessageManager.getInstance().setFont(Content.Load<SpriteFont>("Arial"));
                    break;

                case 2:
                    loadStatus = "Initializing Sound Manager";
                    SoundManager.getInstance().initializeSoundManager();
                    break;

                case 3:
                    // create cursor
                    loadStatus = "Initializing Cursor";
                    cursor = Cursor.getInstance();
                    clickDestination = Content.Load<Texture2D>(@"destinationClick");
                    break;

                case 4:
                    // load the HUD
                    loadStatus = "Loading Heads Up Display";
                    hud.loadHeadsUpDisplay();
                    break;

                case 5:
                    // load paused game menu components
                    loadStatus = "Building Menus";
                    this.gameMenuButton = new Button(new Vector2(465, 738), TextureBank.getInstance().InterfaceTextures.interfaceTextures.gameMenuButton.texture, TextureBank.getInstance().InterfaceTextures.interfaceTextures.gameMenuButtonPressed.texture);
                    Button resumeGameButton = new ResumeGameButton();
                    Button exitGameToMenuButton = new ExitGameToMenuButton();

                    Button saveButton = new SaveMenuButton();
                    Button loadButton = new LoadMenuButton();

                    //load the menu components
                    Button mapSelectButton = new MapSelectButton();
                    Button newGameButton_level1 = new NewGameButton(State.CampaignLevel.ONE);
                    Button newGameButton_level2 = new NewGameButton(State.CampaignLevel.TWO);
                    Button newGameButton_level3 = new NewGameButton(State.CampaignLevel.THREE);
                    Button exitGameButton = new ExitGameButton();
                    Button creditsButton = new CreditsButton();

                    //create menus
                    mainMenu = new MenuSet(600, 500, mapSelectButton, loadButton, new InstructionButton(), creditsButton, exitGameButton);
                    mapSelectionMenu = new MenuSet(600, 500, newGameButton_level1, newGameButton_level2, newGameButton_level3, new MainMenuButton());
                    pauseMenu = new MenuSet(600, 500, resumeGameButton, saveButton, exitGameToMenuButton);
                    loadMenu = new MenuSet(600, 500, new LoadButton("Hero1.wtf", 1), new LoadButton("Hero2.wtf", 2), new LoadButton("Hero3.wtf", 3), new MainMenuButton());
                    saveMenu = new MenuSet(600, 500, new SaveButton("Hero1.wtf", 1), new SaveButton("Hero2.wtf", 2), new SaveButton("Hero3.wtf", 3), new MainMenuButton());
                    instructionsMenu = new MenuSet(800, 400, new CreditsButton(), new MainMenuButton());
                    creditsMenu = new MenuSet(0, 0, new MainMenuButton());
                    break;

                default:
                    //once everything is loaded up, go to the main menu
                    currentState = State.GameState.Menu;
                    loadStatus = "Content Loaded";
                    this.game_loaded = true;
                    break;
            }

            this.loadIterator++;
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
            if(this.game_loaded){
                cursor.Update();    //update the cursor

                //TODO: Remove these 2 lines; this should be done in the InputManager
                kbState = Keyboard.GetState();  //get the present state of the keyboard
                keysPressed = (Keys[])kbState.GetPressedKeys(); //get all the keys that are being pressed

            }
            else{
                if (loadStatus != prevLoadStatus){
                    Console.WriteLine(loadStatus);
                    prevLoadStatus = loadStatus;
                }
            }

            //Check game state!
            this.checkState(gameTime);
            this.playStateSounds();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

            if (frameTimer > FRAME_LENGTH)
            {
                frameTimer = 0;
            }
            else
                frameTimer++;

            base.Draw(gameTime);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            drawState();

            //draw the cursor
            if(game_loaded)
                cursor.Draw(spriteBatch);

            //draw text, if needed
            InputManager.getInstance().Draw(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// This function checks the currentState value agains the State.GameState enums
        /// 
        /// It will also perform the neccessary computations dependent upon the state
        /// </summary>
        private void checkState(GameTime gameTime){

            switch(this.currentState){
                case (State.GameState.Initializing):
                    if (!this.game_loaded)
                        this.LoadIncursioContent();
                    break;

                case (State.GameState.InPlay):

                    hud.update(cursor);

                    //update entities
                    EntityManager.getInstance().updateAllEntities(gameTime);

                    MapManager.getInstance().currentMap.update();

                    PlayerManager.getInstance().updatePlayers(gameTime);

                    MessageManager.getInstance().update();

                    //listener for menu button
                    gameMenuButton.Update(cursor);
                    if (!gameMenuButton.getPressed() && gameMenuButton.getFocus()) //if the menu button is pressed, pause the game
                    {
                        gameMenuButton.setFocus(false);
                        currentState = State.GameState.PausedPlay;
                    }

                    //update campaign conditions
                    MapManager.getInstance().UpdateCampaign(gameTime);

                    break;

                case (State.GameState.Menu):
                    mainMenu.Update(cursor);
                    break;

                case State.GameState.MapSelection:
                    mapSelectionMenu.Update(cursor);
                    break;

                case (State.GameState.LoadMenu):
                    loadMenu.Update(cursor);

                    break;

                case (State.GameState.SaveMenu):
                    saveMenu.Update(cursor);

                    break;

                case (State.GameState.Credits):
                    creditsMenu.Update(cursor);
                    break;

                case (State.GameState.Defeat):
                    mainMenu.Update(cursor);
                    try{
                        this.hero = EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0];
                    }
                    catch(Exception e){}

                    break;

                case (State.GameState.Victory):
                    mainMenu.Update(cursor);
                    try
                    {
                        this.hero = EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0];
                    }
                    catch (Exception e) { }

                    break;

                case (State.GameState.PausedPlay):
                    pauseMenu.Update(cursor);
                    
                    break;

                case (State.GameState.Instructions):
                    instructionsMenu.Update(cursor);
                    break;

                case (State.GameState.None): 
                    break;

                default: break;
            }

            //update input regardless; this update checks for State to determine what to update
            InputManager.getInstance().Update(gameTime);
        }

        /// <summary>
        /// drawState draws all elements to the screen, depending on which state the game is currently in.
        /// </summary>
        public void drawState()
        {
            string stateString = this.gameResult.result;
            switch (this.currentState)
            {
                case (State.GameState.Initializing):
                    spriteBatch.DrawString(font, "Game State: INITIALIZING: " + this.loadStatus, FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: INITIALIZING: " + this.loadStatus) / 2, 1.0f, SpriteEffects.None, 0.5f);
                    break;

                case (State.GameState.InPlay):
                    //draw the map
                    currentMap.draw(spriteBatch, cursor);

                    //draw units
                    drawEntity();

                    if (cursor.getIsRightPressed())
                    {
                        cursorAtClick = new Coordinate((int)cursor.getPos().X, (int)cursor.getPos().Y);
                        mouseClickDestination();
                    }

                    //draw selection rectangle
                    InputManager.getInstance().drawSelectionRectangle(ref spriteBatch);

                    //draw the HUD
                    hud.draw(spriteBatch, Window.ClientBounds.Height, font);

                    //draw the button
                    gameMenuButton.Draw(spriteBatch);

                    //display any messages that need to be shown
                    MessageManager.getInstance().displayMessages(spriteBatch);

                    break;

                ////////////////////////////////////////////////

                case (State.GameState.Menu):
                    //spriteBatch.DrawString(font, "INCURSIO", FontPos, Color.White, 0, font.MeasureString("INCURSIO") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.mainMenuBackground.texture, 
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    mainMenu.Draw(spriteBatch);

                    break;

                case (State.GameState.LoadMenu):
                    //spriteBatch.DrawString(font, "INCURSIO", FontPos, Color.White, 0, font.MeasureString("INCURSIO") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.mainMenuBackground.texture,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    loadMenu.Draw(spriteBatch);

                    break;

                case (State.GameState.SaveMenu):
                    //spriteBatch.DrawString(font, "INCURSIO", FontPos, Color.White, 0, font.MeasureString("INCURSIO") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.pauseMenuBackground.texture,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    saveMenu.Draw(spriteBatch);

                    break;

                case State.GameState.MapSelection:
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.mainMenuBackground.texture,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    mapSelectionMenu.Draw(spriteBatch);

                    break;

                case (State.GameState.Credits):
                    //spriteBatch.DrawString(font, "Game State: CREDITS", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: CREDITS") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.creditsBackground.texture, new Rectangle(0, 0, 1024, 768), Color.White);
                    creditsMenu.Draw(spriteBatch);
                    break;

                case (State.GameState.Defeat):
                    //spriteBatch.DrawString(font, "Game State: Massive Failure! " + stateString, FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: Massive Failure! " + stateString) / 2, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.defeatMenuBackground.texture,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    mainMenu.Draw(spriteBatch);

                    break;

                case (State.GameState.Victory):
                    //spriteBatch.DrawString(font, "Game State: Epic Win!!! " + stateString, FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: Epic Win!!! " + stateString) / 2, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.victoryMenuBackground.texture,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    mainMenu.Draw(spriteBatch);
                    break;

                case (State.GameState.PausedPlay):
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);

                    //spriteBatch.DrawString(font, "Game Paused", new Vector2(520, 100), Color.White, 0, font.MeasureString("Game Paused") / 2, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.pauseMenuBackground.texture,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
                    
                    pauseMenu.Draw(spriteBatch);
                    break;

                case (State.GameState.Instructions):
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.instructions.texture,
                       new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    instructionsMenu.Draw(spriteBatch);
                    break;

                case (State.GameState.None):
                    spriteBatch.DrawString(font, "Game State: NONE", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: NONE") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    break;

                default: break;
            }
        }

        /// <summary>
        /// Plays appropriate background sounds per state
        /// </summary>
        public void playStateSounds(){

            switch(currentState){
                case State.GameState.Menu:
                case State.GameState.LoadMenu:
                case State.GameState.SaveMenu:
                case State.GameState.MapSelection:
                    SoundManager.getInstance().PlayBGMusic(SoundManager.getInstance().AudioCollection.ambience.main_menu);
                    break;

                case State.GameState.InPlay:
                case State.GameState.PausedPlay:
                    SoundManager.getInstance().PlayBGMusic(SoundManager.getInstance().AudioCollection.ambience.inPlay);
                    break;

                case State.GameState.Victory:
                    SoundManager.getInstance().PlayBGMusic(SoundManager.getInstance().AudioCollection.ambience.victory);
                    break;

                case State.GameState.Defeat:
                    SoundManager.getInstance().PlayBGMusic(SoundManager.getInstance().AudioCollection.ambience.defeat);
                    break;

                default:
                    break;

            }
        }

        /// <summary>
        /// drawEntity() goes through the entityBank and draws all entities that are presently on the screen
        /// </summary>
        public void drawEntity()
        {
            //draw all visible units
            EntityManager.getInstance().getAllEntities().ForEach(delegate(BaseGameEntity e)
            {
                if (e.currentState == State.EntityState.Buried)
                {}
                else if (currentMap.isOnScreen(e.getLocation()))
                {
                    e.drawThyself(ref spriteBatch, frameTimer, FRAME_LENGTH);
                }

            });

            //show selection overlay on all visible selected units
            EntityManager.getInstance().getSelectedUnits().ForEach(delegate(BaseGameEntity u)
            {
                if(currentMap.isOnScreen(u.getLocation())){
                    u.renderComponent.drawSelectionOverlay(ref spriteBatch);
                }
            });
        }

        public void mouseClickDestination()
        {
            bool fadeOut = false;

            if (!fadeOut)
            {
                spriteBatch.Draw(clickDestination, new Rectangle(cursorAtClick.x - (clickDestination.Width / 2), cursorAtClick.y - (int)(clickDestination.Height * 0.5626), clickDestination.Width, clickDestination.Height),
                    new Color(255, 255, 255, (byte)(clickDestinationFader + 26)));

                if (clickDestinationFader > 225)
                {
                    fadeOut = true;
                }
            }
            else
            {
                spriteBatch.Draw(clickDestination, new Rectangle(cursorAtClick.x - (clickDestination.Width / 2), cursorAtClick.y - (int)(clickDestination.Height * 0.5626), clickDestination.Width, clickDestination.Height),
                    new Color(255, 255, 255, (byte)(clickDestinationFader - 26)));
            }

        }

        public void pause_play(){
            switch(currentState){
                case State.GameState.InPlay:        currentState = State.GameState.PausedPlay; break;
                case State.GameState.PausedPlay:    currentState = State.GameState.InPlay; break;
            }
        }

        public void resetManagers(){

        }

        public void setHero(BaseGameEntity h){
            this.hero = h;
        }

        public BaseGameEntity getHero(){
            return this.hero;
        }

        public void exitGame(){
            UnloadContent();
            Exit();
        }

        public SpriteFont getFont_Courier(){
            return this.font;
        }

        public SpriteFont getFont_Arial(){
            return Content.Load<SpriteFont>(@"Arial");
        }
    }
}
