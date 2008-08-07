using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Incursio.Entities.Components;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Entities.Projectiles
{
    public class BaseProjectile
    {
        //public GameTexture gameTexture;

        public Vector2 pos = new Vector2(-1, -1);
        public int LENGTH = 5;
        public int SPEED = 7;
        public bool draw = false;
        public Vector2 onScreen = new Vector2(-1, -1);
        public double angle = 0;
        public Vector2 destination = new Vector2(-1, -1);
        public int splashRange = 0;
        public double splashDamagePercent = 0;
        public double splashDegradeRate = 0;    //TODO: IMPLEMENT
        public int damage = 0;

        public RenderComponent renderComponent;
        public bool fastDecay = false;

        public bool instant = false;

        public BaseProjectile(){
            this.renderComponent = new RenderComponent(this);
        }

        public BaseProjectile(ProjectileConfiguration config){
            this.SPEED = config.speed;
            this.fastDecay = config.fastDecay;
            this.instant = config.instant;
            this.renderComponent = new RenderComponent(this);
            this.renderComponent.setAttributes(config.renderComponentConfiguration.attributes);
            this.splashRange = config.splashRange;
            this.splashDamagePercent = config.splashDamagePercent / 100;
        }

        public void Update()
        {
            if (draw)
            {
                updatePosition();
            }

            //This will update the onscreen coordinates so that the projectile will not stay in the same place when the screen moves.
            if (MapManager.getInstance().currentMap.isOnScreen(new Coordinate((int)pos.X, (int)pos.Y)))
            {
                onScreen = MapManager.getInstance().currentMap.positionOnScreen(new Coordinate((int)pos.X, (int)pos.Y)).toVector2();
            }
            else
                onScreen = new Vector2(-1, -1);
        }

        private void finishProjectile(){
            this.draw = false;
            int x = (int)this.destination.X, 
                y = (int)this.destination.Y;

            //get entity at cell
            BaseGameEntity e = EntityManager.getInstance().getEntity(
                MapManager.getInstance().currentMap.getCellEntity_pixels(x, y));
            
            //deal impact damage
            if (e != null)
                e.takeDamage(this.damage, null);

            //splash damage (if applicable)
            //TODO: DEGRADE SPLASH DAMAGE DEALT AS DISTANCE GETS LARGER
            if(this.splashRange > 0){
                List<int> ids = MapManager.getInstance().currentMap.getEntitiesInRange(new Coordinate(this.destination), this.splashRange);

                if(ids.Count > 0){
                    //deal splash damage
                    foreach(int id in ids){
                        EntityManager.getInstance().getEntity(id).takeDamage((int)(this.damage * (this.splashDamagePercent / 100)), null);
                    }
                }
            }
        }

        public void updatePosition()
        {
            double newPosX = (Math.Cos(angle * (Math.PI / 180)) * SPEED);
            double newPosY = (-1) * (Math.Sin(angle * (Math.PI / 180)) * SPEED); //INVERT TO COMPENSATE FOR PIXEL GROWTH DIRECTION

            //determine the direction, and if we are close enough to the destination, end movement
            if (pos.X > destination.X)
            {
                if (pos.X + newPosX < destination.X)
                {
                    this.finishProjectile();
                }
            }
            else if (pos.X < destination.X)
            {
                if (pos.X + newPosX > destination.X)
                {
                    this.finishProjectile();
                }
            }

            pos.X = pos.X + (float)newPosX;
            pos.Y = pos.Y + (float)newPosY;

            if (this.instant)
                this.finishProjectile();
        }

        public void startProjectile(Vector2 startLoc, Vector2 destLoc)
        {
            if (!draw)
            {
                draw = true;
                pos.X = (float)startLoc.X;
                pos.Y = (float)startLoc.Y;
                destination.X = (float)destLoc.X;
                destination.Y = (float)destLoc.Y;
                onScreen.X = -1;
                onScreen.Y = -1;

                //find angle between projectile and destination
                //straight shots first
                if (pos.Y == destination.Y && pos.X == destination.X)
                {
                    angle = 0;
                }
                else if (pos.Y == destination.Y && pos.X > destination.X)
                {
                    angle = 180;
                }
                else if (pos.X == destination.X && pos.Y > destination.Y)
                {
                    angle = 90;
                }
                else if (pos.X == destination.X && pos.Y < destination.Y)
                {
                    angle = 270;
                }
                //if none of those, do based off quadrants
                else
                {
                    //quadrant one
                    if (pos.X < destination.X && pos.Y > destination.Y)
                    {
                        angle = (180 / Math.PI) * Math.Atan((pos.Y - destination.Y) / (destination.X - pos.X));
                    }
                    //quadrant two
                    else if (pos.X > destination.X && pos.Y > destination.Y)
                    {
                        angle = 90 + (90 - (180 / Math.PI) * Math.Atan((pos.Y - destination.Y) / (pos.X - destination.X)));
                    }
                    //quadrant three
                    else if (pos.X > destination.X && pos.Y < destination.Y)
                    {
                        angle = 180 + (180 / Math.PI) * Math.Atan((destination.Y - pos.Y) / (pos.X - destination.X));
                    }
                    //quadrant four
                    else if (pos.X < destination.X && pos.Y < destination.Y)
                    {
                        angle = 270 + (90 - (180 / Math.PI) * Math.Atan((destination.Y - pos.Y) / (destination.X - pos.X)));
                    }
                }
            }


        }
    }
}
