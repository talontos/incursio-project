using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Incursio.Interface;

using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Classes
{
    public class MapBase
    {
        public static bool DRAW_OCCUPANCY_GRID = true;
        //member variables

        //width/height, in cells
        public int width;
        public int height;

        protected BaseMapEntity[,] tileGrid;

        public byte[,] occupancyGrid;

        public int TILE_WIDTH  = MapManager.TILE_WIDTH;
        public int TILE_HEIGHT = MapManager.TILE_HEIGHT;

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

            this.occupancyGrid = new byte[0, 0];
        }

        public MapBase(int width, int height, int screenWidth, int screenHeight)
        {
            this.width = width / TILE_WIDTH;
            this.height = height / TILE_HEIGHT;
            this.tileGrid = new BaseMapEntity[this.width, this.height];
            this.occupancyGrid = new byte[this.width, this.height];
            this.minViewableX = 0;
            this.minViewableY = 0;
            this.maxViewableX = screenWidth / TILE_WIDTH;
            this.maxViewableY = screenHeight / TILE_HEIGHT;
            this.cameraMovePause = 0;

            //initialize occupancyGrid
            for (int x = 0; x < this.height; x++)
            {
                for (int y = 0; y < this.width; y++)
                {
                    occupancyGrid[x, y] = 1;
                }
            }
        }

        public virtual void setMapDimensions(int width, int height, int screenWidth, int screenHeight)
        {
            this.width = width / TILE_WIDTH;
            this.height = height / TILE_HEIGHT;
            this.tileGrid = new BaseMapEntity[this.width, this.height];
            this.occupancyGrid = new byte[this.width, this.height];
            this.minViewableX = 0;
            this.minViewableY = 0;
            this.maxViewableX = screenWidth / TILE_WIDTH;
            this.maxViewableY = screenHeight / TILE_HEIGHT;
            this.cameraMovePause = 0;

            //initialize occupancyGrid
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    occupancyGrid[i, j] = 1;
                }
            }
        }

        public virtual State.GameState inspectWinConditions(){
            return State.GameState.None;
        }

        public virtual void initializeMap(){
            this.loadTerrain();
        }

        public void update(Keys[] keysPressed, int screenWidth, int screenHeight)
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

            Color mask = Color.White;

            for (int j = minViewableY; j < maxViewableY; j++)
            {
                screenX = 0;
                for (int i = minViewableX; i < maxViewableX; i++)
                {
                    if(DRAW_OCCUPANCY_GRID){
                        if (this.occupancyGrid[i, j] == (byte)0)
                            mask = Color.CornflowerBlue;
                        else
                            mask = Color.White;
                    }

                    spriteBatch.Draw(tileGrid[i, j].texture, new Rectangle(screenX * TILE_WIDTH, screenY * TILE_HEIGHT, TILE_WIDTH, TILE_HEIGHT), mask);
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
                this.occupancyGrid[xPos, yPos] = entity.passable ? (byte)1 : (byte)0;
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
                return this.occupancyGrid[c, d] == 1;
            }
        }

        public byte getCellOccupancy_cells(int x, int y){
            return this.occupancyGrid[x, y];
        }

        public byte getCellOccupancy_pixels(int pixX, int pixY){
            int x, y;
            this.translatePixelToMapCell(pixX, pixY, out x, out y);
            return this.occupancyGrid[x, y];
        }
        
        public void setSingleCellOccupancy(int pixX, int pixY, byte occupied){
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

        private void translateMapCellToPixel(int indexX, int indexY, out int pixX, out int pixY){
            pixX = indexX * TILE_WIDTH;
            pixY = indexY * TILE_HEIGHT;
        }

        private void translatePixelToMapCell(int pixX, int pixY, out int indexX, out int indexY){
            indexX = pixX / TILE_WIDTH;
            indexY = pixY / TILE_HEIGHT;
        }

        public Vector2 translateClickToMapLocation(Vector2 click){
            click.X += minViewableX * TILE_WIDTH;
            click.Y += minViewableY * TILE_HEIGHT;

            return click;
        }

        public Vector2 translateClickToMapLocation(int x, int y){
            return new Vector2(x + minViewableX * TILE_WIDTH, y + minViewableY * TILE_HEIGHT);
        }

        public Point translateClickToMapLocation_point(int x, int y){
            return new Point(x + minViewableX * TILE_WIDTH, y + minViewableY * TILE_HEIGHT);
        }

        /// <summary>
        /// Loops around a central location, looking for a passable spot
        /// </summary>
        /// <param name="destination">Destination, in pixels</param>
        /// <returns>Passable location, in pixels</returns>
        public Coordinate getPassableLocation(Coordinate destination){
            Coordinate cell = new Coordinate();
            int radius = 1;
            int dist = 2;
            bool found = false;

            this.translatePixelToMapCell(destination.x, destination.y, out cell.x, out cell.y);

            if(this.getCellOccupancy_cells(cell.x, cell.y) == (byte)1){
                return destination;
            }

            //loop around the surrounding area looking for '(byte)1'

            do
            {
                cell.x -= 1;
                cell.y -= 1;

                //Move right
                for (int i = 0; i < dist; i++)
                {
                    //Check bounds
                    checkBounds(cell);

                    if (this.getCellOccupancy_cells(cell.x, cell.y) == (byte)1){
                        found = true;
                        break;
                    }

                    cell.x += 1;
                }

                if (found) break;

                //Move down
                for (int i = 0; i < dist; i++)
                {
                    //Check bounds
                    checkBounds(cell);

                    if (this.getCellOccupancy_cells(cell.x, cell.y) == (byte)1)
                    {
                        found = true;
                        break;
                    }

                    cell.y += 1;
                }

                if (found) break;

                //Move left
                for (int i = 0; i < dist; i++)
                {
                    //Check bounds
                    checkBounds(cell);
                    if (this.getCellOccupancy_cells(cell.x, cell.y) == (byte)1)
                    {
                        found = true;
                        break;
                    }

                    cell.x -= 1;
                }

                if (found) break;

                //Move up
                for (int i = 0; i < dist; i++)
                {
                    //Check bounds
                    checkBounds(cell);

                    if (this.getCellOccupancy_cells(cell.x, cell.y) == (byte)1)
                    {
                        found = true;
                        break;
                    }

                    cell.y -= 1;
                }
                if (found) break;

                dist += 2;

            } while (this.getCellOccupancy_cells(cell.x, cell.y) == (byte)0);

            this.translateMapCellToPixel(cell.x, cell.y, out cell.x, out cell.y);
            return cell;
        }

        private Coordinate checkBounds(Coordinate cell)
        {
            if (cell.x <= 0)
                cell.x = 0;
            else if (cell.y <= 0)
                cell.y = 0;
            else if (cell.x > this.width)
                cell.x = this.width;
            else if (cell.y > this.height)
                cell.y = this.height;

            return cell;
        }

        public Coordinate getClosestPassableLocation(Coordinate origin, Coordinate point){
            //translate pixel points
            this.translatePixelToMapCell(point.x, point.y, out point.x, out point.y);
            this.translatePixelToMapCell(origin.x, origin.y, out origin.x, out origin.y);

            if(this.getCellOccupancy_pixels(point.x, point.y) == (byte)1){
                //passable
                this.translateMapCellToPixel(point.x, point.y, out point.x, out point.y);
            }
            else{
                //NOT passable, find closest location to 'origin'
                int distX = origin.x - point.x;
                int distY = origin.y - point.y;
                byte curPass = (byte)0;

                if(distX < distY){
                    //point is closer with respect to X
                    while(curPass == (byte)0){
                        if (distX < 0)
                            point.x -= 1;
                        else
                            point.x += 1;

                        if (point.x < 0 || point.x > this.width)
                            return null;

                        curPass = this.getCellOccupancy_pixels(point.x, point.y);
                    }
                }
                else{
                    while (curPass == (byte)0)
                    {
                        if (distY < 0)
                            point.y -= 1;
                        else
                            point.y += 1;

                        if (point.y < 0 || point.y > this.height)
                            return null;

                        curPass = this.getCellOccupancy_pixels(point.x, point.y);
                    }
                }
            }

            this.translateMapCellToPixel(point.x, point.y, out point.x, out point.y);

            return point;
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

        public virtual void loadTerrain(){
            //for now, do this
            BaseMapEntity tex1 = new BaseMapEntity(TextureBank.MapTiles.grass);
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    this.addMapEntity(tex1, i, j);
                }
            }
        }
    }

}
