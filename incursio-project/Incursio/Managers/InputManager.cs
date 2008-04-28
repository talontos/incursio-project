using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Incursio.Classes;
using Incursio.Utils;

namespace Incursio.Managers
{
    public class InputManager
    {
        #region VARIABLE DECLARATION

        public MouseState mouseStateCurrent;
        public MouseState mouseStatePrev;

        public KeyboardState keyStateCurrent;
        public KeyboardState keyStatePrev;

        public Vector2 mouseDragStart = new Vector2(-1,-1);
        public Vector2 mouseDragEnd = new Vector2(-1, -1);
        public Vector2 mouseDragStart_shifted = new Vector2(-1, -1);
        public Vector2 mouseDragEnd_shifted = new Vector2(-1, -1);

        private bool enterStringMode = false;
        private string enteredString = "";

        public bool MOVE_UP = false,
                    MOVE_LEFT = false,
                    MOVE_DOWN = false,
                    MOVE_RIGHT = false;

        //public int dragDetectCounter = 0;
        public bool dragging = false;

        public bool positioningTower = false;

        #endregion

        private static InputManager instance;

        public static InputManager getInstance(){
            if(instance == null)
                instance = new InputManager();

            return instance;
        }

        public void Update(GameTime gameTime){
            //set current states
            mouseStateCurrent = Mouse.GetState();
            keyStateCurrent = Keyboard.GetState();

            if(enterStringMode){
                
            }

            //interface
            if(this.keyPressed(Keys.Escape))
                Incursio.getInstance().pause_play();

            if(this.keyPressed(Keys.F11)){
                //Set Full-Screen
                Incursio.getInstance().toggleFullScreen();
            }

            if(this.keyPressed(Keys.Enter)){
                if(enterStringMode){
                    //perform action
                }

                this.enterStringMode = !this.enterStringMode;
                this.enteredString = "";
            }

            //only update these things if game is in play
            if(Incursio.getInstance().currentState == State.GameState.InPlay && !enterStringMode){
                this.Update_PlayState(gameTime);
            }

            //save states for previous
            mouseStatePrev = mouseStateCurrent;
            keyStatePrev = keyStateCurrent;
        }

        private void Update_PlayState(GameTime gameTime){
            
            #region KEYBOARD COMMANDS
            
            this.clearMoveDirections();

            if (keyStateCurrent.IsKeyDown(Keys.W) || keyStateCurrent.IsKeyDown(Keys.Up))
            {
                MOVE_UP = true;
            }
            else if (keyStateCurrent.IsKeyDown(Keys.S) || keyStateCurrent.IsKeyDown(Keys.Down))
            {
                MOVE_DOWN = true;
            }

            if (keyStateCurrent.IsKeyDown(Keys.A) || keyStateCurrent.IsKeyDown(Keys.Left))
            {
                MOVE_LEFT = true;
            }
            else if (keyStateCurrent.IsKeyDown(Keys.D) || keyStateCurrent.IsKeyDown(Keys.Right))
            {
                MOVE_RIGHT = true;
            }

            //Entity construction
            if (this.keyPressed(Keys.L))
            {
                EntityManager.getInstance().tryToBuild(new LightInfantryUnit());// LightInfantryUnit.CLASSNAME);
            }

            if (this.keyPressed(Keys.R))
            {
                EntityManager.getInstance().tryToBuild(new ArcherUnit());// ArcherUnit.CLASSNAME);
            }

            if (this.keyPressed(Keys.H))
            {
                EntityManager.getInstance().tryToBuild(new HeavyInfantryUnit());// HeavyInfantryUnit.CLASSNAME);
            }

            if (this.keyPressed(Keys.T))
            {
                //try to build guard Tower
                positioningTower = true;
                TextureBank.InterfaceTextures.cursorEvent = TextureBank.EntityTextures.guardTowerTexturePlayer;
            }

            if (this.keyPressed(Keys.C))
            {
                //select camp
                EntityManager.getInstance().selectPlayerCamp();
                MapManager.getInstance().currentMap.moveCameraToEvent(EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.HUMAN)[0].location);
            }

            if (this.keyPressed(Keys.E))
            {
                EntityManager.getInstance().selectPlayerHero();
                MapManager.getInstance().currentMap.moveCameraToEvent(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].location);
            }

