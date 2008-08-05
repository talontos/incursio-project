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
        private Vector2 panelOrigin = new Vector2(780, 610);
        private int buttonWidth = 60;
        private int buttonHeight = 45;
        private int buttonBuffer = 5;

        private List<BuildEntityButton> entities;

        public BuildEntityPanel(List<int> entityIds){
            int i = 0, j = 0;
            int x = (int)panelOrigin.X;
            int y = (int)panelOrigin.Y;

            entities = new List<BuildEntityButton>();

            foreach(int id in entityIds)
            {
                BaseGameEntityConfiguration c = ObjectFactory.getInstance().entities[id];

                if(entities.Count < 9 && c.costToBuild > 0){
                    //easiest way to get icon...by using the object factory directly, these entities are
                    //  NOT added to the entityBank.
                    //TODO: Find a more efficient way?
                    BaseGameEntity en = ObjectFactory.getInstance().create(c.classID, -1);

                    entities.Add(new BuildEntityButton(c, new Vector2(x, y), en.renderComponent.textures.icon));

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
            }
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
