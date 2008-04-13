using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Managers;
using Incursio.Commands;

namespace Incursio.Classes
{
    /// <summary>
    /// Control Points are structures, but also not.  They cannot build anything, nor can they be built.  They cannot be damaged or destroyed.
    /// They are merely nodes of influence in a map that rewards the owner with monetary amounts.
    /// </summary>
    public class ControlPoint : Structure
    {
        public const int TIME_TO_CAPTURE = 15;

        int income = 12;    //monatary amount
        int timeSpentCapping = 0;
        HeavyInfantryUnit capturingHero;
        long heroStartingHealth;
        bool capping = false;

        public ControlPoint() : base(){
            //TODO: set controlpoint values
            this.sightRange = 250;
            this.setType(State.EntityName.ControlPoint);
            this.map = Incursio.getInstance().currentMap;

            //TEMP - mitch
            //this.isConstructor = true;
        }

        public void startCap(HeavyInfantryUnit capturingHero)
        {
            this.capturingHero = capturingHero;
            heroStartingHealth = capturingHero.health;
            timeSpentCapping = 0;
            capping = true;
        }

        public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
        {
            if (capping)
            {
                if (timeSpentCapping >= TIME_TO_CAPTURE * 60)
                {
                    this.owner = capturingHero.getPlayer(); //change over ownership
                    timeForResource = 0;
                    timeSpentCapping = 0;
                    capturingHero = null;
                    capping = false;
                }
                else
                {
                    //if the hero gets attacked, cancel capping
                    if (capturingHero.getCurrentState() == State.UnitState.Attacking || capturingHero.health < heroStartingHealth)
                    {
                        capping = false;
                        timeSpentCapping = 0;
                    }
                    else
                    {
                        timeSpentCapping++;
                    }
                }
            }

            base.Update(gameTime, ref myRef);
        }

        public override void updateResourceTick()
        {
            //give the owner money
            if (timeForResource >= RESOURCE_TICK * 60)
            {
                timeForResource = 0;
                if (this.owner == State.PlayerId.HUMAN)
                {
                    PlayerManager.getInstance().humanPlayer.MONETARY_UNIT = PlayerManager.getInstance().humanPlayer.MONETARY_UNIT + income;
                }
                else
                {
                    PlayerManager.getInstance().computerPlayer.MONETARY_UNIT = PlayerManager.getInstance().computerPlayer.MONETARY_UNIT + income;
                }
            }
            else
            {
                timeForResource++;
            }
        }

        public override void updateBounds()
        {
            
            Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.controlPointPlayer;

            this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
                location.x - myRef.Width / 2,
                (int)(location.y - myRef.Height * 0.80),
                myRef.Width,
                myRef.Height
            );
            
        }

        public double getPercentageDone()
        {
            int timeTotal = TIME_TO_CAPTURE * 60;
            return (float)timeSpentCapping / timeTotal;
        }

        public bool isCapping()
        {
            return capping;
        }
    }
}
