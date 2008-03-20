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
        public static Random rand = new Random(DebugUtil.RandomNumberSeed);

        public ObjectFactory factory;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;                //draws our images

        //players
        Player computerPlayer;
        Player humanPlayer;

        //Object-map///////////////////
        int nextKeyId;
        List<BaseGameEntity> entityBank;
        ///////////////////////////////

        //Textures-Map/////////////////
        List<Texture2D> textureBank;
        ///////////////////////////////

        //unit initialization
        //TODO: **move these to Player class**
        List<BaseGameEntity> selectedUnits;
        int numUnitsSelected;

        //Overlay for selected units
        Texture2D selectedUnitOverlayTexture;
        Texture2D healthRatioTexture;

        //Unit Textures//////////////
        Texture2D lightInfantryEast;
        Texture2D lightInfantryWest;
        Texture2D lightInfantrySouth;
        Texture2D lightInfantryNorth;
        Texture2D lightInfantryDead;

        Texture2D archerEast;
        Texture2D archerWest;
        Texture2D archerSouth;
        Texture2D archerNorth;
        Texture2D archerDead;

        Texture2D heroEast;
        Texture2D heroWest;
        Texture2D heroSouth;
        Texture2D heroNorth;
        /////////////////////////////

        //Structure Textures/////////
        Texture2D campTexturePlayer;
        Texture2D campTextureComputer;
        Texture2D campTextureComputerDamaged;
        Texture2D campTextureComputerDestroyed;
        Texture2D campTextureComputerExploded;

        Texture2D guardTowerTexturePlayer;
        Texture2D guardTowerTextureComputer;

        /////////////////////////////

        //map initialization
        BaseMapEntity tex1;
        public MapBase currentMap;

        //game information
        public State.GameState currentState = State.GameState.Initializing;

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
        Texture2D clickDestination;
        int clickDestinationFader = 0;
        Coordinate cursorAtClick;

        public Incursio(){

            Incursio.instance = this;

            graphics = new GraphicsDeviceManager(this);
            hud = new HeadsUpDisplay();
            kbState = new KeyboardState();
            keysPressed = new Keys[15];
            Content.RootDirectory = "Content";

            factory = new ObjectFactory(this);

            entityBank = new List<BaseGameEntity>();
            textureBank = new List<Texture2D>();

            //NOTE: the map should be instantiated before units
            //TODO: test map, DELETE THESE LINES
            currentMap = new MapBase(2048, 1024, 1024, 768);

            
            //testing unit creation/placement/moving///
            LightInfantryUnit infUnit1 = (LightInfantryUnit) factory.create("Incursio.Classes.LightInfantryUnit", State.PlayerId.HUMAN);
            LightInfantryUnit infUnit2 = (LightInfantryUnit) factory.create("Incursio.Classes.LightInfantryUnit", State.PlayerId.HUMAN);
            LightInfantryUnit infUnit3 = (LightInfantryUnit) factory.create("Incursio.Classes.LightInfantryUnit", State.PlayerId.COMPUTER);

            ArcherUnit archUnit1 = (ArcherUnit)factory.create("Incursio.Classes.ArcherUnit", State.PlayerId.HUMAN);
            ArcherUnit archUnit2 = (ArcherUnit)factory.create("Incursio.Classes.ArcherUnit", State.PlayerId.COMPUTER);

            CampStructure playerCamp = (CampStructure)factory.create("Incursio.Classes.CampStructure", State.PlayerId.HUMAN);
            CampStructure computerCamp = (CampStructure)factory.create("Incursio.Classes.CampStructure", State.PlayerId.COMPUTER);

            GuardTowerStructure playerTower1 = (GuardTowerStructure)factory.create("Incursio.Classes.GuardTowerStructure", State.PlayerId.HUMAN);

            //infUnit1.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            infUnit1.setLocation(new Coordinate(240, 380));
            infUnit2.setLocation(new Coordinate(200, 400));
            //archUnit2.setLocation(new Coordinate(820, 195));
            archUnit2.setLocation(new Coordinate(300, 200));
            infUnit3.setLocation(new Coordinate(800, 220)); //for ease of testing
            archUnit1.setLocation(new Coordinate(160, 395));
            playerCamp.setLocation(new Coordinate(100, 400));
            computerCamp.setLocation(new Coordinate(900, 200));
            playerTower1.setLocation(new Coordinate(150, 340));
            //infUnit3.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            
            infUnit1.setHealth(80);
            infUnit2.setHealth(80);
            infUnit3.setHealth(50);
            archUnit1.setHealth(90);
            archUnit2.setHealth(90);
            playerCamp.setHealth(350);
            computerCamp.setHealth(350);
            playerTower1.setHealth(350);

            infUnit1.setMap(currentMap);
            infUnit2.setMap(currentMap);
            infUnit3.setMap(currentMap);
            archUnit1.setMap(currentMap);
            archUnit2.setMap(currentMap);
            playerCamp.setMap(currentMap);
            computerCamp.setMap(currentMap);
            playerTower1.setMap(currentMap);
            

            /*
            infUnit1.move(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            infUnit2.move(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            infUnit3.move(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
             */
            ///////////////////////////////////////////

            //TODO: instead have 1 Player[] ??
            computerPlayer = new Player();
            computerPlayer.id = State.PlayerId.COMPUTER;

            humanPlayer = new Player();
            humanPlayer.id = State.PlayerId.HUMAN;

            selectedUnits = new List<BaseGameEntity>();
            selectedUnits.Add(infUnit1);
            numUnitsSelected = 1;

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

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Courier New");

            FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

            // create cursor
            cursor = new Cursor(new Vector2(0, 0), Content.Load<Texture2D>(@"cursor"), Content.Load<Texture2D>(@"cursor_click"));
            clickDestination = Content.Load<Texture2D>(@"destinationClick");

            // load the HUD texture 
            hud.loadHeadsUpDisplay(Content.Load<Texture2D>(@"utilityBarUnderlay"), Content.Load<Texture2D>(@"lightInfantryPortrait"), 
                Content.Load<Texture2D>(@"archerPortrait"), Content.Load<Texture2D>(@"infantryIcon"), Content.Load<Texture2D>(@"archerIcon"), 
                Content.Load<Texture2D>(@"resourceBarUnderlay"));

            // load paused game menu components
            gameMenuButton = new Button(new Vector2(465, 738), Content.Load<Texture2D>(@"gameMenuButton"), Content.Load<Texture2D>(@"gameMenuButtonPressed"));
            resumeGameButton = new Button(new Vector2(475, 349), Content.Load<Texture2D>(@"resumeButton"), Content.Load<Texture2D>(@"resumeButtonPressed"));
            exitGameToMenuButton = new Button(new Vector2(475, 384), Content.Load<Texture2D>(@"exitGameButton"), Content.Load<Texture2D>(@"exitGameButtonPressed"));

            //load the menu components
            newGameButton = new Button(new Vector2(400, 638), Content.Load<Texture2D>(@"newGameButton"), Content.Load<Texture2D>(@"newGamePressed"));
            exitGameButton = new Button(new Vector2(524, 638), Content.Load<Texture2D>(@"exitGameButton"), Content.Load<Texture2D>(@"exitGameButtonPressed"));

            //load maps 
            //load testmap texture
            tex1 = new BaseMapEntity(Content.Load<Texture2D>(@"grass"));
            for (int j = 0; j < 32; j++)
            {
                for (int i = 0; i < 64; i++)
                {
                    currentMap.addMapEntity(tex1, i, j);
                }
            }

            /*
            tex1 = new BaseMapEntity(Content.Load<Texture2D>(@"barrel"));
            currentMap.addMapEntity(tex1, 5, 6);
            currentMap.addMapEntity(tex1, 16, 2);
            currentMap.addMapEntity(tex1, 60, 21);
            currentMap.addMapEntity(tex1, 2, 19);
            currentMap.addMapEntity(tex1, 39, 60);
            */

            //Load Unit Textures
            lightInfantryEast = Content.Load<Texture2D>(@"infantry_right");
            lightInfantryWest = Content.Load<Texture2D>(@"infantry_left");
            lightInfantrySouth = Content.Load<Texture2D>(@"infantry_still");
            lightInfantryNorth = Content.Load<Texture2D>(@"infantry_back");
            lightInfantryDead = Content.Load<Texture2D>(@"infantry_dead");

            archerEast = Content.Load<Texture2D>(@"archer_right");
            archerWest = Content.Load<Texture2D>(@"archer_left");
            archerSouth = Content.Load<Texture2D>(@"archer_Still");
            archerNorth = Content.Load<Texture2D>(@"archer_Back");
            archerDead = Content.Load<Texture2D>(@"Archer_dead");

            //TODO: get hero textures
            //heroEast = Content.Load<Texture2D>(@"");
            //heroWest = Content.Load<Texture2D>(@"");
            //heroSouth = Content.Load<Texture2D>(@"");
            //heroNorth = Content.Load<Texture2D>(@"");

            //Load structure textures
            campTexturePlayer = Content.Load<Texture2D>(@"Fort_friendly");
            campTextureComputer = Content.Load<Texture2D>(@"Fort_hostile");
            campTextureComputerDamaged = Content.Load<Texture2D>(@"Fort_hostile_damaged");
            campTextureComputerDestroyed = Content.Load<Texture2D>(@"Fort_hostile_destroyed");
            campTextureComputerExploded = Content.Load<Texture2D>(@"Fort_hostile_exploded");

            guardTowerTexturePlayer = Content.Load<Texture2D>(@"Tower_friendly");
            guardTowerTextureComputer = Content.Load<Texture2D>(@"Tower_hostile");

            //load overlays
            selectedUnitOverlayTexture = Content.Load<Texture2D>(@"selectedUnitOverlay");
            healthRatioTexture = Content.Load<Texture2D>(@"healthBar");

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
        private void checkState(GameTime gameTime){
            switch(this.currentState){
                case (State.GameState.Initializing): 
                    //TODO: perform initializing actions
                    break;

                case (State.GameState.InPlay):

                    //Keypress memory
                    bool shiftPressed = false;

                    selectedUnits = hud.update(cursor, selectedUnits, numUnitsSelected);
                    numUnitsSelected = hud.getNumUnits();

                    entityBank.ForEach(delegate(BaseGameEntity e)
                    {
                        e.Update(gameTime);
                    });

                    for (int i = 0; i < keysPressed.Length; i++)
                    {
                        switch (keysPressed[i])
                        {
                            case Keys.Escape: currentState = State.GameState.PausedPlay; break;

                            case Keys.L:
                                if (numUnitsSelected == 1 && selectedUnits[0].getType() == State.EntityName.Camp)
                                {
                                    (selectedUnits[0] as CampStructure).build(new LightInfantryUnit());
                                }
                                break;

                            case Keys.A:
                                if (numUnitsSelected == 1 && selectedUnits[0].getType() == State.EntityName.Camp)
                                {
                                    (selectedUnits[0] as CampStructure).build(new ArcherUnit());
                                }
                                break;

                            case Keys.Enter://just so we can have a breakpoint whenever we want...
                                //this.currentMap.printOccupancyGrid();
                                break;

                            case Keys.LeftShift:
                            case Keys.RightShift:
                                shiftPressed = true;
                                break;
                            default: break;
                        }
                    }
                    
                    //LEFT BUTTON state has changed
                    if(cursor.getMouseState().LeftButton != cursor.getPreviousState().LeftButton){
                        if(cursor.getIsLeftPressed()){
                            bool done = false;

                            //CLICKING ENTITY?
                            Vector2 point = cursor.getPos();
                            int selectionOffSetX = 0;
                            int selectionOffSetY = 0;
                            int selectionWidth = 0;
                            int selectionHeight = 0;

                            //find who i'm clicking
                            entityBank.ForEach(delegate(BaseGameEntity e)
                            {
                                if ((e is Unit) && (e as Unit).getCurrentState() == State.UnitState.Buried)
                                {
                                    //do nothing
                                }
                                else if ((e is Structure) && (e as Structure).getCurrentState() == State.StructureState.Destroyed)
                                {
                                    //do nothing
                                }
                                else
                                {
                                    if (e.visible)
                                    { //only check visible ones so we don't waste time

                                        //adjust the selection rectangle to account for different unit sizes
                                        if (e.getType() == State.EntityName.LightInfantry || e.getType() == State.EntityName.Archer)
                                        {
                                            selectionOffSetX = this.lightInfantrySouth.Width / 2;
                                            selectionOffSetY = (int)(this.lightInfantrySouth.Height * 0.80);
                                            selectionWidth = this.lightInfantryEast.Width;
                                            selectionHeight = this.lightInfantryEast.Height;
                                        }
                                        else if (e.getType() == State.EntityName.Camp)
                                        {
                                            selectionOffSetX = this.campTexturePlayer.Width / 2;
                                            selectionOffSetY = (int)(this.campTexturePlayer.Height * 0.80);
                                            selectionWidth = this.campTexturePlayer.Width;
                                            selectionHeight = this.campTexturePlayer.Height;
                                        }
                                        else if (e.getType() == State.EntityName.GuardTower)
                                        {
                                            selectionOffSetX = this.guardTowerTextureComputer.Width / 2;
                                            selectionOffSetY = (int)(this.guardTowerTextureComputer.Height * 0.80);
                                            selectionWidth = this.guardTowerTextureComputer.Width;
                                            selectionHeight = this.guardTowerTextureComputer.Height;
                                        }

                                        //Rectangle unit = new Rectangle(e.getLocation().x - selectionOffSetX, e.getLocation().y - selectionOffSetY, currentMap.getTileWidth(), currentMap.getTileHeight());
                                        Rectangle unit = new Rectangle(e.getLocation().x - selectionOffSetX, e.getLocation().y - selectionOffSetY, selectionWidth, selectionHeight);
                                        if (unit.Contains(new Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y))))
                                        {

                                            if (e.getPlayer() == State.PlayerId.COMPUTER && e is Unit)
                                            {
                                                selectedUnits = new List<BaseGameEntity>();
                                                selectedUnits.Add(e as Unit);
                                                numUnitsSelected = 1;
                                            }
                                            else if (e.getType() == State.EntityName.Camp || e.getType() == State.EntityName.GuardTower)
                                            {
                                                selectedUnits = new List<BaseGameEntity>();
                                                selectedUnits.Add(e as Structure);
                                                numUnitsSelected = 1;
                                            }
                                            else
                                            {
                                                if (numUnitsSelected == 1)
                                                {
                                                    if (selectedUnits[0].getPlayer() == State.PlayerId.COMPUTER)
                                                    {
                                                        selectedUnits = new List<BaseGameEntity>();
                                                        numUnitsSelected = 0;
                                                    }
                                                }
                                                bool newUnitIsSelected = selectedUnits.Contains(e as Unit);
                                                if (shiftPressed)
                                                {
                                                    //just add/remove new guy
                                                    if (newUnitIsSelected)
                                                    {
                                                        selectedUnits.Remove(e as Unit);
                                                        numUnitsSelected--;
                                                    }
                                                    else if ((e as Unit).getCurrentState() != State.UnitState.Dead)
                                                    {
                                                        selectedUnits.Add(e as Unit);
                                                        numUnitsSelected++;
                                                    }
                                                }
                                                else
                                                {
                                                    //shift not pressed

                                                    selectedUnits = new List<BaseGameEntity>();
                                                    selectedUnits.Add(e as Unit);
                                                    numUnitsSelected = 1;
                                                }
                                            }
                                            done = true;
                                        }
                                    } 
                                }
                            });
                            if(!done){   //not clicking a unit
                                selectedUnits = new List<BaseGameEntity>();
                                numUnitsSelected = 0;
                            }
                        }
                    }//end left button state change

                    //RIGHT BUTTON state has changed
                    if (cursor.getMouseState().RightButton != cursor.getPreviousState().RightButton)
                    {
                        bool done = false;
                        if (cursor.getIsRightPressed())
                        {
                            int selectionOffSetX = 0;
                            int selectionOffSetY = 0;
                            int selectionFillerX = 0;
                            int selectionFillerY = 0;

                            //clicking entity
                            entityBank.ForEach(delegate(BaseGameEntity e)
                            {
                                
                                if (e.visible && (((e is Unit) && (e as Unit).getCurrentState() != State.UnitState.Dead && (e as Unit).getCurrentState() != State.UnitState.Buried) || ((e is Structure) && (e as Structure).getCurrentState() != State.StructureState.Destroyed)))
                                { //only check visible ones so we don't waste time
                                    //adjust the selection rectangle to account for different unit sizes
                                    if (e.getType() == State.EntityName.LightInfantry || e.getType() == State.EntityName.Archer)
                                    {
                                        //since the textures are the same size
                                        selectionOffSetX = this.lightInfantrySouth.Width / 2;
                                        selectionOffSetY = (int)(this.lightInfantrySouth.Height * 0.80);
                                        selectionFillerX = this.lightInfantrySouth.Width;
                                        selectionFillerY = this.lightInfantrySouth.Height;
                                    }
                                    else if (e.getType() == State.EntityName.Camp)
                                    {
                                        selectionOffSetX = this.campTextureComputer.Width / 2;
                                        selectionOffSetY = (int)(this.campTextureComputer.Height * 0.80);
                                        selectionFillerX = this.campTextureComputer.Width;
                                        selectionFillerY = this.campTextureComputer.Height;
                                    }
                                    else if (e.getType() == State.EntityName.GuardTower)
                                    {
                                        selectionOffSetX = this.guardTowerTextureComputer.Width / 2;
                                        selectionOffSetY = (int)(this.guardTowerTextureComputer.Height * 0.80);
                                        selectionFillerX = this.guardTowerTextureComputer.Width;
                                        selectionFillerY = this.guardTowerTextureComputer.Height;
                                    }

                                    Rectangle unit = new Rectangle(e.getLocation().x - selectionOffSetX, e.getLocation().y - selectionOffSetY, selectionFillerX, selectionFillerY);
                                    if (unit.Contains(new Point(Convert.ToInt32(cursor.getPos().X), Convert.ToInt32(cursor.getPos().Y))))
                                    {
                                        //NOW, if unit is enemy, selected units attack!
                                        if (e.getPlayer() == State.PlayerId.COMPUTER)
                                        {
                                            selectedUnits.ForEach(delegate(BaseGameEntity u)
                                            {
                                                //e is the entity being clicked, and the target for all u
                                                if (u.getType() == State.EntityName.GuardTower)
                                                {
                                                    (u as GuardTowerStructure).attack(e);
                                                }
                                                else
                                                {
                                                    (u as Unit).attack(e);
                                                }
                                            });
                                        }
                                        done = true;
                                    }
                                }
                            });

                            //NOT ENTITY, SO MOVE SELECTED UNITS
                            if (!done && numUnitsSelected > 0)
                            {
                                selectedUnits.ForEach(delegate(BaseGameEntity u)
                                {
                                    if (u.getPlayer() == State.PlayerId.HUMAN && 
                                        u.getType() != State.EntityName.Camp && u.getType() != State.EntityName.GuardTower && u.getType() != State.EntityName.ControlPoint)
                                    {
                                        (u as Unit).move(new Coordinate(Convert.ToInt32(cursor.getPos().X), Convert.ToInt32(cursor.getPos().Y)), currentMap);
                                    }
                                });
                            }

                            done = true;
                        }
                    }

                    currentMap.update(keysPressed, 1024, 768);
                    
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
                    //draw the map
                    currentMap.draw(spriteBatch, cursor);

                    //draw units
                    drawEntity();

                    if (cursor.getIsRightPressed())
                    {
                        cursorAtClick = new Coordinate((int)cursor.getPos().X, (int)cursor.getPos().Y);
                        mouseClickDestination();
                    }

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

        public void removeEntity(int keyId)
        {
            if (keyId >= 0 && keyId <= entityBank.Count)
            {
                if (this.entityBank[keyId] is Unit)
                {
                    (this.entityBank[keyId] as Unit).setCurrentState(State.UnitState.Buried);
                }
                else if (this.entityBank[keyId] is Structure)
                {
                    (this.entityBank[keyId] as Structure).setCurrentState(State.StructureState.Destroyed);
                }
                
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
            entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e is Unit && (e as Unit).getCurrentState() == State.UnitState.Buried)
                {}
                else if (currentMap.isOnScreen(e.getLocation()))
                {
                    if (e.getType() == State.EntityName.LightInfantry)
                    {
                        e.visible = true;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

                        //depending on the unit's state, draw their textures
                        //idle
                        if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Idle)
                        {
                            //south or idle
                            if ((e as LightInfantryUnit).getDirection() == State.Direction.Still || (e as LightInfantryUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(this.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (this.lightInfantrySouth.Width / 2), onScreen.y - (int)(this.lightInfantrySouth.Height * 0.80),
                                    this.lightInfantrySouth.Width, this.lightInfantrySouth.Height), Color.White);
                            }
                            //east
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(this.lightInfantryEast,
                                    new Rectangle(onScreen.x - (this.lightInfantryEast.Width / 2), onScreen.y - (int)(this.lightInfantryEast.Height * 0.80),
                                    this.lightInfantryEast.Width, this.lightInfantryEast.Height), Color.White);
                            }
                            //west
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(this.lightInfantryWest,
                                    new Rectangle(onScreen.x - (this.lightInfantryWest.Width / 2), onScreen.y - (int)(this.lightInfantryWest.Height * 0.80),
                                    this.lightInfantryWest.Width, this.lightInfantryWest.Height), Color.White);
                            }
                            //north
                            else if ((e as LightInfantryUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(this.lightInfantryNorth,
                                    new Rectangle(onScreen.x - (this.lightInfantryNorth.Width / 2), onScreen.y - (int)(this.lightInfantryNorth.Height * 0.80),
                                    this.lightInfantryNorth.Width, this.lightInfantryNorth.Height), Color.White);
                            }

                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Attacking)
                        {
                            //TODO:
                            //Attacking Animation
                            spriteBatch.Draw(this.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (this.lightInfantrySouth.Width / 2), onScreen.y - (int)(this.lightInfantrySouth.Height * 0.80),
                                    this.lightInfantrySouth.Width, this.lightInfantrySouth.Height), Color.Red);
                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.Dead)
                        {
                            //TODO:
                            //Dead stuff
                            spriteBatch.Draw(this.lightInfantryDead,
                                    new Rectangle(onScreen.x - (this.lightInfantryDead.Width / 2), onScreen.y - (int)(this.lightInfantryDead.Height * 0.80),
                                    this.lightInfantryDead.Width, this.lightInfantryDead.Height), Color.White);
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
                        }
                        else if ((e as LightInfantryUnit).getCurrentState() == State.UnitState.UnderAttack)
                        {
                            //TODO:
                            //Under Attack Animation
                            spriteBatch.Draw(this.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (this.lightInfantrySouth.Width / 2), onScreen.y - (int)(this.lightInfantrySouth.Height * 0.80),
                                    this.lightInfantrySouth.Width, this.lightInfantrySouth.Height), Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(this.lightInfantrySouth,
                                    new Rectangle(onScreen.x - (this.lightInfantrySouth.Width / 2), onScreen.y - (int)(this.lightInfantrySouth.Height * 0.80),
                                    this.lightInfantrySouth.Width, this.lightInfantrySouth.Height), Color.White);
                        }
                        
                    }
                    else if (e.getType() == State.EntityName.Archer)
                    {
                        e.visible = true;
                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

                        //depending on the unit's state, draw their textures
                        //idle
                        if ((e as ArcherUnit).getCurrentState() == State.UnitState.Idle)
                        {
                            //south or idle
                            if ((e as ArcherUnit).getDirection() == State.Direction.Still || (e as ArcherUnit).getDirection() == State.Direction.South)
                            {
                                spriteBatch.Draw(this.archerSouth,
                                    new Rectangle(onScreen.x - (this.archerSouth.Width / 2), onScreen.y - (int)(this.archerSouth.Height * 0.80),
                                    this.archerSouth.Width, this.archerSouth.Height), Color.White);
                            }
                            //east
                            else if ((e as ArcherUnit).getDirection() == State.Direction.East)
                            {
                                spriteBatch.Draw(this.archerEast,
                                    new Rectangle(onScreen.x - (this.archerEast.Width / 2), onScreen.y - (int)(this.archerEast.Height * 0.80),
                                    this.archerEast.Width, this.archerEast.Height), Color.White);
                            }
                            //west
                            else if ((e as ArcherUnit).getDirection() == State.Direction.West)
                            {
                                spriteBatch.Draw(this.archerWest,
                                    new Rectangle(onScreen.x - (this.archerWest.Width / 2), onScreen.y - (int)(this.archerWest.Height * 0.80),
                                    this.archerWest.Width, this.archerWest.Height), Color.White);
                            }
                            //north
                            else if ((e as ArcherUnit).getDirection() == State.Direction.North)
                            {
                                spriteBatch.Draw(this.archerNorth,
                                    new Rectangle(onScreen.x - (this.archerNorth.Width / 2), onScreen.y - (int)(this.archerNorth.Height * 0.80),
                                    this.archerNorth.Width, this.archerNorth.Height), Color.White);
                            }

                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.Attacking)
                        {
                            //TODO:
                            //Attacking Animation
                            spriteBatch.Draw(this.archerSouth,
                                    new Rectangle(onScreen.x - (this.archerSouth.Width / 2), onScreen.y - (int)(this.archerSouth.Height * 0.80),
                                    this.archerSouth.Width, this.archerSouth.Height), Color.Red);
                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.Dead)
                        {
                            //TODO:
                            //Dead stuff
                            spriteBatch.Draw(this.archerDead,
                                    new Rectangle(onScreen.x - (this.archerDead.Width / 2), onScreen.y - (int)(this.archerDead.Height * 0.80),
                                    this.archerDead.Width, this.archerDead.Height), Color.White);
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
                        }
                        else if ((e as ArcherUnit).getCurrentState() == State.UnitState.UnderAttack)
                        {
                            //TODO:
                            //Under Attack Animation
                        }
                        else
                        {
                            spriteBatch.Draw(this.archerSouth,
                                    new Rectangle(onScreen.x - (this.archerSouth.Width / 2), onScreen.y - (int)(this.archerSouth.Height * 0.80),
                                    this.archerSouth.Width, this.archerSouth.Height), Color.White);
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
                                spriteBatch.Draw(this.campTexturePlayer,
                                    new Rectangle(onScreen.x - (this.campTexturePlayer.Width / 2), onScreen.y - (int)(this.campTexturePlayer.Height * 0.80),
                                    this.campTexturePlayer.Width, this.campTexturePlayer.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(this.campTextureComputer,
                                    new Rectangle(onScreen.x - (this.campTextureComputer.Width / 2), onScreen.y - (int)(this.campTextureComputer.Height * 0.80),
                                    this.campTextureComputer.Width, this.campTextureComputer.Height), Color.White);
                            }
                            
                        }
                        else if ((e as CampStructure).getCurrentState() == State.StructureState.Destroyed)
                        {
                            //TODO: building asploded
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {

                            }
                            else
                            {
                                spriteBatch.Draw(this.campTextureComputerExploded,
                                    new Rectangle(onScreen.x - (this.campTextureComputerDestroyed.Width / 2), onScreen.y - (int)(this.campTextureComputerDestroyed.Height * 0.80),
                                    this.campTextureComputerDestroyed.Width, this.campTextureComputerDestroyed.Height), Color.White);
                            }
                        }
                        else if((e as CampStructure).getCurrentState() == State.StructureState.Idle)
                        {
                            if (e.getPlayer() == State.PlayerId.HUMAN)
                            {
                                spriteBatch.Draw(this.campTexturePlayer,
                                    new Rectangle(onScreen.x - (this.campTexturePlayer.Width / 2), onScreen.y - (int)(this.campTexturePlayer.Height * 0.80),
                                    this.campTexturePlayer.Width, this.campTexturePlayer.Height), Color.White);
                            }
                            else
                            {
                                float ratio = (float)e.getHealth() / e.getMaxHealth();

                                if (ratio >= 0.50)
                                {
                                    spriteBatch.Draw(this.campTextureComputer,
                                        new Rectangle(onScreen.x - (this.campTextureComputer.Width / 2), onScreen.y - (int)(this.campTextureComputer.Height * 0.80),
                                        this.campTextureComputer.Width, this.campTextureComputer.Height), Color.White);
                                }
                                else if (ratio < 0.50 && ratio >= 0.25)
                                {
                                    spriteBatch.Draw(this.campTextureComputerDamaged,
                                        new Rectangle(onScreen.x - (this.campTextureComputer.Width / 2), onScreen.y - (int)(this.campTextureComputer.Height * 0.80),
                                        this.campTextureComputer.Width, this.campTextureComputer.Height), Color.White);
                                }
                                else if (ratio < 0.25 && ratio >= 0.00)
                                {
                                    spriteBatch.Draw(this.campTextureComputerDestroyed,
                                        new Rectangle(onScreen.x - (this.campTextureComputer.Width / 2), onScreen.y - (int)(this.campTextureComputer.Height * 0.80),
                                        this.campTextureComputer.Width, this.campTextureComputer.Height), Color.White);
                                }
                                
                            }
                        }

                    }
                    else if (e.getType() == State.EntityName.GuardTower)
                    {
                        e.visible = true;
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
                                spriteBatch.Draw(this.guardTowerTexturePlayer,
                                    new Rectangle(onScreen.x - (this.guardTowerTexturePlayer.Width / 2), onScreen.y - (int)(this.guardTowerTexturePlayer.Height * 0.80),
                                    this.guardTowerTexturePlayer.Width, this.guardTowerTexturePlayer.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(this.guardTowerTextureComputer,
                                    new Rectangle(onScreen.x - (this.guardTowerTextureComputer.Width / 2), onScreen.y - (int)(this.guardTowerTextureComputer.Height * 0.80),
                                    this.guardTowerTextureComputer.Width, this.guardTowerTextureComputer.Height), Color.White);
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
                                spriteBatch.Draw(this.guardTowerTexturePlayer,
                                    new Rectangle(onScreen.x - (this.guardTowerTexturePlayer.Width / 2), onScreen.y - (int)(this.guardTowerTexturePlayer.Height * 0.80),
                                    this.guardTowerTexturePlayer.Width, this.guardTowerTexturePlayer.Height), Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(this.guardTowerTextureComputer,
                                    new Rectangle(onScreen.x - (this.guardTowerTextureComputer.Width / 2), onScreen.y - (int)(this.guardTowerTextureComputer.Height * 0.80),
                                    this.guardTowerTextureComputer.Width, this.guardTowerTextureComputer.Height), Color.White);
                            }
                        }
                    }
                    else
                        e.visible = false;
                }

               
            });

            //show selection overlay on all visible selected units
            selectedUnits.ForEach(delegate(BaseGameEntity u)
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
                        xOffSet = (int)(this.lightInfantrySouth.Width / 2) + 10;
                        yOffSet = (int)(this.lightInfantrySouth.Height * 0.80) + 7;
                        width = lightInfantrySouth.Width + 20;
                        height = lightInfantrySouth.Height + 15;
                    }
                    else if (u.getType() == State.EntityName.Archer)
                    {
                        xOffSet = (int)(this.archerSouth.Width / 2) + 10;
                        yOffSet = (int)(this.archerSouth.Height * 0.80) + 7;
                        width = archerSouth.Width + 20;
                        height = archerSouth.Height + 15;
                    }
                    else if (u.getType() == State.EntityName.Camp)
                    {
                        xOffSet = (int)(this.campTextureComputer.Width / 2) + 32;
                        yOffSet = (int)(this.campTextureComputer.Height * 0.80) + 15;
                        width = campTextureComputer.Width + 50;
                        height = campTextureComputer.Height + 40;
                    }
                    else if (u.getType() == State.EntityName.GuardTower)
                    {
                        xOffSet = (int)(this.guardTowerTextureComputer.Width / 2) + 18;
                        yOffSet = (int)(this.guardTowerTextureComputer.Height * 0.80) + 20;
                        width = guardTowerTextureComputer.Width + 30;
                        height = guardTowerTextureComputer.Height + 40;
                    }

                    spriteBatch.Draw(this.selectedUnitOverlayTexture,
                        new Rectangle(onScreen.x - xOffSet, onScreen.y - yOffSet, width, height),
                        Color.White);

                    if (u.getPlayer() == State.PlayerId.HUMAN)
                    {
                        spriteBatch.Draw(this.healthRatioTexture,
                            new Rectangle(onScreen.x - xOffSet + 1 + (int)(width * healthBarTypicalStartWidth), onScreen.y - yOffSet + 1 + (int)(height * healthBarTypicalStartHeight), (int)((width * healthBarTypicalWidth) * healthRatio), (int)(height * healthBarTypicalHeight)),
                            Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(this.healthRatioTexture,
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
    }
}
