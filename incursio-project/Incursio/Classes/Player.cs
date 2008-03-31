using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Microsoft.Xna.Framework;

using Incursio.Utils;

namespace Incursio.Classes
{
    public class Player
    {
        public String name = "Player";
        public int heroId;
        public long MONETARY_UNIT = 0;
        public int ownedControlPoints = 0;
        public State.PlayerId id;

        public List<GameEvent> events;


        public Player(){
            events = new List<GameEvent>();
        }

        public Player(String playerName) : base()
        {
            this.name = playerName;
        }

        public virtual void update(GameTime gameTime){
            //update player-specifics
            //money
        }
    }
}