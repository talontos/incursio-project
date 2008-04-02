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
            #endregion

            #region MOUSE COMMANDS

            Vector2 point = MapManager.getInstance().currentMap.translateClickToMapLocation(mouseStateCurrent.X, mouseStateCurrent.Y);
            
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
    }
}
