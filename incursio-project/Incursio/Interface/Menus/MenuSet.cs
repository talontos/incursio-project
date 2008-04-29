using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Managers;

namespace Incursio.Interface.Menus
{
    public class MenuSet
    {
        private List<Button> buttons;
        private Rectangle boundingBox;

        private int xPad = 5,
                    yPad = 3;

        public MenuSet(int x, int y, params Button[] args)
        {
            Button curBut;
            int yPos = y + yPad + 1;
            this.buttons = new List<Button>();

            for(int i = 0; i < args.Length; i++){
                curBut = args[i];
                curBut.position.X = x + xPad;
                curBut.position.Y = yPos;

                this.buttons.Add(curBut);

                yPos += yPad + 30;
            }

            this.boundingBox = new Rectangle(x, y, (2*xPad + 100), ( buttons.Count * 30 + (buttons.Count + 2) * yPad ));
        }

        public void Draw(SpriteBatch batch){
            //draw background
            batch.Draw(TextureBank.InterfaceTextures.selectionRectangle, boundingBox, Color.DarkBlue);

            //draw buttons
            this.buttons.ForEach(delegate(Button b)
            {
                b.Draw(batch);
            });
        }

        public void Update(Cursor cursor){
            this.buttons.ForEach(delegate(Button b)
            {
                b.Update(cursor);
            });
        }

    }
}
