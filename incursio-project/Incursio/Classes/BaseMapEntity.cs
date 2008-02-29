using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Incursio.Utils;

namespace Incursio.Classes
{
  public class BaseMapEntity
    {
        public Coordinate location;
        public Boolean passable = true;

        //graphic?
        public Texture2D texture;

        public BaseMapEntity(){

        }

        public BaseMapEntity(Texture2D tex)
        {
            this.texture = tex;
        }
    }
}