using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Classes;
using Incursio.Utils;
using Incursio.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Entities.Components
{
    public class RenderComponent : BaseComponent
    {
        public bool visible = false;
        public bool highlighted = false;
        public bool justDrawn = false;
        public bool playedDeath = false;

        public int currentFrameX = 0;       //for animation
        public int currentFrameY = 0;       //for animation
        public int currentFrameXAttackDeath = 0;
        public int currentFrameYAttackDeath = 0;

        public int attackFramePause = 0;
        public Rectangle boundingBox;

        public int destroyedTimer = 0;
        public const int TIME_TILL_DESTROYED_FADE = 1;

        public State.EntityState currentState;
        public State.Direction directionState;
        public global::Incursio.Entities.TextureCollections.TextureCollection textures;

        public RenderComponent(BaseGameEntity entity) : base(entity){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            updateBounds();
        }

        public void updateBounds(){
            Texture2D myRef = this.textures.still.South;

            this.boundingBox = new Rectangle(
                this.bgEntity.location.x - myRef.Width / 2,
                (int)(this.bgEntity.location.y - myRef.Height * 0.80),
                myRef.Width,
                myRef.Height
            );
        }

        public void drawThyself(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
        {
            this.visible = true;
            this.justDrawn = false;
            Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.bgEntity.location);
            Rectangle unit = this.boundingBox;
            Color colorMask = EntityManager.getInstance().getColorMask(this.bgEntity.owner);

            //TODO: PROJECTILES
            #region PROJECTILES
            /*//draw the arrow if needed
            if (drawArrow)
            {
                spriteBatch.Draw(TextureBank.EntityTextures.arrow,
                    new Vector2(arrowOnScreen.x, arrowOnScreen.y),
                    null, Color.White, -1 * ((float)(arrowAngle * (Math.PI / 180))), new Vector2(TextureBank.EntityTextures.arrow.Width / 2, TextureBank.EntityTextures.arrow.Height / 2), 1.0f, SpriteEffects.None, 0f);
            }
            */
            #endregion

            //depending on the unit's state, draw its textures
            //idle
            #region IDLE
            if (this.currentState == State.EntityState.Idle)
            {
                switch (this.directionState)
                {
                    case State.Direction.South:
                    case State.Direction.Still:
                        spriteBatch.Draw(this.textures.still.South,
                            new Rectangle(onScreen.x - (this.textures.still.South.Width / 2), onScreen.y - (int)(this.textures.still.South.Height * 0.80),
                            this.textures.still.South.Width, this.textures.still.South.Height), colorMask);
                        break;

                    case State.Direction.East:
                        spriteBatch.Draw(this.textures.still.East,
                            new Rectangle(onScreen.x - (this.textures.still.East.Width / 2), onScreen.y - (int)(this.textures.still.East.Height * 0.80),
                            this.textures.still.East.Width, this.textures.still.East.Height), colorMask);
                        break;

                    case State.Direction.West:
                        spriteBatch.Draw(this.textures.still.West,
                            new Rectangle(onScreen.x - (this.textures.still.West.Width / 2), onScreen.y - (int)(this.textures.still.West.Height * 0.80),
                            this.textures.still.West.Width, this.textures.still.West.Height), colorMask);
                        break;

                    case State.Direction.North:
                        spriteBatch.Draw(this.textures.still.North,
                            new Rectangle(onScreen.x - (this.textures.still.North.Width / 2), onScreen.y - (int)(this.textures.still.North.Height * 0.80),
                            this.textures.still.North.Width, this.textures.still.North.Height), colorMask);
                        break;
                }

            }
            #endregion
            #region BEING_BUILT
            else if (this.currentState == State.EntityState.BeingBuilt)
            {
                //TODO: draw construction?
            }
            #endregion
            #region BUILDING
            else if (this.currentState == State.EntityState.Building)
            {
                //draw something special for when the structure is building something (fires flickering or w/e)
                spriteBatch.Draw(this.textures.still.Building,
                    new Rectangle(onScreen.x - (this.textures.still.Building.Width / 2), onScreen.y - (int)(this.textures.still.Building.Height * 0.80),
                    this.textures.still.Building.Width, this.textures.still.Building.Height),
                    new Rectangle(this.currentFrameX, this.currentFrameY, 64, 64), Color.White);

                if (frameTimer >= FRAME_LENGTH)
                {
                    if (this.currentFrameX < this.textures.still.Building.Width - 64)
                    {
                        this.currentFrameX = this.currentFrameX + 64;
                    }
                    else
                    {
                        this.currentFrameX = 0;
                    }
                }
            }
            #endregion
            #region DESTROYED
            else if (this.currentState == State.EntityState.Destroyed)
            {
                if (destroyedTimer < TIME_TILL_DESTROYED_FADE * 60)
                {
                    if (this.textures.damaged.alphaChan >= 0)
                    {
                        spriteBatch.Draw(this.textures.damaged.exploded,
                            new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerExploded.Width / 2), onScreen.y - (int)(this.textures.damaged.exploded.Height * 0.80),
                            this.textures.damaged.exploded.Width, this.textures.damaged.exploded.Height), new Color(255, 255, 255, this.textures.damaged.alphaChan));
                        this.textures.damaged.alphaChan -= 25;
                    }

                    destroyedTimer++;
                }

            }
            #endregion
            #region ATTACKING
            else if (this.currentState == State.EntityState.Attacking)
            {

                switch (this.directionState)
                {
                    case State.Direction.West:
                    case State.Direction.North:
                        spriteBatch.Draw(this.textures.attacking.West,
                            new Rectangle(onScreen.x - (25 / 2), onScreen.y - (int)(this.textures.attacking.West.Height * 0.80),
                            this.textures.attacking.West.Width, this.textures.attacking.West.Height),
                            new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameXAttackDeath < this.textures.attacking.West.Width - 25)
                            {
                                this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 25;
                            }
                            else
                            {
                                this.currentFrameXAttackDeath = 0;
                            }
                        }
                        break;

                    case State.Direction.East:
                    case State.Direction.South:
                        spriteBatch.Draw(this.textures.attacking.East,
                            new Rectangle(onScreen.x - (25 / 2), onScreen.y - (int)(this.textures.attacking.East.Height * 0.80),
                            this.textures.attacking.East.Width, this.textures.attacking.East.Height),
                            new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameXAttackDeath < this.textures.attacking.East.Width - 25)
                            {
                                this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 25;
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
            else if (this.currentState == State.EntityState.Dead)
            {
                switch (this.directionState)
                {
                    case State.Direction.West:
                    case State.Direction.North:
                        if (!this.playedDeath)
                        {
                            spriteBatch.Draw(this.textures.death.East,
                            new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                            new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 33, 30), colorMask);

                            if (frameTimer >= FRAME_LENGTH)
                            {
                                if (this.currentFrameXAttackDeath < this.textures.death.East.Width - 33)
                                {
                                    this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 33;
                                }
                                else
                                {
                                    this.playedDeath = true;
                                }
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(this.textures.death.East,
                            new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                            new Rectangle(66, 0, 33, 30), colorMask);
                        }
                        break;

                    case State.Direction.East:
                    case State.Direction.South:
                        if (!this.playedDeath)
                        {
                            spriteBatch.Draw(this.textures.death.West,
                            new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                            new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 33, 30), colorMask);

                            if (frameTimer >= FRAME_LENGTH)
                            {
                                if (this.currentFrameXAttackDeath < this.textures.death.West.Width - 33)
                                {
                                    this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 33;
                                }
                                else
                                {
                                    this.playedDeath = true;
                                }
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(this.textures.death.West,
                            new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                            new Rectangle(66, 0, 33, 30), colorMask);
                        }
                        break;

                }

            }
            #endregion
            #region GUARDING
            else if (this.currentState == State.EntityState.Guarding)
            {
                //TODO:
                //Guarding Animation
            }
            #endregion
            #region MOVING
            else if (this.currentState == State.EntityState.Moving)
            {
                switch (this.directionState)
                {
                    case State.Direction.West:
                        spriteBatch.Draw(this.textures.movement.West,
                            new Rectangle(onScreen.x - (this.textures.still.West.Width / 2), onScreen.y - (int)(this.textures.still.West.Height * 0.80),
                            this.textures.still.West.Width, this.textures.still.West.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.West.Width - 20)
                            {
                                this.currentFrameX = this.currentFrameX + 20;
                            }
                            else
                            {
                                this.currentFrameX = 0;
                            }
                        }
                        break;

                    case State.Direction.East:
                        spriteBatch.Draw(this.textures.movement.East,
                            new Rectangle(onScreen.x - (this.textures.still.West.Width / 2), onScreen.y - (int)(this.textures.still.West.Height * 0.80),
                            this.textures.still.West.Width, this.textures.still.West.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.East.Width - 20)
                            {
                                this.currentFrameX = this.currentFrameX + 20;
                            }
                            else
                            {
                                this.currentFrameX = 0;
                            }
                        }
                        break;

                    case State.Direction.South:
                        spriteBatch.Draw(this.textures.movement.South,
                            new Rectangle(onScreen.x - (this.textures.still.South.Width / 2), onScreen.y - (int)(this.textures.still.South.Height * 0.80),
                            this.textures.still.South.Width, this.textures.still.South.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.South.Width - 20)
                            {
                                this.currentFrameX = this.currentFrameX + 20;
                            }
                            else
                            {
                                this.currentFrameX = 0;
                            }
                        }
                        break;

                    case State.Direction.North:
                        spriteBatch.Draw(this.textures.movement.North,
                            new Rectangle(onScreen.x - (this.textures.still.North.Width / 2), onScreen.y - (int)(this.textures.still.North.Height * 0.80),
                            this.textures.still.North.Width, this.textures.still.North.Height),
                            new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                        if (frameTimer >= FRAME_LENGTH)
                        {
                            if (this.currentFrameX < this.textures.movement.North.Width - 20)
                            {
                                this.currentFrameX = this.currentFrameX + 20;
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
            else if (this.currentState == State.EntityState.UnderAttack)
            {
                //TODO:
                //Under Attack Animation
            }
            #endregion
            #region ELSE
            else
            {
                spriteBatch.Draw(this.textures.still.South,
                        new Rectangle(onScreen.x - (this.textures.still.South.Width / 2), onScreen.y - (int)(this.textures.still.South.Height * 0.80),
                        this.textures.still.South.Width, this.textures.still.South.Height), colorMask);
            }
            #endregion
        }
    }
}
