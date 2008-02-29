using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Incursio.Utils;

namespace Incursio.Classes
{
    public class MapBase
    {
        //member variables
        protected int width;
        protected int height;
        protected BaseMapEntity[,] tileGrid;

        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;

        protected int minViewableX;
        protected int maxViewableX;
        protected int minViewableY;
        protected int maxViewableY;

        protected int cameraMovePause;


        public MapBase()
        {
            this.width = 0;
            this.height = 0;
            this.minViewableX = 0;
            this.minViewableY = 0;
            this.maxViewableX = 0;
            this.maxViewableY = 0;
            this.cameraMovePause = 0;
        }

        public MapBase(int width, int height, int screenWidth, int screenHeight)
        {
            this.width = width / TILE_WIDTH;
            this.height = height / TILE_HEIGHT;
            this.tileGrid = new BaseMapEntity[width, height];
            this.minViewableX = 0;
            this.minViewableY = 0;
            this.maxViewableX = screenWidth / TILE_WIDTH;
            this.maxViewableY = screenHeight / TILE_HEIGHT;
            this.cameraMovePause = 0;

        }

        public void update(Keys []keysPressed, int screenWidth, int screenHeight)
        {

            for (int i = 0; i < keysPressed.Length; i++)    //scan through the keys being pressed down
            {
                if (keysPressed[i] == Keys.Right && maxViewableX < this.width)
                {
                    this.maxViewableX++;
                    this.minViewableX++;
                }
                else if (keysPressed[i] == Keys.Left && minViewableX > 0)
                {
                    this.maxViewableX--;
                    this.minViewableX--;
                }

                if (keysPressed[i] == Keys.Up && minViewableY > 0)
                {
                    this.maxViewableY--;
                    this.minViewableY--;
                }
                else if (keysPressed[i] == Keys.Down && maxViewableY < this.height)
                {
                    this.maxViewableY++;
                    this.minViewableY++;
                }
            }

        }

        public void draw(SpriteBatch spriteBatch, Cursor cursor)
        {
            //ouch my performance :(
            //is there a better way to do this?
            int screenX = 0;
            int screenY = 0;

            for (int j = minViewableY; j < maxViewableY; j++)
            {
                screenX = 0;
                for (int i = minViewableX; i < maxViewableX; i++)
                {
                    spriteBatch.Draw(tileGrid[i , j].texture, new Rectangle(screenX * TILE_WIDTH, screenY * TILE_HEIGHT, TILE_WIDTH, TILE_HEIGHT), Color.White);
                    screenX++;
                }
                screenY++;
            }
        }

        public void addMapEntity(BaseMapEntity entity, int xPos, int yPos)
        {
            if (xPos >= 0 && xPos < this.width && yPos >= 0 && yPos < this.height)
            {
                this.tileGrid[xPos, yPos] = entity;
            }
        }

        /// <summary>
        /// helper method that determines if an entity at the parameter's coords is on the screen
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool isOnScreen(Coordinate coords)
        {
            int xSpot = coords.x / TILE_WIDTH;
            int ySpot = coords.y / TILE_HEIGHT;

            if (xSpot >= this.minViewableX && xSpot <= this.maxViewableX && ySpot >= this.minViewableY && ySpot <= this.maxViewableY)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// helper method that determines an entity's coordinates on the screen.  Helpful for maps larger than the screen size
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public Coordinate positionOnScreen(Coordinate coords)
        {
            if (isOnScreen(coords))
            {
                return new Coordinate(coords.x - (this.minViewableX * TILE_WIDTH), coords.y - (this.minViewableY * TILE_HEIGHT));
            }
            else
            {
                return null;
            }
        }

        public int getTileHeight()
        {
            return TILE_HEIGHT;
        }

        public int getTileWidth()
        {
            return TILE_WIDTH;
        }
    }
}
