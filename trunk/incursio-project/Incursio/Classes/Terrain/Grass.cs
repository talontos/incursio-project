using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;

namespace Incursio.Classes.Terrain
{
    public class Grass : BaseMapEntity
    {
        public Grass(int x, int y){
            this.passable = true;
            this.location = new global::Incursio.Utils.Coordinate(x, y);

            //TODO: CHANGE BOUNDS TO SPAN NUMBER OF GRASS TILES
            int rnd = Incursio.rand.Next(0, 1);
            switch(rnd){
                /*
                case 0:
                    this.texture = TextureBank.MapTiles.grass1;
                case 1:
                    this.texture = TextureBank.MapTiles.grass2;
                case 2:
                    this.texture = TextureBank.MapTiles.grass3;
                */
                default:
                    this.texture = TextureBank.MapTiles.grass1;
                    break;
            }
        }
    }
}
