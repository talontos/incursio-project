using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Classes;
using Incursio.Utils;
using Incursio.Managers;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Entities.Projectiles;

namespace Incursio.Entities.Components
{
    public class RenderComponent : BaseComponent
    {
        public bool visible = false;
        public bool highlighted = false;
        public bool justDrawn = false;
        public bool playedDeath = false;
        public bool isProjectile = false;

        public BaseProjectile projectile;

        public int currentFrameX = 0;       //for animation
        public int currentFrameY = 0;       //for animation
        public int currentFrameXAttackDeath = 0;
        public int currentFrameYAttackDeath = 0;

        public int attackFramePause = 0;
        public Rectangle boundingBox;

        public int destroyedTimer = 0;
        public const int TIME_TILL_DESTROYED_FADE = 1;

        //public State.EntityState currentState;
        public State.Direction directionState = State.Direction.Still;
        public global::Incursio.Entities.TextureCollections.TextureCollection textures;

        public RenderComponent(BaseGameEntity entity) : base(entity){

        }

        public RenderComponent(BaseProjectile projectile){
            this.projectile = projectile;
        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i].Key)
                {
                    case "collectionName": this.textures = TextureBank.getInstance().getCollectionByName(attributes[i].Value); break;
                    default: break;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            updateBounds();
        }

        public void updateBounds(){
            Texture2D myRef = this.textures.still.South.texture;

            this.boundingBox = new Rectangle(
                this.bgEntity.location.x - myRef.Width / 2,
                (int)(this.bgEntity.location.y - myRef.Height * 0.80),
                myRef.Width,
                myRef.Height
            );
        }

        public void updateOccupancy(){
            //TODO: CALCULATE OCCUPANCY SIZE FROM TEXTURE

            //TODO: UPDATE OCCUPANCY
        }

