/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Josh Amick
 * 
 * All Rights Reserved
 ***************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Managers;
using Incursio.Utils;
using Incursio.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Interface
{
    public class BuildEntityButton : Button
    {
        private Vector2 stringpos;

        private int eConfigId = -1;
        private int eCost = 0;
        private string name = "";

        private bool cursorHover = false;
        private Vector2 cursorHoverPos;


        public BuildEntityButton(BaseGameEntityConfiguration c, Vector2 position, Texture2D image) :
            base(position, image, image)
        {
            this.eConfigId = c.classID;
            this.name = c.className;
            this.eCost = c.costToBuild;

            stringpos = new Vector2(position.X + 5, position.Y + 23);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);

            batch.Draw(TextureBank.InterfaceTextures.moneyIcon,
                new Rectangle((int)stringpos.X, (int)stringpos.Y, TextureBank.InterfaceTextures.moneyIcon.Width, TextureBank.InterfaceTextures.moneyIcon.Height), Color.White);

            batch.DrawString(Incursio.getInstance().getFont_Arial(), "     " + this.eCost, this.stringpos, Color.Gold);

            if(cursorHover){
                //TODO: DRAW A BOX AROUND IT?

                //draw what I build at the cursor
                batch.DrawString(Incursio.getInstance().getFont_Arial(), this.name, this.cursorHoverPos, Color.White);
            }
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                EntityManager.getInstance().tryToBuild(this.eConfigId);
            }

            //cursor is over me
            if ((new Rectangle((int)this.position.X, (int)this.position.Y, this.pressed.Width, this.pressed.Height).Contains(
                    new Point((int)cursor.getPos().X, (int)cursor.getPos().Y))))
            {
                if(!cursorHover){
                    cursorHoverPos = cursor.getPos();
                }

                cursorHover = true;
            }
            else{
                cursorHover = false;
            }
        }
    }
}