            //unit commands
            if (this.keyPressed(Keys.Q))
            {
                EntityManager.getInstance().issueCommand(State.Command.STOP, false, null);
            }

            if (this.keyPressed(Keys.G))
            {
                EntityManager.getInstance().issueCommand(State.Command.GUARD, false, null);
            }

            if (this.keyPressed(Keys.Space))
            {
                //TODO: RESPOND TO MESSAGES
                Coordinate coord = MessageManager.getInstance().getLastMessageLocation();
                if (coord != null)
                {
                    Incursio.getInstance().currentMap.moveCameraToEvent(MessageManager.getInstance().getLastMessageLocation());
                }

            }

            if(this.keyPressed(Keys.Delete)){
                EntityManager.getInstance().killSelectedUnits();
            }

            if(this.keyPressed(Keys.X)){
                //Cancel current command from Camp, if selected
                //EntityManager.getInstance().cancelCurrentBuildOrder(State.PlayerId.HUMAN);
            }

            #region GROUP_SET_SELECT
            if (this.keyPressed(Keys.NumPad0) || this.keyPressed(Keys.D0))
            {
                setSelectGroup(0);
            }

            if (this.keyPressed(Keys.NumPad1) || this.keyPressed(Keys.D1))
            {
                setSelectGroup(1);
            }

            if (this.keyPressed(Keys.NumPad2) || this.keyPressed(Keys.D2))
            {
                setSelectGroup(2);
            }

            if (this.keyPressed(Keys.NumPad3) || this.keyPressed(Keys.D3))
            {
                setSelectGroup(3);
            }

            if (this.keyPressed(Keys.NumPad4) || this.keyPressed(Keys.D4))
            {
                setSelectGroup(4);
            }

            if (this.keyPressed(Keys.NumPad5) || this.keyPressed(Keys.D5))
            {
                setSelectGroup(5);
            }

            if (this.keyPressed(Keys.NumPad6) || this.keyPressed(Keys.D6))
            {
                setSelectGroup(6);
            }

            if (this.keyPressed(Keys.NumPad7) || this.keyPressed(Keys.D7))
            {
                setSelectGroup(7);
            }

            if (this.keyPressed(Keys.NumPad8) || this.keyPressed(Keys.D8))
            {
                setSelectGroup(8);
            }

            if (this.keyPressed(Keys.NumPad9) || this.keyPressed(Keys.D9))
            {
                setSelectGroup(9);
            }
            #endregion

            #endregion

            #region MOUSE COMMANDS

            Vector2 point = MapManager.getInstance().currentMap.translateClickToMapLocation(mouseStateCurrent.X, mouseStateCurrent.Y);
            Vector2 prevPoint = MapManager.getInstance().currentMap.translateClickToMapLocation(mouseStatePrev.X, mouseStatePrev.Y);

            dragging = this.leftDragStart() || dragging;

            if (dragging)
            {
                //listen for mouseUp
                if (mouseStateCurrent.LeftButton == ButtonState.Released)
                {

                    //end drag event

                    if (mouseDragStart.X >= 0 && mouseDragEnd.X >= 0)
                    {
                        //finished drag-selecting; select all units in rectangle
                        EntityManager.getInstance().updateUnitSelection(getSelectionRectangle());

                        mouseDragStart = new Vector2(-1, -1);
                        mouseDragEnd = new Vector2(-1, -1);
                        mouseDragStart_shifted = new Vector2(-1, -1);
                        mouseDragEnd_shifted = new Vector2(-1, -1);

                    }

                    dragging = false;
                    //dragDetectCounter = 0;
                }
                else
                {
                    //keep track of positions
                    if (mouseDragStart.X < 0)
                    {
                        mouseDragStart_shifted = prevPoint;
                        mouseDragStart = new Vector2(mouseStatePrev.X, mouseStatePrev.Y);
                    }

                    mouseDragEnd_shifted = prevPoint;
                    mouseDragEnd = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                }
            }

