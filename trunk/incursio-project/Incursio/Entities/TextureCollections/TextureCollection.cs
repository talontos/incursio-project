using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.TextureCollections
{
    public class TextureCollection
    {
        public int id;
        public string name;

        public StillTextures still;
        public MovementTextures movement;
        public AttackTextures attacking;
        public DeathTextures death;
        public DamagedTextures damaged;

        public TextureCollection(int id, string name){
            this.id = id;
            this.name = name;
        }
    }
}
