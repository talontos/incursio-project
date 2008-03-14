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
        protected Texture2D mapImage;
        protected int width;
        protected int height;

        protected bool[,] occupancyGrid;

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
            this.occupancyGrid = new bool[width, height];
            this.minViewableX = 0;
            this.minViewableY = 0;
            this.maxViewableX = screenWidth / TILE_WIDTH;
            this.maxViewableY = screenHeight / TILE_HEIGHT;
            this.cameraMovePause = 0;
        }

        public MapBase(int width, int height, int screenWidth, int screenHeight, Texture2D image)
        {
            this.width = width / TILE_WIDTH;
            this.height = height / TILE_HEIGHT;
            this.occupancyGrid = new bool[width, height];
            this.minViewableX = 0;
            this.minViewableY = 0;
            this.maxViewableX = screenWidth / TILE_WIDTH;
            this.maxViewableY = screenHeight / TILE_HEIGHT;
            this.cameraMovePause = 0;
            this.mapImage = image;

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
            int xStart = minViewableX * TILE_WIDTH;
            int xEnd = maxViewableX * TILE_WIDTH;
            int yStart = minViewableY * TILE_HEIGHT;
            int yEnd = maxViewableY * TILE_HEIGHT;

            if (xStart > (width - minViewableX) * TILE_WIDTH)
            {
                xStart = (width * TILE_WIDTH - 1024) * TILE_WIDTH;
                xEnd = width * TILE_WIDTH;
            }

            if (yStart > (height - minViewableY) * TILE_HEIGHT)
            {
                yStart = (height * TILE_HEIGHT - 768) * TILE_HEIGHT;
                yEnd = height * TILE_HEIGHT;
            }

            spriteBatch.Draw(mapImage, new Rectangle(0, 0, 1024, 768),
                new Rectangle(xStart, yStart,
                xEnd, yEnd), Color.White);   
        }

        /*public void addMapEntity(BaseMapEntity entity, int xPos, int yPos)
        {
            if (xPos >= 0 && xPos < this.width && yPos >= 0 && yPos < this.height)
            {
                this.tileGrid[xPos, yPos] = entity;
                this.occupancyGrid[xPos, yPos] = entity.passable;
            }
        }*/

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

        /// <summary>
        /// Compares two cells, using 2 pixel-coordinates.
        /// If cells are the same, returns true (can move, duh).
        /// If cells are different, returns true if cell containing
        ///     (x2, y2) is passable, false otherwise
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public bool requestMove(int x1, int y1, int x2, int y2){
            int a, b, c, d;
            
            this.translatePixelToMapCell(x1, y1, out a, out b);
            this.translatePixelToMapCell(x2, y2, out c, out d);

            if(a == c && b == d){
                //same cell, is ok
                return true;
            }
            else{
                return this.occupancyGrid[c, d];
            }
        }

        public bool getCellOccupancy(int pixX, int pixY){
            int x, y;
            this.translatePixelToMapCell(pixX, pixY, out x, out y);
            return this.occupancyGrid[x, y];
        }
        
        public void setSingleCellOccupancy(int pixX, int pixY, bool occupied){
            int x, y;
            this.translatePixelToMapCell(pixX, pixY, out x, out y);
            this.occupancyGrid[x, y] = occupied;
        }

        public int getCellDistance(Coordinate c1, Coordinate c2){
            int a, b, c, d;

            this.translatePixelToMapCell(c1.x, c1.y, out a, out b);
            this.translatePixelToMapCell(c2.x, c2.y, out c, out d);

            Vector2 v1 = new Vector2(a, b);
            Vector2 v2 = new Vector2(c, d);

            return Convert.ToInt32(Math.Abs(Vector2.Distance(v1, v2)));

        }

        private void translatePixelToMapCell(int pixX, int pixY, out int indexX, out int indexY){
            indexX = pixX / TILE_WIDTH;
            indexY = pixY / TILE_HEIGHT;
        }

        public int getTileHeight()
        {
            return TILE_HEIGHT;
        }

        public int getTileWidth()
        {
            return TILE_WIDTH;
        }

        public int getMinimumX()
        {
            return minViewableX;
        }

        public int getMinimumY()
        {
            return minViewableY;
        }

        public void setMapImage(Texture2D image)
        {
            mapImage = image;
        }

        public void printOccupancyGrid(){
            for(int x = 0; x < this.occupancyGrid.GetLength(0); x++){
                for(int y = 0; y < this.occupancyGrid.GetLength(1); y++){
                    Console.Write(this.occupancyGrid[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