        public void drawThyself(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
        {
            if (this.bgEntity.movementComponent != null)
                this.directionState = bgEntity.movementComponent.directionState;

            this.visible = true;
            this.justDrawn = false;
            Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.bgEntity.location);
            Rectangle unit = this.boundingBox;
            Color colorMask = EntityManager.getInstance().getColorMask(this.bgEntity.owner);

            //draw the projectile if needed
            #region PROJECTILES
            if (isProjectile && this.projectile.draw)
            {
                spriteBatch.Draw(TextureBank.EntityTextures.arrow,
                    this.projectile.onScreen,
                    null, Color.White, -1 * ((float)(this.projectile.angle * (Math.PI / 180))),
                    new Vector2(this.projectile.texture.Width / 2, 
                        this.projectile.texture.Height / 2), 
                    1.0f, SpriteEffects.None, 0f);
            }
            
            #endregion

            //depending on the unit's state, draw its textures
            //idle
            #region IDLE
            if (this.bgEntity.currentState == State.EntityState.Idle)
            {
                switch (this.directionState)
                {
                    case State.Direction.South:
                    case State.Direction.Still:
                        spriteBatch.Draw(this.textures.still.South.texture,
                            new Rectangle(onScreen.x - (this.textures.still.South.texture.Width / 2), onScreen.y - (int)(this.textures.still.South.texture.Height * 0.80),
                            this.textures.still.South.texture.Width, this.textures.still.South.texture.Height), colorMask);
                        break;

                    case State.Direction.East:
                        spriteBatch.Draw(this.textures.still.East.texture,
                            new Rectangle(onScreen.x - (this.textures.still.East.texture.Width / 2), onScreen.y - (int)(this.textures.still.East.texture.Height * 0.80),
                            this.textures.still.East.texture.Width, this.textures.still.East.texture.Height), colorMask);
                        break;

                    case State.Direction.West:
                        spriteBatch.Draw(this.textures.still.West.texture,
                            new Rectangle(onScreen.x - (this.textures.still.West.texture.Width / 2), onScreen.y - (int)(this.textures.still.West.texture.Height * 0.80),
                            this.textures.still.West.texture.Width, this.textures.still.West.texture.Height), colorMask);
                        break;

                    case State.Direction.North:
                        spriteBatch.Draw(this.textures.still.North.texture,
                            new Rectangle(onScreen.x - (this.textures.still.North.texture.Width / 2), onScreen.y - (int)(this.textures.still.North.texture.Height * 0.80),
                            this.textures.still.North.texture.Width, this.textures.still.North.texture.Height), colorMask);
                        break;
                }

            }
            #endregion
            #region BEING_BUILT
            else if (this.bgEntity.currentState == State.EntityState.BeingBuilt)
            {
                //TODO: draw construction?
            }
            #endregion
            #region BUILDING
            else if (this.bgEntity.currentState == State.EntityState.Building)
            {
                //draw something special for when the structure is building something (fires flickering or w/e)
                spriteBatch.Draw(this.textures.still.Building.texture,
                    new Rectangle(onScreen.x - (this.textures.still.Building.texture.Width / 2), onScreen.y - (int)(this.textures.still.Building.texture.Height * 0.80),
                    this.textures.still.Building.texture.Width, this.textures.still.Building.texture.Height),
                    new Rectangle(this.currentFrameX, this.currentFrameY, this.textures.still.Building.frameWidth, this.textures.still.Building.frameHeight), Color.White);

                if (frameTimer >= FRAME_LENGTH)
                {
                    if (this.currentFrameX < this.textures.still.Building.texture.Width - this.textures.still.Building.frameWidth)
                    {
                        this.currentFrameX = this.currentFrameX + this.textures.still.Building.frameWidth;
                    }
                    else
                    {
                        this.currentFrameX = 0;
                    }
                }
            }
            #endregion
            #region DESTROYED
            else if (this.bgEntity.currentState == State.EntityState.Destroyed)
            {
                if (destroyedTimer < TIME_TILL_DESTROYED_FADE * 60)
                {
                    if (this.textures.damaged.alphaChan >= 0)
                    {
                        spriteBatch.Draw(this.textures.damaged.exploded.texture,
                            new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerExploded.Width / 2), onScreen.y - (int)(this.textures.damaged.exploded.texture.Height * 0.80),
                            this.textures.damaged.exploded.texture.Width, this.textures.damaged.exploded.texture.Height), new Color(255, 255, 255, this.textures.damaged.alphaChan));
                        this.textures.damaged.alphaChan -= 25;
                    }

                    destroyedTimer++;
                }

            }
            #endregion
            #region ATTACKING
            else if (this.bgEntity.currentState == State.EntityState.Attacking)
            {

                switch (this.directionState)
                {
                    case State.Direction.West:
                    case State.Direction.North:
                        spriteBatch.Draw(this.textures.attacking.West.texture,
                            new Rectangle(onScreen.x - (this.textures.attacking.West.frameWidth / 2), onScreen.y - (int)(this.textures.attacking.West.texture.Height * 0.80),
                            this.textures.attacking.West.texture.Width, this.textures.attacking.West.texture.Height),
                            new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, this.textures.attacking.West.frameWidth, this.textures.attacking.West.frameHeight), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameXAttackDeath < this.textures.attacking.West.texture.Width - this.textures.attacking.West.frameWidth)
                            {
                                this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + this.textures.attacking.West.frameWidth;
                            }
                            else
                            {
                                this.currentFrameXAttackDeath = 0;
                            }
                        }
                        break;

                    case State.Direction.East:
                    case State.Direction.South:
                        spriteBatch.Draw(this.textures.attacking.East.texture,
                            new Rectangle(onScreen.x - (this.textures.attacking.East.frameWidth / 2), onScreen.y - (int)(this.textures.attacking.East.texture.Height * 0.80),
                            this.textures.attacking.East.texture.Width, this.textures.attacking.East.texture.Height),
                            new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, this.textures.attacking.East.frameWidth, this.textures.attacking.East.frameHeight), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameXAttackDeath < this.textures.attacking.East.texture.Width - this.textures.attacking.East.frameWidth)
                            {
                                this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + this.textures.attacking.East.frameWidth;
                            }
                            else
                            {
                                this.currentFrameXAttackDeath = 0;
                            }
                        }
                        break;
                }
            }
            #endregion
            #region DEAD
            else if (this.bgEntity.currentState == State.EntityState.Dead)
            {
                switch (this.directionState)
                {
                    case State.Direction.West:
                    case State.Direction.North:
                        if (!this.playedDeath)
                        {
                            spriteBatch.Draw(this.textures.death.East.texture,
                            new Rectangle(onScreen.x - (int)(this.textures.death.East.frameWidth / 2), 
                                onScreen.y - (int)(this.textures.death.East.frameHeight * 0.80), 
                                this.textures.death.East.frameWidth, this.textures.death.East.frameHeight),
                            new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 
                                this.textures.death.East.frameWidth, this.textures.death.East.frameHeight), colorMask);

                            if (frameTimer >= FRAME_LENGTH)
                            {
                                if (this.currentFrameXAttackDeath < this.textures.death.East.texture.Width - this.textures.death.East.frameWidth)
                                {
                                    this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + this.textures.death.East.frameWidth;
                                }
                                else
                                {
                                    this.playedDeath = true;
                                }
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(this.textures.death.East.texture,
                                new Rectangle(onScreen.x - (int)(this.textures.death.East.frameWidth / 2), 
                                    onScreen.y - (int)(this.textures.death.East.frameHeight * 0.80), this.textures.death.East.frameWidth, 
                                    this.textures.death.East.frameHeight),
                                new Rectangle(this.textures.death.East.frameWidth * 2, 0, this.textures.death.East.frameWidth, this.textures.death.East.frameHeight), colorMask);
                        }
                        break;

                    case State.Direction.East:
                    case State.Direction.South:
                        if (!this.playedDeath)
                        {
                            spriteBatch.Draw(this.textures.death.West.texture,
                                new Rectangle(onScreen.x - (int)(this.textures.death.West.frameWidth / 2), 
                                    onScreen.y - (int)(this.textures.death.West.frameHeight * 0.80), this.textures.death.West.frameWidth, 
                                    this.textures.death.West.frameHeight),
                                new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 
                                    this.textures.death.West.frameWidth, this.textures.death.West.frameHeight), colorMask);

                            if (frameTimer >= FRAME_LENGTH)
                            {
                                if (this.currentFrameXAttackDeath < this.textures.death.West.texture.Width - this.textures.death.West.frameWidth)
                                {
                                    this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + this.textures.death.West.frameWidth;
                                }
                                else
                                {
                                    this.playedDeath = true;
                                }
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(this.textures.death.West.texture,
                                new Rectangle(onScreen.x - (int)(this.textures.death.West.frameWidth / 2), 
                                    onScreen.y - (int)(this.textures.death.West.frameHeight * 0.80), this.textures.death.West.frameWidth, 
                                    this.textures.death.West.frameHeight),
                                new Rectangle(this.textures.death.West.frameWidth * 2, 0, this.textures.death.West.frameWidth, 
                                    this.textures.death.West.frameHeight), colorMask);
                        }
                        break;

                }

            }
            #endregion
            #region GUARDING
            else if (this.bgEntity.currentState == State.EntityState.Guarding)
            {
                //TODO:
                //Guarding Animation
            }
            #endregion
            #region MOVING
            else if (this.bgEntity.currentState == State.EntityState.Moving)
            {
                switch (this.directionState)
                {
                    case State.Direction.West:
                        spriteBatch.Draw(this.textures.movement.West.texture,
                            new Rectangle(onScreen.x - (this.textures.still.West.texture.Width / 2), 
                                onScreen.y - (int)(this.textures.still.West.texture.Height * 0.80),
                                this.textures.still.West.texture.Width, this.textures.still.West.texture.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, this.textures.movement.West.frameWidth, 
                                this.textures.movement.West.frameHeight), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.West.texture.Width - this.textures.movement.West.frameWidth)
                            {
                                this.currentFrameX = this.currentFrameX + this.textures.movement.West.frameWidth;
                            }
                            else
                            {
                                this.currentFrameX = 0;
                            }
                        }
                        break;

                    case State.Direction.East:
                        spriteBatch.Draw(this.textures.movement.East.texture,
                            new Rectangle(onScreen.x - (this.textures.still.West.texture.Width / 2), 
                                onScreen.y - (int)(this.textures.still.West.texture.Height * 0.80),
                                this.textures.still.West.texture.Width, this.textures.still.West.texture.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, this.textures.movement.East.frameWidth, 
                                this.textures.movement.East.frameHeight), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.East.texture.Width - this.textures.movement.East.frameWidth)
                            {
                                this.currentFrameX = this.currentFrameX + this.textures.movement.East.frameWidth;
                            }
                            else
                            {
                                this.currentFrameX = 0;
                            }
                        }
                        break;

                    case State.Direction.South:
                        spriteBatch.Draw(this.textures.movement.South.texture,
                            new Rectangle(onScreen.x - (this.textures.still.South.texture.Width / 2), 
                                onScreen.y - (int)(this.textures.still.South.texture.Height * 0.80),
                                this.textures.still.South.texture.Width, this.textures.still.South.texture.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, this.textures.movement.South.frameWidth, this.textures.movement.South.frameHeight), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.South.texture.Width - this.textures.movement.South.frameWidth)
                            {
                                this.currentFrameX = this.currentFrameX + this.textures.movement.South.frameWidth;
                            }
                            else
                            {
                                this.currentFrameX = 0;
                            }
                        }
                        break;

                    case State.Direction.North:
                        spriteBatch.Draw(this.textures.movement.North.texture,
                            new Rectangle(onScreen.x - (this.textures.still.North.texture.Width / 2), 
                                onScreen.y - (int)(this.textures.still.North.texture.Height * 0.80),
                                this.textures.still.North.texture.Width, this.textures.still.North.texture.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, 
                                this.textures.movement.North.frameWidth, this.textures.movement.North.frameHeight), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.North.texture.Width - this.textures.movement.North.frameWidth)
                            {
                                this.currentFrameX = this.currentFrameX + this.textures.movement.North.frameWidth;
                            }
                            else
                            {
                                this.currentFrameX = 0;
                            }
                        }
                        break;

                }


            }
            #endregion
            #region UNDER_ATTACK
            else if (this.bgEntity.currentState == State.EntityState.UnderAttack)
            {
                //TODO:
                //Under Attack Animation
            }
            #endregion
            #region ELSE
            else
            {
                spriteBatch.Draw(this.textures.still.South.texture,
                        new Rectangle(onScreen.x - (this.textures.still.South.texture.Width / 2), onScreen.y - (int)(this.textures.still.South.texture.Height * 0.80),
                        this.textures.still.South.texture.Width, this.textures.still.South.texture.Height), colorMask);
            }
            #endregion
        }

        public void drawSelectionOverlay(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.bgEntity.getLocation());
            double healthRatio = (float)this.bgEntity.getHealth() / this.bgEntity.getMaxHealth();

            //determine health-bar color
            Color healthColor = healthRatio > 66 ? Color.Lime : healthRatio > 33 ? Color.Yellow : Color.Red;

            double healthBarTypicalWidth = 0.59375;             //these horrible numbers are ratios for the healthbar of the
            double healthBarTypicalHeight = 0.03125;            //selecetedUnitOverlayTexture.  These account for changes in
            double healthBarTypicalStartWidth = 0.25;           //overlay size, so that the healthbar will still display where
            double healthBarTypicalStartHeight = 0.0625;        //it should.

            
            //TODO: THESE NUMBERS WILL HAVE TO BE MODIFIED TO CALCULATE DYNAMICALLY
            int xOffSet = (int)(this.textures.still.South.texture.Width / 2) + 10;
            int yOffSet = (int)(this.textures.still.South.texture.Height * 0.80) + 7;
            int width = this.textures.still.South.texture.Width + 20;
            int height = this.textures.still.South.texture.Height + 15;

            //draw health bar overlay
            spriteBatch.Draw(TextureBank.EntityTextures.selectedUnitOverlayTexture,
                new Rectangle(onScreen.x - xOffSet, onScreen.y - yOffSet, width, height),
                Color.White);

            //draw health bar
            spriteBatch.Draw(TextureBank.EntityTextures.healthRatioTexture,
                new Rectangle(onScreen.x - xOffSet + 1 + (int)(width * healthBarTypicalStartWidth), onScreen.y - yOffSet + 1 + (int)(height * healthBarTypicalStartHeight), (int)((width * healthBarTypicalWidth) * healthRatio), (int)(height * healthBarTypicalHeight)),
                healthColor);
        }
    }
}
