/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

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
using Incursio.World.Terrain;
using Incursio.Utils.PathFinding;
using Incursio.Entities;

namespace Incursio.World
{
    public class MapBase
    {
        //member variables

        //width/height, in cells
        public int width;
        public int height;

        //width/height, in pixels
        public int width_pix;
        public int height_pix;

        protected BaseMapEntity[,] tileGrid;
        protected List<BaseMapEntity> objectList;

        public byte[,] occupancyGrid;

        public int[,] entityGrid;

        public int TILE_WIDTH = MapManager.TILE_WIDTH;
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
            this.entityGrid = new int[0, 0];
            this.objectList = new List<BaseMapEntity>();
        }

        public MapBase(int width, int height, int screenWidth, int screenHeight)
        {
            this.width_pix = width;
            this.height_pix = height;
            this.width = width / TILE_WIDTH;
            this.height = height / TILE_HEIGHT;
            this.tileGrid = new BaseMapEntity[this.width, this.height];
            this.occupancyGrid = new byte[this.width, this.height];
            this.entityGrid = new int[this.width, this.height];
            this.minViewableX = 0;
            this.minViewableY = 0;
            this.maxViewableX = screenWidth / TILE_WIDTH;
            this.maxViewableY = (screenHeight - (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height)) / TILE_HEIGHT;
            this.cameraMovePause = 0;

            //initialize occupancyGrid and entityGrid
            for (int x = 0; x < this.height; x++)
            {
                for (int y = 0; y < this.width; y++)
                {
                    occupancyGrid[x, y] = Util.UNOCCUPIED;
                    entityGrid[x, y] = -1;
                }
            }

            this.objectList = new List<BaseMapEntity>();
        }

