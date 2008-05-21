using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Incursio.Entities;
using Microsoft.Xna.Framework;
using Incursio.Managers;

namespace Incursio.Interface
{
    public class BuildEntityPanel
    {
        private Vector2 panelOrigin = new Vector2(775, 587);
        private int buttonWidth = 60;
        private int buttonHeight = 45;
        private int buttonBuffer = 5;

        private List<BuildEntityButton> entities;

        public BuildEntityPanel(){
            int i = 0, j = 0;
            int x = (int)panelOrigin.X;
            int y = (int)panelOrigin.Y;

            entities = new List<BuildEntityButton>();

            ObjectFactory.getInstance().entities.ForEach(delegate(BaseGameEntityConfiguration c)
            {
                if(entities.Count < 9 && c.costToBuild > 0){
                    //TODO: READ IMAGE FROM CONFIGURATION; NOT BANK
                    entities.Add(new BuildEntityButton(c, new Vector2(x, y), TextureBank.InterfaceTextures.lightInfantryIcon));

                    i += 1;

                    //iterate positions
                    x = (int)(panelOrigin.X + ((buttonWidth + buttonBuffer) * i));

                    //Right now only 3 buttons can fit in each row of the panel
                    if (i % 3 == 0)
                    {
                        y = (int)(panelOrigin.Y + ((buttonHeight + buttonBuffer) * ++j));
                        x = (int)panelOrigin.X;
                        i = 0;
                    }
                }
            });
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch){
            foreach(BuildEntityButton b in entities){
                b.Draw(batch);
            }
        }

        public void Update(Cursor cursor){
            foreach (BuildEntityButton b in entities){
                b.Update(cursor);
            }
        }
    }
}