            if (this.leftClick())
            {
                //If the cursor is clicking within the HUD
                if (Incursio.getInstance().hud.isCursorWithin((int)mouseStateCurrent.X, (int)mouseStateCurrent.Y))
                {

                }
                else if (this.positioningTower)
                {
                    EntityManager.getInstance().tryToBuild(new GuardTowerStructure(), point);
                    this.positioningTower = false;
                    TextureBank.InterfaceTextures.cursorEvent = null;
                }
                else
                {
                    EntityManager.getInstance().updateUnitSelection(point);
                }
            }

            if (this.rightClick())
            {
                //If the cursor is not clicking within the HUD
                if (!Incursio.getInstance().hud.isCursorWithin((int)mouseStateCurrent.X, (int)mouseStateCurrent.Y))
                {
                    EntityManager.getInstance().updateUnitOrders(point);
                }
            }

            #endregion
        }

        public bool keyPressed(Keys key){
            return (keyStateCurrent.IsKeyDown(key) && keyStatePrev.IsKeyUp(key));
        }

        public bool shifting(){
            return (keyStateCurrent.IsKeyDown(Keys.LeftShift) || keyStateCurrent.IsKeyDown(Keys.RightShift));
        }

        public bool alting(){
            return (keyStateCurrent.IsKeyDown(Keys.LeftAlt) || keyStateCurrent.IsKeyDown(Keys.RightAlt));
        }

        public bool ctrling()
        {
            return (keyStateCurrent.IsKeyDown(Keys.LeftControl) || keyStateCurrent.IsKeyDown(Keys.RightControl));
        }

        public bool rightClick(){
            return (mouseStateCurrent.RightButton == ButtonState.Pressed
                 && mouseStatePrev.RightButton == ButtonState.Released);
        }

        public bool leftClick(){
            return (mouseStateCurrent.LeftButton == ButtonState.Pressed
                 && mouseStatePrev.LeftButton == ButtonState.Released);
        }

        public bool leftDragEnd(){
            return (mouseStateCurrent.LeftButton == ButtonState.Released
                && mouseStatePrev.LeftButton == ButtonState.Pressed) ;
        }

        public bool leftDragStart(){
            return (mouseStateCurrent.LeftButton == ButtonState.Pressed
                && mouseStatePrev.LeftButton == ButtonState.Pressed
                &&
                (mouseStateCurrent.X != mouseStatePrev.X
                || mouseStateCurrent.Y != mouseStatePrev.Y)) ;
        }

        public Rectangle getSelectionRectangle()
        {
            //TODO: translate to current viewpoint!!!!
            return new Rectangle(
                Math.Min((int)mouseDragStart_shifted.X, (int)mouseDragEnd_shifted.X),
                Math.Min((int)mouseDragStart_shifted.Y, (int)mouseDragEnd_shifted.Y),
                Math.Abs((int)mouseDragStart_shifted.X - (int)mouseDragEnd_shifted.X),
                Math.Abs((int)mouseDragStart_shifted.Y - (int)mouseDragEnd_shifted.Y)
             );
        }

        public Rectangle getVisibleSelectionRectangle()
        {
            return new Rectangle(
                Math.Min((int)mouseDragStart.X, (int)mouseDragEnd.X),
                Math.Min((int)mouseDragStart.Y, (int)mouseDragEnd.Y),
                Math.Abs((int)mouseDragStart.X - (int)mouseDragEnd.X),
                Math.Abs((int)mouseDragStart.Y - (int)mouseDragEnd.Y)
             );
        }

        public void drawSelectionRectangle(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch){
            if (mouseDragStart.X < 0 || mouseDragEnd.X < 0)
                return;

            spriteBatch.Draw(TextureBank.InterfaceTextures.selectionRectangle, 
                    getVisibleSelectionRectangle(), 
                    Microsoft.Xna.Framework.Graphics.Color.Blue
            );
        }

        private void setSelectGroup(int groupNum){
            if(ctrling()){
                //assigning group
                EntityManager.getInstance().assignGroup(groupNum);
            }
            else{
                //selecting group
                EntityManager.getInstance().selectGroup(groupNum);
            }
        }

        private void clearMoveDirections(){
            this.MOVE_DOWN = false;
            this.MOVE_LEFT = false;
            this.MOVE_RIGHT = false;
            this.MOVE_UP = false;
        }
    }
}
