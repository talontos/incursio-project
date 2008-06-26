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

        public BuildEntityButton(BaseGameEntityConfiguration c, Vector2 position, Texture2D image) :
            base(position, image, image)
        {
            this.eConfigId = c.classID;
            this.name = c.className;
            this.eCost = c.costToBuild;

            stringpos = new Vector2(position.X, position.Y + 23);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);

            batch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.moneyIcon.texture,
                new Rectangle((int)stringpos.X, (int)stringpos.Y, TextureBank.getInstance().InterfaceTextures.interfaceTextures.moneyIcon.texture.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.moneyIcon.texture.Height), Color.White);

            batch.DrawString(Incursio.getInstance().getFont_Arial(), "     " + this.eCost, this.stringpos, Color.Gold);
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);

                if(ObjectFactory.getInstance().entities[eConfigId].isStructure){
                    //we need to place it before we start construction.
                    cursor.beginPlaceStructure(ObjectFactory.getInstance().entities[eConfigId]);
                }
                else{
                    EntityManager.getInstance().tryToBuild(this.eConfigId);
                }
            }

            //cursor is over me
            if ((new Rectangle((int)this.position.X, (int)this.position.Y, this.pressed.Width, this.pressed.Height).Contains(
                    new Point((int)cursor.getPos().X, (int)cursor.getPos().Y))))
            {
                Cursor.getInstance().tooltip = this.name;
            }
            else if(Cursor.getInstance().tooltip == name){
                Cursor.getInstance().tooltip = null;
            }
        }
    }
}
