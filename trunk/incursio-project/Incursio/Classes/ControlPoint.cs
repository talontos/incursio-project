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
        Hero capturingHero;
        long heroStartingHealth;
        bool capping = false;

        public ControlPoint() : base(){
            this.pointValue = 500;
            //TODO: set controlpoint values
            this.sightRange = 250;
            this.setType(State.EntityName.ControlPoint);
            this.map = Incursio.getInstance().currentMap;

            //TEMP - mitch
            //this.isConstructor = true;
        }

        public void startCap(Hero capturingHero)
        {
            this.capturingHero = capturingHero;
            heroStartingHealth = capturingHero.health;
            timeSpentCapping = 0;
            capping = true;
            
        }

        private void finishCapture(){
            this.owner = capturingHero.getPlayer(); //change over ownership
            timeForResource = 0;
            timeSpentCapping = 0;
            
            capturingHero.finishCapture(this);

            capturingHero = null;
            capping = false;

        }

        public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
        {
            if (capping)
            {
                if (timeSpentCapping >= TIME_TO_CAPTURE * 60)
                {
                    this.finishCapture();
                }
                else
                {
                    //if the hero gets attacked, cancel capping
                    if (capturingHero.getCurrentState() != State.UnitState.Capturing || capturingHero.health < heroStartingHealth)
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

        public override void updateOccupancy(bool occupied)
        {
            //hardcode blagh
            int xStart = location.x - 32;
            int yStart = location.y - (int)(32 * 0.80);
            int xEnd = location.x;// +32;
            int yEnd = location.y + (int)(32 * 0.20);

            if (xStart < 0 || xEnd < 0 || yStart < 0 || yEnd < 0)
                return;

            map.setSingleCellOccupancy(xStart, yStart, (byte)(occupied ? 0 : 1));
            map.setSingleCellOccupancy(xStart, yEnd, (byte)(occupied ? 0 : 1));
            map.setSingleCellOccupancy(xEnd, yStart, (byte)(occupied ? 0 : 1));
            map.setSingleCellOccupancy(xEnd, yEnd, (byte)(occupied ? 0 : 1));

            map.setSingleCellEntity(xStart, yStart, (occupied ? this.keyId : -1));
            map.setSingleCellEntity(xStart, yEnd, (occupied ? this.keyId : -1));
            map.setSingleCellEntity(xEnd, yStart, (occupied ? this.keyId : -1));
            map.setSingleCellEntity(xEnd, yEnd, (occupied ? this.keyId : -1));
        }

        public override void setLocation(Coordinate coords)
        {
            updateOccupancy(false);

            base.setLocation(coords);

            updateOccupancy(true);
        }

        public override void takeDamage(int damage, BaseGameEntity attacker)
        {
            //I CANNOT BE DESTROYED, FOR I AM THE CONTROL POINT!!!!
        }

        public override void drawThyself(ref SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
        {
            this.visible = true;
            //onScreen = currentMap.positionOnScreen(this.getLocation());
            //Rectangle unit = new Rectangle(this.getLocation().x, this.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
            Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.location);
            Rectangle unit = this.boundingBox;

            if (this.currentState == State.StructureState.Building)
            {
                //TODO: draw something special for when the structure is building something (fires flickering or w/e)
                if (this.getPlayer() == State.PlayerId.HUMAN)
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.controlPointPlayer,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointPlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointPlayer.Height * 0.80),
                        TextureBank.EntityTextures.controlPointPlayer.Width, TextureBank.EntityTextures.controlPointPlayer.Height), Color.White);
                }
                else
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.controlPointComputer,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointComputer.Height * 0.80),
                        TextureBank.EntityTextures.controlPointComputer.Width, TextureBank.EntityTextures.controlPointComputer.Height), Color.White);
                }

            }
            else if (this.currentState == State.StructureState.Idle || this.currentState == State.StructureState.Attacking)
            {
                if (this.getPlayer() == State.PlayerId.HUMAN)
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.controlPointPlayer,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointPlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointPlayer.Height * 0.80),
                        TextureBank.EntityTextures.controlPointPlayer.Width, TextureBank.EntityTextures.controlPointPlayer.Height), Color.White);
                }
                else
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.controlPointComputer,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.controlPointComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.controlPointComputer.Height * 0.80),
                        TextureBank.EntityTextures.controlPointComputer.Width, TextureBank.EntityTextures.controlPointComputer.Height), Color.White);
                }
            }
        }
    }
}
