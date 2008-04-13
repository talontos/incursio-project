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

            #region KEYBOARD COMMANDS
            //interface
            if(this.keyPressed(Keys.Escape))
                Incursio.getInstance().pause_play();

            //Entity construction
            if(this.keyPressed(Keys.L)){
                EntityManager.getInstance().tryToBuild(new LightInfantryUnit());// LightInfantryUnit.CLASSNAME);
            }

            if (this.keyPressed(Keys.A))
            {
                EntityManager.getInstance().tryToBuild(new ArcherUnit());// ArcherUnit.CLASSNAME);
            }

            if (this.keyPressed(Keys.H))
            {
                EntityManager.getInstance().tryToBuild(new HeavyInfantryUnit());// HeavyInfantryUnit.CLASSNAME);
            }

            if(this.keyPressed(Keys.T))
            {
                //try to build guard Tower
                positioningTower = true;
                TextureBank.InterfaceTextures.cursorEvent = TextureBank.EntityTextures.guardTowerTexturePlayer;
            }

            if(this.keyPressed(Keys.C)){
                //try to build Camp
            }

            //unit commands
            if(this.keyPressed(Keys.S)){
                EntityManager.getInstance().issueCommand(State.Command.STOP, false, null);
            }

            if(this.keyPressed(Keys.G)){
                EntityManager.getInstance().issueCommand(State.Command.GUARD, false, null);
            }

            if (this.keyPressed(Keys.Enter))
            {

            }


            #endregion

            #region MOUSE COMMANDS

            Vector2 point = MapManager.getInstance().currentMap.translateClickToMapLocation(mouseStateCurrent.X, mouseStateCurrent.Y);
            Vector2 prevPoint = MapManager.getInstance().currentMap.translateClickToMapLocation(mouseStatePrev.X, mouseStatePrev.Y);

            dragging = this.leftDragStart() || dragging;

            if(dragging){
                //listen for mouseUp
                if(mouseStateCurrent.LeftButton == ButtonState.Released){

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
                else{
                    //keep track of positions
                    if (mouseDragStart.X < 0){
                        mouseDragStart_shifted = prevPoint;
                        mouseDragStart = new Vector2(mouseStatePrev.X, mouseStatePrev.Y);
                    }

                    mouseDragEnd_shifted = prevPoint;
                    mouseDragEnd = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                }
            }
            
            if(this.leftClick()){
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

            if(this.rightClick()){
                //If the cursor is not clicking within the HUD
                if (!Incursio.getInstance().hud.isCursorWithin((int)mouseStateCurrent.X, (int)mouseStateCurrent.Y))
                {
                    EntityManager.getInstance().updateUnitOrders(point);
                }  
            }

            #endregion

            //save states for previous
            mouseStatePrev = mouseStateCurrent;
            keyStatePrev = keyStateCurrent;
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

            spriteBatch.Draw(TextureBank.InterfaceTextures.exitGameToMenuButton, 
                    getVisibleSelectionRectangle(), 
                    Microsoft.Xna.Framework.Graphics.Color.Blue
            );
        }
    }
}
