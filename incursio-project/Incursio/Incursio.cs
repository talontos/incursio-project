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

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Interface;
using Incursio.Managers;

using IrrKlang;
using Incursio.Interface.Menus;

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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;                //draws our images
        TextureManager textureManager;
        PlayerManager playerManager;
        MessageManager messageManager;
        
        //Sound Stuff
        //SoundManager soundManager;              //does sound stuff
        //ISoundEngine engine = new ISoundEngine();
        
        //Game Time keeping
        float frameTime;

        //unit initialization

        /////////////////////////////

        //map initialization
        //BaseMapEntity tex1;
        public MapBase currentMap;

        //game information
        public State.GameState currentState = State.GameState.Initializing;

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
        Button gameMenuButton;
        Button resumeGameButton;
        Button exitGameToMenuButton;
        KeyboardState kbState;  //lets us know if any input is coming in through the keyboard
        Keys[] keysPressed;
        Button mapSelectButton;
        Button newGameButton_level1;
        Button newGameButton_level2;
        Button newGameButton_level3;
        Button exitGameButton;
        Button saveButton;
        Button loadButton;
        Texture2D clickDestination;
        int clickDestinationFader = 0;
        Coordinate cursorAtClick;

        MenuSet mainMenu;
        MenuSet mapSelectionMenu;
        MenuSet pauseMenu;

        //for game animation
        int frameTimer = 0;
        const int FRAME_LENGTH = 8;

        //analyze FPS
        //string frameString;

        //Game Data Storage stuff
        //IAsyncResult result;
        //Object stateobj;
        //bool GameSaveRequested = false;
        //GamePadState currentState;

        public Incursio(){

            //this.IsFixedTimeStep = false;

            Incursio.instance = this;

            graphics = new GraphicsDeviceManager(this);
            hud = new HeadsUpDisplay();
            kbState = new KeyboardState();
            keysPressed = new Keys[15];
            Content.RootDirectory = "Content";

            this.Components.Add(new GamerServicesComponent(this));

            //MapManager.getInstance().initializeCurrentMap();

            playerManager = PlayerManager.getInstance();
            messageManager = MessageManager.getInstance();

            //soundManager = SoundManager.getInstance();
            
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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //TODO: Load images into textureBank
            this.textureManager = TextureManager.initializeTextureManager(Content);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Courier New");

            MessageManager.getInstance().setFont(Content.Load<SpriteFont>("Arial"));

            FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            // create cursor
            cursor = new Cursor(new Vector2(0, 0));
            clickDestination = Content.Load<Texture2D>(@"destinationClick");

            //initialize the current map
            //MapManager.getInstance().initializeCurrentMap();

            // load the HUD
            hud.loadHeadsUpDisplay();

            // load paused game menu components
            gameMenuButton = new Button(new Vector2(465, 738), TextureBank.InterfaceTextures.gameMenuButton, TextureBank.InterfaceTextures.gameMenuButtonPressed);
            //resumeGameButton = new Button(new Vector2(475, 349), TextureBank.InterfaceTextures.resumeGameButton, TextureBank.InterfaceTextures.resumeGameButtonPressed);
            resumeGameButton = new ResumeGameButton();
            //exitGameToMenuButton = new Button(new Vector2(475, 384), TextureBank.InterfaceTextures.exitGameToMenuButton, TextureBank.InterfaceTextures.exitGameToMenuButtonPressed);
            exitGameToMenuButton = new ExitGameToMenuButton();

            saveButton = new SaveButton();
            loadButton = new LoadButton();

            //load the menu components
            mapSelectButton = new MapSelectButton();
            //newGameButton = new Button(new Vector2(400, 638), TextureBank.InterfaceTextures.newGameButton, TextureBank.InterfaceTextures.newGameButtonPressed);
            newGameButton_level1 = new NewGameButton(State.CampaignLevel.ONE);
            newGameButton_level2 = new NewGameButton(State.CampaignLevel.TWO);
            newGameButton_level3 = new NewGameButton(State.CampaignLevel.THREE);
            //exitGameButton = new Button(new Vector2(524, 638), TextureBank.InterfaceTextures.exitGameButton, TextureBank.InterfaceTextures.exitGameButtonPressed);
            exitGameButton = new ExitGameButton();

            //create menus
            mainMenu = new MenuSet(600, 500, mapSelectButton, loadButton, exitGameButton);
            mapSelectionMenu = new MenuSet(600, 500, newGameButton_level1, newGameButton_level2, newGameButton_level3);
            pauseMenu = new MenuSet(600, 500, resumeGameButton, saveButton, exitGameToMenuButton);

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
            frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            

            cursor.Update();    //update the cursor
            kbState = Keyboard.GetState();  //get the present state of the keyboard
            keysPressed = (Keys[])kbState.GetPressedKeys(); //get all the keys that are being pressed

            //Check game state!
            this.checkState(gameTime);

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
        private void checkState(GameTime gameTime){
            switch(this.currentState){
                case (State.GameState.Initializing): 
                    //TODO: perform initializing actions
                    break;

                case (State.GameState.InPlay):

                    hud.update(cursor);

                    //update entities
                    EntityManager.getInstance().updateAllEntities(gameTime);

                    //NOTE: InputUpdate has been moved outside of this case
                    //InputManager.getInstance().Update(gameTime);

                    MapManager.getInstance().currentMap.update();

                    PlayerManager.getInstance().updatePlayers(gameTime);

                    MessageManager.getInstance().update();

                    //FileManager.getInstance().saveCurrentGame("HeroSave.txt");
                    //FileManager.getInstance().loadGame("HeroSave.txt");

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
                    
                    //listener for menu button
                    //mapSelectButton.Update(cursor);
                    //exitGameButton.Update(cursor);
                    mainMenu.Update(cursor);
                    break;

                case State.GameState.MapSelection:
                    //newGameButton_level1.Update(cursor);
                    //exitGameToMenuButton.Update(cursor);
                    mapSelectionMenu.Update(cursor);
                    break;

                case (State.GameState.Credits):
                    //TODO: perform Credits actions
                    break;

                case (State.GameState.Defeat):
                    //TODO: perform Defeat actions
                    //mapSelectButton.Update(cursor);
                    //exitGameButton.Update(cursor);
                    mainMenu.Update(cursor);
                    //SoundManager.getInstance().StopSound();
                    break;

                case (State.GameState.Victory):
                    //TODO: perform Victory actions
                    //mapSelectButton.Update(cursor);
                    //exitGameButton.Update(cursor);
                    mainMenu.Update(cursor);
                    //SoundManager.getInstance().StopSound();
                    break;

                case (State.GameState.PausedPlay):

                    InputManager.getInstance().Update(gameTime);

                    //exitGameToMenuButton.Update(cursor);
                    //resumeGameButton.Update(cursor);
                    pauseMenu.Update(cursor);
                    //TODO: perform PausedPlay actions
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
                    spriteBatch.DrawString(font, "Game State: INITIALIZING", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: INITIALIZING") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    //TODO: perform initializing actions
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
                    messageManager.displayMessages(spriteBatch);

                    break;

                ////////////////////////////////////////////////

                case (State.GameState.Menu):
                    spriteBatch.DrawString(font, "INCURSIO", FontPos, Color.White, 0, font.MeasureString("INCURSIO") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);
                    spriteBatch.Draw(TextureBank.InterfaceTextures.mainMenuBackground, 
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    //mapSelectButton.Draw(spriteBatch);
                    //exitGameButton.Draw(spriteBatch);
                    //saveButton.Draw(spriteBatch);
                    //loadButton.Draw(spriteBatch);
                    mainMenu.Draw(spriteBatch);
                
                    //TODO: perform Menu actions
                    break;

                case State.GameState.MapSelection:
                    spriteBatch.Draw(TextureBank.InterfaceTextures.mainMenuBackground,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    spriteBatch.DrawString(font, "Game State: MAP SELECTION", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: MAP SELECTION") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    //newGameButton_level1.Draw(spriteBatch);
                    //exitGameToMenuButton.Draw(spriteBatch);
                    mapSelectionMenu.Draw(spriteBatch);

                    break;

                case (State.GameState.Credits):
                    spriteBatch.DrawString(font, "Game State: CREDITS", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: CREDITS") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    //TODO: perform Credits actions
                    break;

                case (State.GameState.Defeat):
                    spriteBatch.DrawString(font, "Game State: Massive Failure! " + stateString, FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: Massive Failure! " + stateString) / 2, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.Draw(TextureBank.InterfaceTextures.defeatMenuBackground,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    //mapSelectButton.Draw(spriteBatch);
                    //exitGameButton.Draw(spriteBatch);
                    mainMenu.Draw(spriteBatch);

                    //TODO: perform Defeat actions
                    break;

                case (State.GameState.Victory):
                    spriteBatch.DrawString(font, "Game State: Epic Win!!! " + stateString, FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: Epic Win!!! " + stateString) / 2, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.Draw(TextureBank.InterfaceTextures.victoryMenuBackground,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);

                    //mapSelectButton.Draw(spriteBatch);
                    //exitGameButton.Draw(spriteBatch);
                    mainMenu.Draw(spriteBatch);

                    //TODO: perform Victory actions
                    break;

                case (State.GameState.PausedPlay):
                    graphics.GraphicsDevice.Clear(Color.SteelBlue);

                    spriteBatch.DrawString(font, "Game Paused", new Vector2(520, 100), Color.White, 0, font.MeasureString("Game Paused") / 2, 1.0f, SpriteEffects.None, 0.5f);

                    spriteBatch.Draw(TextureBank.InterfaceTextures.pauseMenuBackground,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
                    
                    //exitGameToMenuButton.Draw(spriteBatch);
                    //resumeGameButton.Draw(spriteBatch);
                    pauseMenu.Draw(spriteBatch);
                 
                    //TODO: perform PausedPlay actions
                    break;

                case (State.GameState.None):
                    spriteBatch.DrawString(font, "Game State: NONE", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: NONE") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    break;

                default: break;
            }
        }

        /// <summary>
        /// drawEntity() goes through the entityBank and draws all entities that are presently on the screen
        /// </summary>
        public void drawEntity()
        {
            Coordinate onScreen;
            double healthRatio;
            double healthBarTypicalWidth = 0.59375;             //these horrible numbers are ratios for the healthbar of the
            double healthBarTypicalHeight = 0.03125;            //selecetedUnitOverlayTexture.  These account for changes in
            double healthBarTypicalStartWidth = 0.25;           //overlay size, so that the healthbar will still display where
            double healthBarTypicalStartHeight = 0.0625;        //it should.

            //draw all visible units
            EntityManager.getInstance().getAllEntities().ForEach(delegate(BaseGameEntity e)
            {
                e.justDrawn = false;

                if (e is Unit && (e as Unit).getCurrentState() == State.UnitState.Buried)
                {}
                else if (currentMap.isOnScreen(e.getLocation()))
                {
                    if (e.getType() == State.EntityName.LightInfantry ||
                        e.getType() == State.EntityName.HeavyInfantry ||
                        e.getType() == State.EntityName.Archer ||
                        e.getType() == State.EntityName.Hero ||
                        e.getType() == State.EntityName.Camp ||
                        e.getType() == State.EntityName.GuardTower ||
                        e.getType() == State.EntityName.ControlPoint)
                    {
                        e.drawThyself(ref spriteBatch, frameTimer, FRAME_LENGTH);
                    }
                    else if(!e.justDrawn)
                        e.visible = false;
                }

               
            });

            //show selection overlay on all visible selected units
            EntityManager.getInstance().getSelectedUnits().ForEach(delegate(BaseGameEntity u)
            {
                if(currentMap.isOnScreen(u.getLocation())){
                    onScreen = currentMap.positionOnScreen(u.getLocation());


                    healthRatio = (float)u.getHealth() / u.getMaxHealth();

                    int xOffSet = 0;
                    int yOffSet = 0;
                    int width = 0;
                    int height = 0;

                    //find out what the unit is, and configure the offset for each different type
                    if (u.getType() == State.EntityName.LightInfantry)
                    {
                        xOffSet = (int)(TextureBank.EntityTextures.lightInfantrySouth.Width / 2) + 10;
                        yOffSet = (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80) + 7;
                        width = TextureBank.EntityTextures.lightInfantrySouth.Width + 20;
                        height = TextureBank.EntityTextures.lightInfantrySouth.Height + 15;
                    }
                    else if (u.getType() == State.EntityName.Archer)
                    {
                        xOffSet = (int)(TextureBank.EntityTextures.archerSouth.Width / 2) + 10;
                        yOffSet = (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80) + 7;
                        width = TextureBank.EntityTextures.archerSouth.Width + 20;
                        height = TextureBank.EntityTextures.archerSouth.Height + 15;
                    }
                    else if (u.getType() == State.EntityName.HeavyInfantry)
                    {
                        xOffSet = (int)(TextureBank.EntityTextures.archerSouth.Width / 2) + 13;
                        yOffSet = (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80) + 12;
                        width = TextureBank.EntityTextures.archerSouth.Width + 25;
                        height = TextureBank.EntityTextures.archerSouth.Height + 25;
                    }
                    else if (u.getType() == State.EntityName.Hero)
                    {
                        xOffSet = (int)(TextureBank.EntityTextures.heroSouth.Width / 2) + 13;
                        yOffSet = (int)(TextureBank.EntityTextures.heroSouth.Height * 0.80) + 12;
                        width = TextureBank.EntityTextures.heroSouth.Width + 25;
                        height = TextureBank.EntityTextures.heroSouth.Height + 25;
                    }
                    else if (u.getType() == State.EntityName.Camp)
                    {
                        xOffSet = (int)(TextureBank.EntityTextures.campTextureComputer.Width / 2) + 32;
                        yOffSet = (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80) + 15;
                        width = TextureBank.EntityTextures.campTextureComputer.Width + 50;
                        height = TextureBank.EntityTextures.campTextureComputer.Height + 40;
                    }
                    else if (u.getType() == State.EntityName.GuardTower)
                    {
                        xOffSet = (int)(TextureBank.EntityTextures.guardTowerTextureComputer.Width / 2) + 18;
                        yOffSet = (int)(TextureBank.EntityTextures.guardTowerTextureComputer.Height * 0.80) + 20;
                        width = TextureBank.EntityTextures.guardTowerTextureComputer.Width + 30;
                        height = TextureBank.EntityTextures.guardTowerTextureComputer.Height + 40;
                    }
                    else if (u.getType() == State.EntityName.ControlPoint)
                    {
                        xOffSet = (int)(TextureBank.EntityTextures.controlPointComputer.Width / 2) + 18;
                        yOffSet = (int)(TextureBank.EntityTextures.controlPointComputer.Height * 0.80) + 20;
                        width = TextureBank.EntityTextures.controlPointComputer.Width + 30;
                        height = TextureBank.EntityTextures.controlPointComputer.Height + 40;
                    }

                    spriteBatch.Draw(TextureBank.EntityTextures.selectedUnitOverlayTexture,
                        new Rectangle(onScreen.x - xOffSet, onScreen.y - yOffSet, width, height),
                        Color.White);

                    if (u.getPlayer() == State.PlayerId.HUMAN)
                    {
                        spriteBatch.Draw(TextureBank.EntityTextures.healthRatioTexture,
                            new Rectangle(onScreen.x - xOffSet + 1 + (int)(width * healthBarTypicalStartWidth), onScreen.y - yOffSet + 1 + (int)(height * healthBarTypicalStartHeight), (int)((width * healthBarTypicalWidth) * healthRatio), (int)(height * healthBarTypicalHeight)),
                            Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(TextureBank.EntityTextures.healthRatioTexture,
                            new Rectangle(onScreen.x - xOffSet + 1 + (int)(width * healthBarTypicalStartWidth), onScreen.y - yOffSet + 1 + (int)(height * healthBarTypicalStartHeight), (int)((width * healthBarTypicalWidth) * healthRatio), (int)(height * healthBarTypicalHeight)),
                            Color.Red);
                    }
                   
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

        public void exitGame(){
            UnloadContent();
            Exit();
        }
    }
}
