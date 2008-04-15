using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;

namespace Incursio.Classes.Terrain
{
    public class Grass : BaseMapEntity
    {
        public Grass(int x, int y) : base(TextureBank.MapTiles.grass, true, x, y){
            
        }
    }
}