        public virtual void setMapDimensions(int width, int height, int screenWidth, int screenHeight)
        {
            this.width_pix = width;
            this.height_pix = height;
            this.width = width / TILE_WIDTH;
            this.height = height / TILE_HEIGHT;
            this.tileGrid = new BaseMapEntity[this.width, this.height];
            this.occupancyGrid = new byte[this.width, this.height];
            this.entityGrid = new int[this.width, this.height];
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
                    occupancyGrid[i, j] = Util.UNOCCUPIED;
                    entityGrid[i, j] = -1;
                }
            }
        }

        public virtual GameResult inspectWinConditions()
        {
            return new GameResult();
        }

        public virtual void initializeMap()
        {
            this.loadTerrain();
        }

        public void update()
        {

            if (InputManager.getInstance().MOVE_RIGHT && maxViewableX < this.width)
            {
                this.maxViewableX++;
                this.minViewableX++;
            }
            else if (InputManager.getInstance().MOVE_LEFT && minViewableX > 0)
            {
                this.maxViewableX--;
                this.minViewableX--;
            }

            if (InputManager.getInstance().MOVE_UP && minViewableY > 0)
            {
                this.maxViewableY--;
                this.minViewableY--;
            }
            else if (InputManager.getInstance().MOVE_DOWN && maxViewableY < this.height)
            {
                this.maxViewableY++;
                this.minViewableY++;
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
                    if (MapManager.getInstance().DRAW_OCCUPANCY_GRID)
                    {
                        if (this.occupancyGrid[i, j] == (byte)0)
                            mask = Color.CornflowerBlue;
                        else
                            mask = Color.White;
                    }

                    spriteBatch.Draw(tileGrid[i, j].texture, new Rectangle(screenX * TILE_WIDTH, screenY * TILE_HEIGHT, TILE_WIDTH, TILE_HEIGHT), mask);

                    if (MapManager.getInstance().DRAW_ENTITY_GRID)
                    {
                        if(entityGrid[i, j] > -1)
                            spriteBatch.DrawString(Incursio.getInstance().getFont_Arial(), this.entityGrid[i, j].ToString(), new Vector2(screenX * TILE_WIDTH, screenY * TILE_HEIGHT), Color.Black);
                    }
                    
                    screenX++;
                }
                screenY++;
            }

            //draw objects on map (trees, brushes, etc)
            objectList.ForEach(delegate(BaseMapEntity e)
            {
                int xPos;
                int yPos;
                this.translateMapCellToPixel(e.location.x, e.location.y, out xPos, out yPos);

                if(this.isOnScreen(new Coordinate(xPos, yPos)))
                {
                    Coordinate locationOnScreen = this.positionOnScreen(new Coordinate(xPos, yPos));

                    spriteBatch.Draw(e.texture,
                        new Rectangle((int)(locationOnScreen.x - (e.texture.Width / 2)), (int)(locationOnScreen.y - (e.texture.Height * 0.90)),
                        e.texture.Width, e.texture.Height), 
                        Color.White);
                }
            });
        }


        public void addMapEntity(BaseMapEntity entity, int xPos, int yPos)
        {
            if (xPos >= 0 && xPos < this.width && yPos >= 0 && yPos < this.height)
            {
                this.tileGrid[xPos, yPos] = entity;
                this.occupancyGrid[xPos, yPos] = entity.passable ? (byte)1 : (byte)0;
            }
        }

        public void addObjectEntity(BaseMapEntity entity)
        {
            if (entity.location.x >= 0 && entity.location.x < this.width && entity.location.y >= 0 && entity.location.y < this.height)
            {
                this.objectList.Add(entity);
                entity.setOccupancy(ref occupancyGrid);
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
                return new Coordinate(0,0);
            }
        }

        public void moveCameraToEvent(Coordinate coords)
        {
            int cellX, cellY;
            this.translatePixelToMapCell(coords.x, coords.y, out cellX, out cellY);

            int viewableX = maxViewableX - minViewableX;
            int viewableY = maxViewableY - minViewableY;

            if (cellX - (viewableX / 2) < 0)
            {
                minViewableX = 0;
                maxViewableX = viewableX;
            }
            else if (cellX + (viewableX / 2) > this.width)
            {
                minViewableX = width - viewableX;
                maxViewableX = width;
            }
            else
            {
                minViewableX = cellX - (viewableX / 2);
                maxViewableX = cellX + (viewableX / 2);
            }

            if (cellY - (viewableY / 2) < 0)
            {
                minViewableY = 0;
                maxViewableY = viewableY;
            }
            else if (cellY + (viewableY / 2) > this.height)
            {
                minViewableY = height - viewableY;
                maxViewableY = height;
            }
            else
            {
                minViewableY = cellY - (viewableY / 2);
                maxViewableY = cellY + (viewableY / 2);
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
        public bool requestMove(int x1, int y1, int x2, int y2)
        {
            int a, b, c, d;

            this.translatePixelToMapCell(x1, y1, out a, out b);
            this.translatePixelToMapCell(x2, y2, out c, out d);

            if (a == c && b == d)
            {
                //same cell, is ok
                return true;
            }
            else
            {
                return this.occupancyGrid[c, d] == Util.UNOCCUPIED;
            }
        }

        public byte getCellOccupancy_cells(int x, int y)
        {
            if (x < occupancyGrid.GetLowerBound(0) || x > occupancyGrid.GetUpperBound(0) ||
                y < occupancyGrid.GetLowerBound(1) || y > occupancyGrid.GetUpperBound(1))
                return (byte)0;
            else
                return this.occupancyGrid[x, y];
        }

        public byte getCellOccupancy_pixels(int pixX, int pixY)
        {
            int x, y;
            this.translatePixelToMapCell(pixX, pixY, out x, out y);

            if (x < occupancyGrid.GetLength(0) && y < occupancyGrid.GetLength(1))
                return this.occupancyGrid[x, y];

            else return Util.OCCUPIED;
        }

        public void setSingleCellOccupancy_pix(int pixX, int pixY, byte occupied)
        {
            int x, y;
            this.translatePixelToMapCell(pixX, pixY, out x, out y);
            this.occupancyGrid[x, y] = occupied;
        }
        
        public void setSingleCellOccupancy_cell(int cellX, int cellY, byte occupied)
        {
            if(cellX < occupancyGrid.GetLength(0) && cellY < occupancyGrid.GetLength(1))
                this.occupancyGrid[cellX, cellY] = occupied;
        }

        public int getCellDistance(Coordinate c1, Coordinate c2)
        {
            int a, b, c, d;

            this.translatePixelToMapCell(c1.x, c1.y, out a, out b);
            this.translatePixelToMapCell(c2.x, c2.y, out c, out d);

            Vector2 v1 = new Vector2(a, b);
            Vector2 v2 = new Vector2(c, d);

            return Convert.ToInt32(Math.Abs(Vector2.Distance(v1, v2)));

        }

        public int getCellEntity_cells(int x, int y)
        {
            return this.entityGrid[x, y];
        }

        public int getCellEntity_pixels(int pixX, int pixY)
        {
            int x, y;
            this.translatePixelToMapCell(pixX, pixY, out x, out y);

            if (x >= 0 && x < this.entityGrid.GetUpperBound(0) && y >= 0 && y < this.entityGrid.GetUpperBound(1))
                return this.entityGrid[x, y];
            else return -1;
        }

        public void setSingleCellEntity_cell(int cellX, int cellY, int occupant)
        {
            //int x, y;
            //this.translatePixelToMapCell(pixX, pixY, out x, out y);
            this.entityGrid[cellX, cellY] = occupant;
        }

        public void translateMapCellToPixel(int indexX, int indexY, out int pixX, out int pixY)
        {
            pixX = (indexX * TILE_WIDTH + (TILE_WIDTH / 2));
            pixY = (indexY * TILE_HEIGHT + (TILE_HEIGHT / 2));
        }

        public void translatePixelToMapCell(int pixX, int pixY, out int indexX, out int indexY)
        {
            indexX = pixX / TILE_WIDTH;
            indexY = pixY / TILE_HEIGHT;
        }

        public Vector2 translateClickToMapLocation(Vector2 click)
        {
            click.X += minViewableX * TILE_WIDTH;
            click.Y += minViewableY * TILE_HEIGHT;

            return click;
        }

        public Vector2 translateClickToMapLocation(int x, int y)
        {
            return new Vector2(x + minViewableX * TILE_WIDTH, y + minViewableY * TILE_HEIGHT);
        }

        public Point translateClickToMapLocation_point(int x, int y)
        {
            return new Point(x + minViewableX * TILE_WIDTH, y + minViewableY * TILE_HEIGHT);
        }

        public Coordinate getClosestPassableLocation_new(Coordinate origin, Coordinate point)
        {

            if (getCellOccupancy_pixels(point.x, point.y) == (byte)1)
            {
                return point;
            }

            Coordinate cellOrigin = new Coordinate();
            Coordinate cellDestination = new Coordinate();

            //translate pixel points
            translatePixelToMapCell(origin.x, origin.y, out cellOrigin.x, out cellOrigin.y);
            translatePixelToMapCell(point.x, point.y, out cellDestination.x, out cellDestination.y);

            //find a path from the origin to the destination with length l & return the last node
            List<PathReturnNode> path = MapManager.getInstance().pathFinder.FindPath(cellOrigin.toPoint(), cellDestination.toPoint(), 1, -1);

            //convert back to pixels
            //If path not found, return origin
            if (path == null)
                this.translateMapCellToPixel(cellOrigin.x, cellOrigin.y, out cellDestination.x, out cellDestination.y);
            else
                this.translateMapCellToPixel(path[0].PosX, path[0].PosY, out cellDestination.x, out cellDestination.y);

            return (cellDestination);
        }

        public Coordinate getClosestPassableLocation(Coordinate origin, Coordinate point)
        {

            if (getCellOccupancy_pixels(point.x, point.y) == (byte)1)
            {
                return point;
            }

            Coordinate cellOrigin = new Coordinate();
            Coordinate cellDestination = new Coordinate();

            //translate pixel points
            translatePixelToMapCell(origin.x, origin.y, out cellOrigin.x, out cellOrigin.y);
            translatePixelToMapCell(point.x, point.y, out cellDestination.x, out cellDestination.y);

            
            //find where origin is in relation to point; there's only 8 possible directions...

            //dir vars: true == left/up;
            //          false == right/down;
            bool horizontalLeft = (cellOrigin.x > cellDestination.x);
            bool verticalUp = (cellOrigin.y > cellDestination.y);

            //2 special cases: (4 cases total
            if (cellOrigin.x == cellDestination.x)
            {
                if (verticalUp)
                {
                    //origin.y < dest.y; add to y value
                    while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.y <= this.height_pix)
                    {
                        cellDestination.y += 1;
                    }
                }
                else
                {
                    //subtract from y value
                    while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.y >= 0)
                    {
                        cellDestination.y -= 1;
                    }
                }
            }


            else if (cellOrigin.y == cellDestination.y)
            {
                if (horizontalLeft)
                {
                    //origin.x < dest.x; add to x value
                    while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.x <= this.width_pix)
                    {
                        cellDestination.x += 1;
                    }
                }
                else
                {
                    //subtract from x value
                    while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.x >= 0)
                    {
                        cellDestination.x -= 1;
                    }
                }
            }

            //TODO: MAKE MORE ROBUST
            else if (horizontalLeft && verticalUp)
            {
                //origin to northwest of destination, add to x && y values
                while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.x <= this.width_pix && cellDestination.y <= this.height_pix)
                {
                    cellDestination.x += 1;
                    cellDestination.y += 1;
                }
            }

            else if (horizontalLeft && !verticalUp)
            {
                //origin to southeast of destination, add to x, subtract from y
                while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.x <= this.width_pix && cellDestination.y >= 0)
                {
                    cellDestination.x += 1;
                    cellDestination.y -= 1;
                }
            }

            else if (!horizontalLeft && verticalUp)
            {
                //origin northeast of destination, subtract from x, add to y
                while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.x >= 0 && cellDestination.y <= this.height_pix)
                {
                    cellDestination.x -= 1;
                    cellDestination.y += 1;
                }
            }

            else
            {
                //origin to southeast of destination, subtract from x && y
                while (getCellOccupancy_cells(cellDestination.x, cellDestination.y) == (byte)0 &&
                    cellDestination.x >= 0 && cellDestination.y >= 0)
                {
                    cellDestination.x -= 1;
                    cellDestination.y -= 1;
                }
            }

            //convert back to pixels
            this.translateMapCellToPixel(cellDestination.x, cellDestination.y, out cellDestination.x, out cellDestination.y);

            return cellDestination;
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

        public virtual void loadTerrain()
        {
            //Cover map with grass
            Grass grass;
            for (int j = 0; j < this.height; j++)
            {
                for (int i = 0; i < this.width; i++)
                {
                    grass = new Grass(i, j);
                    this.addMapEntity(grass, i, j);
                    //grass doesn't need to set occupancy
                }
            }
        }

        /// <summary>
        /// Returns a List of keyIds of all entities within SightRange from locatoin
        /// </summary>
        /// <param name="location">Location, in pixels</param>
        /// <param name="sightRange"></param>
        /// <returns>List of entities</returns>
        public List<int> getEntitiesInRange(Coordinate location, int sightRange)
        {
            //TODO: Should we look in a square, or circle-ish shape?????
            //Currently square
            List<int> entities = new List<int>();
            Point cellLocation = new Point();
            translatePixelToMapCell(location.x, location.y, out cellLocation.X, out cellLocation.Y);

            int xStart = 0;
            int yStart = 0;
            int xEnd = 0;
            int yEnd = 0;

            if (cellLocation.X - sightRange < 0)
            {
                yStart = 0;
            }
            else
            {
                xStart = Math.Max(0, cellLocation.X - sightRange);
            }

            if (cellLocation.X + sightRange >= MapManager.getInstance().currentMap.width)
            {
                xStart = MapManager.getInstance().currentMap.width - 1;
            }
            else
            {
                xEnd = Math.Min(cellLocation.X + sightRange, this.width);
            }

            if (cellLocation.Y - sightRange < 0)
            {
                yStart = 0;
            }
            else
            {
                yStart = Math.Max(0, cellLocation.Y - sightRange);
            }

            if (cellLocation.Y + sightRange >= MapManager.getInstance().currentMap.height)
            {
                yEnd = MapManager.getInstance().currentMap.height - 1;
            }
            else
            {
                yEnd = Math.Min(cellLocation.Y + sightRange, this.height);
            }



            for (int y = yStart; y <= yEnd; y++)
            {
                for (int x = xStart; x <= xEnd; x++)
                {
                    if (entityGrid[x, y] >= 0)
                        entities.Add(entityGrid[x, y]);
                }
            }

            return entities;
        }

        public void convertToXml(){
            //Terrain
            for(int x = 0; x < this.tileGrid.GetUpperBound(0); x++){
                for(int y = 0; y < this.tileGrid.GetUpperBound(1); y++){
                    String node = "<Tile type=\"" + "TODO: SET TERRAIN TYPE" + "\" x=\"" + x + "\" y=\"" + y + "/>";
                }
            }

            //Objects
        }
    }

}
