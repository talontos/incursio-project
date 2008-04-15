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
        SoundManager soundManager;              //does sound stuff
        //int x = -1;
        //ISoundEngine engine = new ISoundEngine();
        
        //Game Time keeping
        float frameTime;

        //unit initialization

        /////////////////////////////

        //map initialization
        BaseMapEntity tex1;
        public MapBase currentMap;

        //game information
        public State.GameState currentState = State.GameState.Initializing;

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
        Button newGameButton;
        Button exitGameButton;
        Texture2D clickDestination;
        int clickDestinationFader = 0;
        Coordinate cursorAtClick;

        //for game animation
        int frameTimer = 0;
        const int FRAME_LENGTH = 8;

        //analyze FPS
        string frameString;

        public Incursio(){

            //this.IsFixedTimeStep = false;

            Incursio.instance = this;

            graphics = new GraphicsDeviceManager(this);
            hud = new HeadsUpDisplay();
            kbState = new KeyboardState();
            keysPressed = new Keys[15];
            Content.RootDirectory = "Content";

            currentMap = MapManager.getInstance().setCurrentLevel(State.CampaignLevel.ONE);
            //MapManager.getInstance().initializeCurrentMap();

            playerManager = PlayerManager.getInstance();
            messageManager = MessageManager.getInstance();

            soundManager = SoundManager.getInstance();
            
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
            MapManager.getInstance().initializeCurrentMap();

            // load the HUD
            hud.loadHeadsUpDisplay();

            // load paused game menu components
            gameMenuButton = new Button(new Vector2(465, 738), TextureBank.InterfaceTextures.gameMenuButton, TextureBank.InterfaceTextures.gameMenuButtonPressed);
            resumeGameButton = new Button(new Vector2(475, 349), TextureBank.InterfaceTextures.resumeGameButton, TextureBank.InterfaceTextures.resumeGameButtonPressed);
            exitGameToMenuButton = new Button(new Vector2(475, 384), TextureBank.InterfaceTextures.exitGameToMenuButton, TextureBank.InterfaceTextures.exitGameToMenuButtonPressed);

            //load the menu components
            newGameButton = new Button(new Vector2(400, 638), TextureBank.InterfaceTextures.newGameButton, TextureBank.InterfaceTextures.newGameButtonPressed);
            exitGameButton = new Button(new Vector2(524, 638), TextureBank.InterfaceTextures.exitGameButton, TextureBank.InterfaceTextures.exitGameButtonPressed);

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

            if (frameTimer > FRAME_LENGTH)
            {
                frameTimer = 0;
            }
            else
                frameTimer++;

            cursor.Update();    //update the cursor
            kbState = Keyboard.GetState();  //get the present state of the keyboard
            keysPressed = (Keys[])kbState.GetPressedKeys(); //get all the keys that are being pressed
            //soundManager.updateSounds();

            //Check game state!
            this.checkState(gameTime);

            base.Update(gameTime);

            frameString = frameTime + " FPS";
            Console.Out.Write(frameString);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

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

                    //this.soundManager.PlaySound("../../../Content/Audio/Thunderhorse.mp3", true);

                    hud.update(cursor);

                    //update entities
                    EntityManager.getInstance().updateAllEntities(gameTime);

                    InputManager.getInstance().Update(gameTime);

                    MapManager.getInstance().currentMap.update(keysPressed, 1024, 768);

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
                    
                    //listener for menu button
                    newGameButton.Update(cursor);
                    exitGameButton.Update(cursor);

                    //if (x < 0)
                    //{
                    //    engine.Play2D("../../../Content/Audio/explosion.wav");
                    //    engine.Play2D("../../../Content/Audio/Thunderhorse.mp3", true);
                    //}
                    //x = 1;

                    SoundManager.getInstance().PlaySound("../../../Content/Audio/Thunderhorse.mp3", false);

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

                    InputManager.getInstance().Update(gameTime);

                    exitGameToMenuButton.Update(cursor);
                    resumeGameButton.Update(cursor);

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
                    spriteBatch.DrawString(font, "Game State: Epic Win!!!", FontPos, Color.DarkBlue, 0, font.MeasureString("Game State: VICTORY") / 2, 1.0f, SpriteEffects.None, 0.5f);
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
                    if (e.getType() == State.EntityName.LightInfantry)
                    {
                        e.visible = true;
                        e.justDrawn = true;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

                        //depending on the unit's state, draw their textures
                        //idle
                        if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Idle)
                        {
                            //south or idle
                            if ((e as LightInfantryUnit).getDirection() == State.Direction.Still || (e as LightInfantryUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height), Color.White);
                            }
                            //east
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryEast,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryEast.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantryEast.Width, TextureBank.EntityTextures.lightInfantryEast.Height), Color.White);
                            }
                            //west
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryWest,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryWest.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantryWest.Width, TextureBank.EntityTextures.lightInfantryWest.Height), Color.White);
                            }
                            //north
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryNorth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryNorth.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantryNorth.Width, TextureBank.EntityTextures.lightInfantryNorth.Height), Color.White);
                            }

                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Attacking)
                        {
                            //TODO:
                            //Attacking Animation
                            spriteBatch.Draw(TextureBank.EntityTextures.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height), Color.Red);
                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Dead)
                        {
                            //TODO:
                            //Dead stuff
                            spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryDead,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryDead.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryDead.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantryDead.Width, TextureBank.EntityTextures.lightInfantryDead.Height), Color.White);
                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Guarding)
                        {
                            //TODO:
                            //Guarding Animation
                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Moving)
                        {
                            //TODO:
                            //Moving Animation
                            if ((e as LightInfantryUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingWest,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryWest.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantryWest.Width, TextureBank.EntityTextures.lightInfantryWest.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 20, 30), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingWest.Width - 20)
                                    {
                                        e.currentFrameX = e.currentFrameX + 20;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingEast,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryEast.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantryEast.Width, TextureBank.EntityTextures.lightInfantryEast.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 20, 30), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingEast.Width - 20)
                                    {
                                        e.currentFrameX = e.currentFrameX + 20;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingSouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingSouth.Width - 50)
                                    {
                                        e.currentFrameX = e.currentFrameX + 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingNorth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryNorth.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantryNorth.Width, TextureBank.EntityTextures.lightInfantryNorth.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingNorth.Width - 50)
                                    {
                                        e.currentFrameX = e.currentFrameX + 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.UnderAttack)
                        {
                            //TODO:
                            //Under Attack Animation
                            spriteBatch.Draw(TextureBank.EntityTextures.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height), Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(TextureBank.EntityTextures.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height), Color.White);
                        }
                        
                    }
                    if (e.getType() == State.EntityName.HeavyInfantry)
                    {
                        e.visible = true;
                        e.justDrawn = true;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

                        //depending on the unit's state, draw their textures
                        //idle
                        if ((e as HeavyInfantryUnit).getCurrentState() == State.UnitState.Idle)
                        {
                            //south or idle
                            if ((e as HeavyInfantryUnit).getDirection() == State.Direction.Still || (e as HeavyInfantryUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantrySouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantrySouth.Width, TextureBank.EntityTextures.heavyInfantrySouth.Height), Color.White);
                            }
                            //east
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryEast,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryEast.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantryEast.Width, TextureBank.EntityTextures.heavyInfantryEast.Height), Color.White);
                            }
                            //west
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryWest,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryWest.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantryWest.Width, TextureBank.EntityTextures.heavyInfantryWest.Height), Color.White);
                            }
                            //north
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryNorth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryNorth.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantryNorth.Width, TextureBank.EntityTextures.heavyInfantryNorth.Height), Color.White);
                            }

                        }
                        else if ((e as HeavyInfantryUnit).getCurrentState() == State.UnitState.Attacking)
                        {
                            //TODO:
                            //Attacking Animation
                            if ((e as HeavyInfantryUnit).getDirection() == State.Direction.East || (e as HeavyInfantryUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryAttackingEast,
                                    new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                                    new Rectangle(e.currentFrameXAttackDeath, e.currentFrameYAttackDeath, 45, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.attackFramePause >= 4 || e.currentFrameXAttackDeath > 0)
                                    {
                                        if (e.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryAttackingEast.Width - 45)
                                        {
                                            e.currentFrameXAttackDeath = e.currentFrameXAttackDeath + 45;
                                        }
                                        else
                                        {
                                            e.currentFrameXAttackDeath = 0;
                                        }
                                        e.attackFramePause = 0;
                                    }
                                    else
                                    {
                                        e.attackFramePause++;
                                    }
                                    
                                }
                            }
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.West || (e as HeavyInfantryUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryAttackingWest,
                                    new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                                    new Rectangle(e.currentFrameXAttackDeath, e.currentFrameYAttackDeath, 45, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.attackFramePause >= 4 || e.currentFrameXAttackDeath > 0)
                                    {
                                        if (e.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryAttackingEast.Width - 45)
                                        {
                                            e.currentFrameXAttackDeath = e.currentFrameXAttackDeath + 45;
                                        }
                                        else
                                        {
                                            e.currentFrameXAttackDeath = 0;
                                        }
                                        e.attackFramePause = 0;
                                    }
                                    else
                                    {
                                        e.attackFramePause++;
                                    }
                                }
                            }
                            
                        }
                        else if ((e as HeavyInfantryUnit).getCurrentState() == State.UnitState.Dead)
                        {
                            //TODO:
                            //Dead stuff
                            if( (e as HeavyInfantryUnit).getDirection() == State.Direction.East || (e as HeavyInfantryUnit).getDirection() == State.Direction.North)
                            {
                                if(!(e as HeavyInfantryUnit).playedDeath)
                                {
                                    spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_East,
                                    new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                                    new Rectangle(e.currentFrameXAttackDeath, e.currentFrameYAttackDeath, 45, 38), Color.White);

                                    if (frameTimer >= FRAME_LENGTH)
                                    {
                                        if (e.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryDeath_East.Width - 45)
                                        {
                                            e.currentFrameXAttackDeath = e.currentFrameXAttackDeath + 45;
                                        }
                                        else
                                        {
                                            (e as HeavyInfantryUnit).playedDeath = true;
                                        }
                                    }
                                }
                                else
                                {
                                    spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_East,
                                    new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                                    new Rectangle(135, 0, 45, 38), Color.White);
                                }
                                
                            }
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.West || (e as HeavyInfantryUnit).getDirection() == State.Direction.South)
                            {
                                if (!(e as HeavyInfantryUnit).playedDeath)
                                {
                                    spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_West,
                                    new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                                    new Rectangle(e.currentFrameXAttackDeath, e.currentFrameYAttackDeath, 45, 38), Color.White);

                                    if (frameTimer >= FRAME_LENGTH)
                                    {
                                        if (e.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryDeath_West.Width - 45)
                                        {
                                            e.currentFrameXAttackDeath = e.currentFrameXAttackDeath + 45;
                                        }
                                        else
                                        {
                                            (e as HeavyInfantryUnit).playedDeath = true;
                                        }
                                    }
                                }
                                else
                                {
                                    spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_West,
                                    new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                                    new Rectangle(135, 0, 45, 38), Color.White);
                                }

                            }
                        }
                        else if ((e as HeavyInfantryUnit).getCurrentState() == State.UnitState.Guarding)
                        {
                            //TODO:
                            //Guarding Animation
                        }
                        else if ((e as HeavyInfantryUnit).getCurrentState() == State.UnitState.Moving)
                        {
                            //TODO:
                            //Moving Animation
                            if( (e as HeavyInfantryUnit).getDirection() == State.Direction.East){
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingEast,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryEast.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantryEast.Width, TextureBank.EntityTextures.heavyInfantryEast.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.heavyInfantryMovingEast.Width - 25)
                                    {
                                        e.currentFrameX = e.currentFrameX + 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingWest,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryWest.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantryWest.Width, TextureBank.EntityTextures.heavyInfantryWest.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX > 0)
                                    {
                                        e.currentFrameX = e.currentFrameX - 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = TextureBank.EntityTextures.heavyInfantryMovingWest.Width - TextureBank.EntityTextures.heavyInfantryWest.Width;
                                    }
                                }
                            }
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingSouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantrySouth.Width, TextureBank.EntityTextures.heavyInfantrySouth.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.heavyInfantryMovingSouth.Width - 50)
                                    {
                                        e.currentFrameX = e.currentFrameX + 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as HeavyInfantryUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingNorth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryNorth.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantryNorth.Width, TextureBank.EntityTextures.heavyInfantryNorth.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.heavyInfantryMovingNorth.Width - 50)
                                    {
                                        e.currentFrameX = e.currentFrameX + 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                        }
                        else if ((e as HeavyInfantryUnit).getCurrentState() == State.UnitState.UnderAttack)
                        {
                            //TODO:
                            //Under Attack Animation
                        }
                        else
                        {
                            spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantrySouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantrySouth.Height * 0.80),
                                    TextureBank.EntityTextures.heavyInfantrySouth.Width, TextureBank.EntityTextures.heavyInfantrySouth.Height), Color.White);
                        }
                    }
                    else if (e.getType() == State.EntityName.Archer)
                    {
                        e.visible = true;
                        e.justDrawn = false;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

                        //depending on the unit's state, draw their textures
                        //idle
                        if ((e as ArcherUnit).getCurrentState() == State.UnitState.Idle)
                        {
                            //south or idle
                            if ((e as ArcherUnit).getDirection() == State.Direction.Still || (e as ArcherUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerSouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80),
                                    TextureBank.EntityTextures.archerSouth.Width, TextureBank.EntityTextures.archerSouth.Height), Color.White);
                            }
                            //east
                            else if ((e as ArcherUnit).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerEast,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerEast.Height * 0.80),
                                    TextureBank.EntityTextures.archerEast.Width, TextureBank.EntityTextures.archerEast.Height), Color.White);
                            }
                            //west
                            else if ((e as ArcherUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerWest,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerWest.Height * 0.80),
                                    TextureBank.EntityTextures.archerWest.Width, TextureBank.EntityTextures.archerWest.Height), Color.White);
                            }
                            //north
                            else if ((e as ArcherUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerNorth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerNorth.Height * 0.80),
                                    TextureBank.EntityTextures.archerNorth.Width, TextureBank.EntityTextures.archerNorth.Height), Color.White);
                            }

                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.Attacking)
                        {
                            //TODO:
                            //Attacking Animation
                            spriteBatch.Draw(TextureBank.EntityTextures.archerSouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80),
                                    TextureBank.EntityTextures.archerSouth.Width, TextureBank.EntityTextures.archerSouth.Height), Color.Red);
                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.Dead)
                        {
                            //TODO:
                            //Dead stuff
                            spriteBatch.Draw(TextureBank.EntityTextures.archerDead,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerDead.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerDead.Height * 0.80),
                                    TextureBank.EntityTextures.archerDead.Width, TextureBank.EntityTextures.archerDead.Height), Color.White);
                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.Guarding)
                        {
                            //TODO:
                            //Guarding Animation
                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.Moving)
                        {
                            //TODO:
                            //Moving Animation
                            if ((e as ArcherUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerMovingWest,
                                                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerWest.Height * 0.80),
                                                                    TextureBank.EntityTextures.archerWest.Width, TextureBank.EntityTextures.archerWest.Height),
                                                                    new Rectangle(e.currentFrameX, e.currentFrameY, 20, 30), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.archerMovingWest.Width - 20)
                                    {
                                        e.currentFrameX = e.currentFrameX + 20;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as ArcherUnit).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerMovingEast,
                                                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerWest.Height * 0.80),
                                                                    TextureBank.EntityTextures.archerWest.Width, TextureBank.EntityTextures.archerWest.Height),
                                                                    new Rectangle(e.currentFrameX, e.currentFrameY, 20, 30), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.archerMovingEast.Width - 20)
                                    {
                                        e.currentFrameX = e.currentFrameX + 20;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as ArcherUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerMovingSouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80),
                                    TextureBank.EntityTextures.archerSouth.Width, TextureBank.EntityTextures.archerSouth.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.archerMovingSouth.Width - 50)
                                    {
                                        e.currentFrameX = e.currentFrameX + 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else if ((e as ArcherUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.archerMovingNorth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerNorth.Height * 0.80),
                                    TextureBank.EntityTextures.archerNorth.Width, TextureBank.EntityTextures.archerNorth.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 25, 38), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.archerMovingNorth.Width - 50)
                                    {
                                        e.currentFrameX = e.currentFrameX + 25;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }

                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.UnderAttack)
                        {
                            //TODO:
                            //Under Attack Animation
                        }
                        else
                        {
                            spriteBatch.Draw(TextureBank.EntityTextures.archerSouth,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80),
                                    TextureBank.EntityTextures.archerSouth.Width, TextureBank.EntityTextures.archerSouth.Height), Color.White);
                        }

                    }
                    //TODO: once we get hero textures, uncomment this block
                    /*if (e.getType() == State.EntityName.Hero)
                    {
                        e.visible = true;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

                        //depending on the unit's state, draw their textures
                        //idle
                        if ((e as Hero).getCurrentState() == State.UnitState.Idle)
                        {
                            //south or idle
                            if ((e as Hero).getDirection() == State.Direction.Still || (e as Hero).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(this.heroSouth,
                                    new Rectangle(onScreen.x - (this.heroSouth.Width / 2), onScreen.y - (int)(this.heroSouth.Height * 0.80),
                                    this.heroSouth.Width, this.heroSouth.Height), Color.White);
                            }
                            //east
                            else if ((e as Hero).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(this.heroEast,
                                    new Rectangle(onScreen.x - (this.heroEast.Width / 2), onScreen.y - (int)(this.heroEast.Height * 0.80),
                                    this.heroEast.Width, this.heroEast.Height), Color.White);
                            }
                            //west
                            else if ((e as Hero).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(this.heroWest,
                                    new Rectangle(onScreen.x - (this.heroWest.Width / 2), onScreen.y - (int)(this.heroWest.Height * 0.80),
                                    this.heroWest.Width, this.heroWest.Height), Color.White);
                            }
                            //north
                            else if ((e as Hero).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(this.heroNorth,
                                    new Rectangle(onScreen.x - (this.heroNorth.Width / 2), onScreen.y - (int)(this.heroNorth.Height * 0.80),
                                    this.heroNorth.Width, this.heroNorth.Height), Color.White);
                            }

                        }
                        else if ((e as Hero).getCurrentState() == State.UnitState.Attacking)
                        {
                            //TODO:
                            //Attacking Animation
                        }
                        else if ((e as Hero).getCurrentState() == State.UnitState.Dead)
                        {
                            //TODO:
                            //Dead stuff
                            //with hero death, end the current map in defeat for player hero, victory if computer hero
                        }
                        else if ((e as Hero).getCurrentState() == State.UnitState.Guarding)
                        {
                            //TODO:
                            //Guarding Animation
                        }
                        else if ((e as Hero).getCurrentState() == State.UnitState.Moving)
                        {
                            //TODO:
                            //Moving Animation
                        }
                        else if ((e as Hero).getCurrentState() == State.UnitState.UnderAttack)
                        {
                            //TODO:
                            //Under Attack Animation
                        }
                        else
                        {
                            spriteBatch.Draw(this.heroSouth,
                                    new Rectangle(onScreen.x - (this.heroSouth.Width / 2), onScreen.y - (int)(this.heroSouth.Height * 0.80),
                                    this.heroSouth.Width, this.heroSouth.Height), Color.White);
                        }
                    }*/
                    else if (e.getType() == State.EntityName.Camp)
                    {
                        e.visible = true;
                        e.justDrawn = false;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
                        if ((e as CampStructure).getCurrentState() == State.StructureState.BeingBuilt)
                        {
                            //TODO: draw construction?
                        }
                        else if ((e as CampStructure).getCurrentState() == State.StructureState.Building)
                        {
                            //TODO: draw something special for when the structure is building something (fires flickering or w/e)
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.campTexturePlayerBuilding,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTexturePlayer.Height * 0.80),
                                    TextureBank.EntityTextures.campTexturePlayer.Width, TextureBank.EntityTextures.campTexturePlayer.Height),
                                    new Rectangle(e.currentFrameX, e.currentFrameY, 64, 64), Color.White);

                                if (frameTimer >= FRAME_LENGTH)
                                {
                                    if (e.currentFrameX < TextureBank.EntityTextures.campTexturePlayerBuilding.Width - 64)
                                    {
                                        e.currentFrameX = e.currentFrameX + 64;
                                    }
                                    else
                                    {
                                        e.currentFrameX = 0;
                                    }
                                }
                            }
                            else
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                                    TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                            }

                        }
                        else if ((e as CampStructure).getCurrentState() == State.StructureState.Destroyed)
                        {
                            //TODO: building asploded
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerExploded,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputerDestroyed.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputerDestroyed.Height * 0.80),
                                    TextureBank.EntityTextures.campTextureComputerDestroyed.Width, TextureBank.EntityTextures.campTextureComputerDestroyed.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerExploded,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputerDestroyed.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputerDestroyed.Height * 0.80),
                                    TextureBank.EntityTextures.campTextureComputerDestroyed.Width, TextureBank.EntityTextures.campTextureComputerDestroyed.Height), Color.White);
                            }
                        }
                        else if ((e as CampStructure).getCurrentState() == State.StructureState.Idle)
                        {
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.campTexturePlayer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTexturePlayer.Height * 0.80),
                                    TextureBank.EntityTextures.campTexturePlayer.Width, TextureBank.EntityTextures.campTexturePlayer.Height), Color.White);
                            }
                            else
                            {
                                float ratio = (float)e.getHealth() / e.getMaxHealth();

                                if (ratio >= 0.50)
                                {
                                    spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputer,
                                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                                        TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                                }
                                else if (ratio < 0.50 && ratio >= 0.25)
                                {
                                    spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerDamaged,
                                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                                        TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                                }
                                else if (ratio < 0.25 && ratio >= 0.00)
                                {
                                    spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerDestroyed,
                                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                                        TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                                }

                            }
                        }

                    }
                    else if (e.getType() == State.EntityName.GuardTower)
                    {
                        e.visible = true;
                        e.justDrawn = false;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
                        if ((e as GuardTowerStructure).getCurrentState() == State.StructureState.BeingBuilt)
                        {
                            //TODO: draw construction?
                        }
                        else if ((e as GuardTowerStructure).getCurrentState() == State.StructureState.Building)
                        {
                            //TODO: draw something special for when the structure is building something (fires flickering or w/e)
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTexturePlayer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTexturePlayer.Height * 0.80),
                                    TextureBank.EntityTextures.guardTowerTexturePlayer.Width, TextureBank.EntityTextures.guardTowerTexturePlayer.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTextureComputer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTextureComputer.Height * 0.80),
                                    TextureBank.EntityTextures.guardTowerTextureComputer.Width, TextureBank.EntityTextures.guardTowerTextureComputer.Height), Color.White);
                            }

                        }
                        else if ((e as GuardTowerStructure).getCurrentState() == State.StructureState.Destroyed)
                        {
                            //TODO: building asploded
                        }
                        else if ((e as GuardTowerStructure).getCurrentState() == State.StructureState.Idle || (e as GuardTowerStructure).getCurrentState() == State.StructureState.Attacking)
                        {
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTexturePlayer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTexturePlayer.Height * 0.80),
                                    TextureBank.EntityTextures.guardTowerTexturePlayer.Width, TextureBank.EntityTextures.guardTowerTexturePlayer.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTextureComputer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTextureComputer.Height * 0.80),
                                    TextureBank.EntityTextures.guardTowerTextureComputer.Width, TextureBank.EntityTextures.guardTowerTextureComputer.Height), Color.White);
                            }
                        }
                    }
                    else if (e.getType() == State.EntityName.ControlPoint)
                    {
                        e.visible = true;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
                        if ((e as ControlPoint).getCurrentState() == State.StructureState.Building)
                        {
                            //TODO: draw something special for when the structure is building something (fires flickering or w/e)
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.controlPointPlayer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointPlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointPlayer.Height * 0.80),
                                    TextureBank.EntityTextures.controlPointPlayer.Width, TextureBank.EntityTextures.controlPointPlayer.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.controlPointComputer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointComputer.Height * 0.80),
                                    TextureBank.EntityTextures.controlPointComputer.Width, TextureBank.EntityTextures.controlPointComputer.Height), Color.White);
                            }

                        }
                        else if ((e as ControlPoint).getCurrentState() == State.StructureState.Idle || (e as ControlPoint).getCurrentState() == State.StructureState.Attacking)
                        {
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.controlPointPlayer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointPlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointPlayer.Height * 0.80),
                                    TextureBank.EntityTextures.controlPointPlayer.Width, TextureBank.EntityTextures.controlPointPlayer.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(TextureBank.EntityTextures.controlPointComputer,
                                    new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointComputer.Height * 0.80),
                                    TextureBank.EntityTextures.controlPointComputer.Width, TextureBank.EntityTextures.controlPointComputer.Height), Color.White);
                            }
                        }
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
    }
}
