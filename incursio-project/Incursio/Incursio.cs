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
        List<Unit> selectedUnits;
        int numUnitsSelected;

        //Overlay for selected units
        Texture2D selectedUnitOverlayTexture;
        Texture2D healthRatioTexture;

        //unit texture
        Texture2D lightInfantryUnitTexture;

        //map initialization
        BaseMapEntity tex1;
        public MapBase currentMap;

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
            textureBank = new List<Texture2D>();

            //NOTE: the map should be instantiated before units
            //TODO: test map, DELETE THESE LINES
            currentMap = new MapBase(2048, 1024, 1024, 768);
            
            //testing unit creation/placement/moving///
            LightInfantryUnit infUnit1 = (LightInfantryUnit) factory.create("Incursio.Classes.LightInfantryUnit", State.PlayerId.HUMAN);
            LightInfantryUnit infUnit2 = (LightInfantryUnit) factory.create("Incursio.Classes.LightInfantryUnit", State.PlayerId.HUMAN);
            LightInfantryUnit infUnit3 = (LightInfantryUnit) factory.create("Incursio.Classes.LightInfantryUnit", State.PlayerId.COMPUTER);
            //infUnit1.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            infUnit1.setLocation(new Coordinate(300, 300));
            infUnit2.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            infUnit3.setLocation(new Coordinate(500, 500)); //for ease of testing
            //infUnit3.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            
            infUnit1.setHealth(80);
            infUnit2.setHealth(80);
            infUnit3.setHealth(50);

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

            selectedUnits = new List<Unit>();
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

            //load testmap texture
            tex1 = new BaseMapEntity(Content.Load<Texture2D>(@"grass"));
            for(int j = 0; j < 32; j++)
            {
                for(int i = 0; i < 64; i++)
                {
                    currentMap.addMapEntity(tex1, i, j);
                }
            }

            tex1 = new BaseMapEntity(Content.Load<Texture2D>(@"barrel"));
            currentMap.addMapEntity(tex1, 5, 6);
            currentMap.addMapEntity(tex1, 16, 2);
            currentMap.addMapEntity(tex1, 60, 21);
            currentMap.addMapEntity(tex1, 2, 19);
            currentMap.addMapEntity(tex1, 39, 60);

            //load unit textures
            lightInfantryUnitTexture = Content.Load<Texture2D>(@"infantryUnit");

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

                    selectedUnits = hud.update(cursor, selectedUnits, numUnitsSelected);
                    numUnitsSelected = hud.getNumUnits();

                    entityBank.ForEach(delegate(BaseGameEntity e)
                    {
                        e.Update(gameTime);
                    });
                    
                    //LEFT BUTTON state has changed
                    if(cursor.getMouseState().LeftButton != cursor.getPreviousState().LeftButton){
                        if(cursor.getIsLeftPressed()){
                            bool done = false;

                            //CLICKING ENTITY?
                            Vector2 point = cursor.getPos();
                            entityBank.ForEach(delegate(BaseGameEntity e)
                            {
                                if(e.visible){ //only check visible ones so we don't waste time
                                    Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
                                    if(unit.Contains( new Point( Convert.ToInt32(point.X), Convert.ToInt32(point.Y)) ) ){

                                        if(selectedUnits.Contains(e as Unit)){
                                            selectedUnits.Remove(e as Unit);
                                            numUnitsSelected--;
                                        }

                                        else{
                                            selectedUnits.Add(e as Unit);
                                            numUnitsSelected++;
                                        }

                                        done = true;
                                    }
                                }
                            });
                        }
                    }//end left button state change

                    //RIGHT BUTTON state has changed
                    if (cursor.getMouseState().RightButton != cursor.getPreviousState().RightButton)
                    {
                        bool done = false;
                        if (cursor.getIsRightPressed())
                        {
                            //clicking entity
                            entityBank.ForEach(delegate(BaseGameEntity e)
                            {
                                if (e.visible)
                                { //only check visible ones so we don't waste time
                                    Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
                                    if (unit.Contains(new Point(Convert.ToInt32(cursor.getPos().X), Convert.ToInt32(cursor.getPos().Y))))
                                    {
                                        //NOW, if unit is enemy, selected units attack!
                                        if (e.getPlayer() == State.PlayerId.COMPUTER)
                                        {
                                            selectedUnits.ForEach(delegate(Unit u)
                                            {
                                                u.attack(e);
                                            });
                                        }
                                    }
                                }
                            });

                            //NOT ENTITY, SO MOVE SELECTED UNITS
                            if (!done && numUnitsSelected > 0)
                            {
                                selectedUnits.ForEach(delegate(Unit u)
                                {
                                    if (u.getPlayer() == State.PlayerId.HUMAN)
                                    {
                                        u.move(new Coordinate(Convert.ToInt32(cursor.getPos().X), Convert.ToInt32(cursor.getPos().Y)), currentMap);
                                    }
                                });
                            }

                            done = true;
                        }
                    }

                    for(int i = 0; i < keysPressed.Length; i++){
                        switch(keysPressed[i]){
                            case Keys.Escape:
                                //this is tempory, as there is no other current way to unselect a unit
                                selectedUnits.Clear();
                                numUnitsSelected = 0;
                                //currentState = State.GameState.PausedPlay; break;
                                break;
                            case Keys.Enter://just so we can have a breakpoint whenever we want...
                                currentState = currentState; break;
                            default: break;
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
                if (currentMap.isOnScreen(e.getLocation()))
                {
                    if (e.getType() == State.EntityName.LightInfantry)
                    {
                        e.visible = true;

                        onScreen = currentMap.positionOnScreen(e.getLocation());
                        Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

                        //temporary conditional to see if units are really attacking
                        if ((e as Unit).getCurrentState() == State.UnitState.Attacking)
                        {
                            spriteBatch.Draw(this.lightInfantryUnitTexture,
                                new Rectangle(onScreen.x, onScreen.y, this.lightInfantryUnitTexture.Width, this.lightInfantryUnitTexture.Height),
                                Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(this.lightInfantryUnitTexture,
                                new Rectangle(onScreen.x, onScreen.y, this.lightInfantryUnitTexture.Width, this.lightInfantryUnitTexture.Height),
                                Color.White);
                        }
                        
                    }
                    else
                        e.visible = false;
                }
               
            });

            //show selection overlay on all visible selected units
            selectedUnits.ForEach(delegate(Unit u)
            {
                if(currentMap.isOnScreen(u.getLocation())){
                    onScreen = currentMap.positionOnScreen(u.getLocation());


                    healthRatio = (float)u.getHealth() / u.getMaxHealth();

                    spriteBatch.Draw(this.selectedUnitOverlayTexture,
                        new Rectangle(onScreen.x, onScreen.y, this.selectedUnitOverlayTexture.Width, this.selectedUnitOverlayTexture.Height),
                        Color.White);

                    if (u.getPlayer() == State.PlayerId.HUMAN)
                    {
                        spriteBatch.Draw(this.healthRatioTexture,
                            new Rectangle(onScreen.x + (int)(this.selectedUnitOverlayTexture.Width * healthBarTypicalStartWidth), onScreen.y + (int)(this.selectedUnitOverlayTexture.Height * healthBarTypicalStartHeight), (int)((this.selectedUnitOverlayTexture.Width * healthBarTypicalWidth) * healthRatio), (int)(this.selectedUnitOverlayTexture.Height * healthBarTypicalHeight)),
                            Color.Lime);
                    }
                    else
                    {
                        spriteBatch.Draw(this.healthRatioTexture,
                            new Rectangle(onScreen.x + (int)(this.selectedUnitOverlayTexture.Width * healthBarTypicalStartWidth), onScreen.y + (int)(this.selectedUnitOverlayTexture.Height * healthBarTypicalStartHeight), (int)((this.selectedUnitOverlayTexture.Width * healthBarTypicalWidth) * healthRatio), (int)(this.selectedUnitOverlayTexture.Height * healthBarTypicalHeight)),
                            Color.Red);
                    }
                   
                }
            });
        }


        public void selectEntity()
        {
            
        }
    }
}
