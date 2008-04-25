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
    public class CampStructure : Structure
    {
        //constants
        public const int LIGHT_INFANTRY_BUILD_TIME = 5;
        public const int HEAVY_INFANTRY_BUILD_TIME = 10;
        public const int ARCHER_BUILD_TIME = 7;
        public const int GUARD_TOWER_BUILD_TIME = 15;
        public const int ITEM_UPGRADE_BUILD_TIME = 90;
        public const int HEAL_TICK = 4;

        public const int COST_LIGHT_INFANTRY = 45;
        public const int COST_HEAVY_INFANTRY = 90;
        public const int COST_ARCHER = 60;
        public const int COST_GUARD_TOWER = 200;
        public const int COST_UPGRADE = 200;

        int newUnitPlacementX = 10;
        int newUnitPlacementY = 120;    //little bit of hard coding, but can't really help it here 
        int income = 8;

        private int healTimer = 0;
        public int healRange = 4;
        public int healthBoost = 10;

        Coordinate destination;
        Coordinate newStructureCoords;

        String currentlyBuildingThis = "IDLE"; //this is for the HUD to display what it's building
        String currentBuildForObjectFactory = "IDLE"; //and this is for the object factory

        public CampStructure() : base(){
            this.pointValue = 1000;

            //TODO: Set camp properties
            this.maxHealth = 350;
            this.health = 350;
            this.sightRange = 6;
            this.setType(State.EntityName.Camp);
            this.map = Incursio.getInstance().currentMap;

            setDefaultDestination();

            this.isConstructor = true;
        }

        public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
        {
            if (this.healTimer == HEAL_TICK * 90)
            {
                EntityManager.getInstance().healEntitiesInRange(this, this.healRange, true);
                healTimer = 0;
            }
            else if (this.healTimer == HEAL_TICK * 60)
            {
                //heal units in range
                EntityManager.getInstance().healEntitiesInRange(this, this.healRange, false);
                healTimer++;
            }
            else
            {
                healTimer++;
            }

            this.updateOccupancy(true);
            base.Update(gameTime, ref myRef);
        }

        public override void build(EntityBuildOrder toBeBuilt)
        {
            if (buildProject != null)
            {

            }
            else
            {
                Player owningPlayer;
                if (this.owner == State.PlayerId.HUMAN)
                    owningPlayer = PlayerManager.getInstance().humanPlayer;
                else
                    owningPlayer = PlayerManager.getInstance().computerPlayer;

                if(toBeBuilt.location != null){
                    if(toBeBuilt.entity is GuardTowerStructure){
                        this.setNewStructureCoords(toBeBuilt.location);
                    }
                    else{
                        this.setDestination(toBeBuilt.location);
                    }
                }

                if (toBeBuilt.entity.getType() == State.EntityName.LightInfantry)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_LIGHT_INFANTRY)
                    {
                        currentlyBuildingThis = "Light Infantry";
                        currentBuildForObjectFactory = "Incursio.Classes.LightInfantryUnit";
                        this.timeBuilt = 0;
                        this.timeRequired = LIGHT_INFANTRY_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt.entity;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_LIGHT_INFANTRY;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                this,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                    }

                }
                else if (toBeBuilt.entity.getType() == State.EntityName.Archer)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_ARCHER)
                    {
                        currentlyBuildingThis = "Archer";
                        currentBuildForObjectFactory = "Incursio.Classes.ArcherUnit";
                        this.timeBuilt = 0;
                        this.timeRequired = ARCHER_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt.entity;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_ARCHER;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                this,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                    }

                }
                else if (toBeBuilt.entity.getType() == State.EntityName.HeavyInfantry)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_HEAVY_INFANTRY)
                    {
                        currentlyBuildingThis = "Heavy Infantry";
                        currentBuildForObjectFactory = "Incursio.Classes.HeavyInfantryUnit";
                        this.timeBuilt = 0;
                        this.timeRequired = HEAVY_INFANTRY_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt.entity;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_HEAVY_INFANTRY;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                this,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                    }

                }
                else if (toBeBuilt.entity.getType() == State.EntityName.GuardTower)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_GUARD_TOWER)
                    {
                        currentlyBuildingThis = "Guard Tower";
                        currentBuildForObjectFactory = "Incursio.Classes.GuardTowerStructure";
                        this.timeBuilt = 0;
                        this.timeRequired = GUARD_TOWER_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt.entity;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_GUARD_TOWER;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                this,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                    }

                }
            }
        }


        public override void updateResourceTick()
        {
            //give the owner money
            if (timeForResource >= RESOURCE_TICK * 60)
            {
                timeForResource = 0;
                if (this.owner == State.PlayerId.HUMAN)
                {
                    MessageManager.getInstance().addMessage(new GameEvent(State.EventType.GAIN_RESOURCE, this, Convert.ToString(income), this.location));

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

        public override void buildTick()
        {
            if (this.timeBuilt >= this.timeRequired)
            {
                if (buildProject.getType() != State.EntityName.GuardTower)
                {
                    Unit temp = ( EntityManager.getInstance().createNewEntity(currentBuildForObjectFactory, this.owner) as Unit);
                    temp.setLocation(this.location);
                    temp.issueSingleOrder(new MoveCommand(this.destination));
                    timeBuilt = 0;
                    timeRequired = 0;
                    this.currentState = State.StructureState.Idle;
                    this.currentBuildForObjectFactory = "IDLE";
                    this.currentlyBuildingThis = "IDLE";
                    this.buildProject = null;

                    PlayerManager.getInstance().notifyPlayer(
                        this.owner,
                        new GameEvent(State.EventType.CREATION_COMPLETE,
                            temp,
                            /*SOUND,*/
                            "Unit Ready",
                            this.location
                        )
                    );
                }
                else
                {
                    GuardTowerStructure temp = (EntityManager.getInstance().createNewEntity(currentBuildForObjectFactory, this.owner) as GuardTowerStructure);
                    temp.setLocation(newStructureCoords);
                    timeBuilt = 0;
                    timeRequired = 0;
                    this.currentState = State.StructureState.Idle;
                    this.currentBuildForObjectFactory = "IDLE";
                    this.currentlyBuildingThis = "IDLE";
                    this.buildProject = null;
                    
                    PlayerManager.getInstance().notifyPlayer(
                        this.owner,
                        new GameEvent(State.EventType.CREATION_COMPLETE,
                            temp,
                            /*SOUND,*/ 
                            "Construction Complete", 
                            this.location
                        )
                    );
                }
            }
            else
            {
                this.timeBuilt++;
            }
        }

        public String getCurrentlyBuilding()
        {
            return currentlyBuildingThis;
        }

        public override void setDestination(Coordinate dest)
        {
            if(dest != null)
                this.destination = dest;
        }

        public void setNewStructureCoords(Coordinate coords)
        {
            if(coords != null && MapManager.getInstance().currentMap.getCellOccupancy_pixels(coords.x, coords.y) == (byte)1){
                this.newStructureCoords = coords;
            }
            else{
                PlayerManager.getInstance().notifyPlayer(
                    this.owner,
                    new GameEvent(State.EventType.CANT_MOVE_THERE,
                        this,
                        /*SOUND,*/
                        "Cannot Build There",
                        this.location
                    )
                );
            }
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

            setDefaultDestination();
        }

        public override void updateBounds()
        {
            Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.campTextureComputer;

            this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
                location.x - myRef.Width / 2,
                (int)(location.y - myRef.Height * 0.80),
                myRef.Width,
                myRef.Height
            );
        }

        public double getPercentDone()
        {
            if (this.isBuilding())
            {
                return (float)timeBuilt / timeRequired;
            }
            else return -1.0;
            
        }

        private void setDefaultDestination(){
            this.destination = new Coordinate(this.location.x + newUnitPlacementX, this.location.y + newUnitPlacementY);
        }

        /// <summary> 
        /// For camps, we don't want to lose our queue.
        /// If we are issued a 'single' order (which by default empties order queue),
        /// we want to execute this order immediately and then continue our build queue.
        /// </summary>
        /// <param name="order">Order to be issued</param>
        public override void issueSingleOrder(BaseCommand order)
        {
            base.issueImmediateOrder(order);
        }

        /// <summary> 
        /// For camps, we don't want to lose our queue.
        /// If we are issued a List of orders (which by default empties order queue),
        /// we want to add these to the end of the queue
        /// </summary>
        /// <param name="order">Order to be issued</param>
        public override void issueOrderList(params BaseCommand[] commands)
        {
            this.orders.AddRange(commands);
        }

        public void cancelCurrentOrder(){
            if (this.orders.Count > 0)
                this.orders.RemoveAt(0);
        }

        public override void drawThyself(ref SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
        {
            this.visible = true;
            this.justDrawn = false;
            //onScreen = currentMap.positionOnScreen(this.getLocation());
            //Rectangle unit = new Rectangle(this.getLocation().x, this.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
            Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.location);
            Rectangle unit = this.boundingBox;

            if (this.currentState == State.StructureState.BeingBuilt)
            {
                //TODO: draw construction?
            }
            else if (this.currentState == State.StructureState.Building)
            {
                //TODO: draw something special for when the structure is building something (fires flickering or w/e)
                if (this.getPlayer() == State.PlayerId.HUMAN)
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.campTexturePlayerBuilding,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTexturePlayer.Height * 0.80),
                        TextureBank.EntityTextures.campTexturePlayer.Width, TextureBank.EntityTextures.campTexturePlayer.Height),
                        new Rectangle(this.currentFrameX, this.currentFrameY, 64, 64), Color.White);

                    if (frameTimer >= FRAME_LENGTH)
                    {
                        if (this.currentFrameX < TextureBank.EntityTextures.campTexturePlayerBuilding.Width - 64)
                        {
                            this.currentFrameX = this.currentFrameX + 64;
                        }
                        else
                        {
                            this.currentFrameX = 0;
                        }
                    }
                }
                else
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputer,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                        TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                }

            }
            else if (this.currentState == State.StructureState.Destroyed)
            {
                //TODO: building asploded
                if (this.getPlayer() == State.PlayerId.HUMAN)
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerExploded,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputerDestroyed.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputerDestroyed.Height * 0.80),
                        TextureBank.EntityTextures.campTextureComputerDestroyed.Width, TextureBank.EntityTextures.campTextureComputerDestroyed.Height), Color.White);
                }
                else
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerExploded,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputerDestroyed.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputerDestroyed.Height * 0.80),
                        TextureBank.EntityTextures.campTextureComputerDestroyed.Width, TextureBank.EntityTextures.campTextureComputerDestroyed.Height), Color.White);
                }
            }
            else if (this.currentState == State.StructureState.Idle)
            {
                if (this.getPlayer() == State.PlayerId.HUMAN)
                {
                    spriteBatch.Draw(TextureBank.EntityTextures.campTexturePlayer,
                        new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTexturePlayer.Height * 0.80),
                        TextureBank.EntityTextures.campTexturePlayer.Width, TextureBank.EntityTextures.campTexturePlayer.Height), Color.White);
                }
                else
                {
                    float ratio = (float)this.getHealth() / this.getMaxHealth();

                    if (ratio >= 0.50)
                    {
                        spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputer,
                            new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                            TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                    }
                    else if (ratio < 0.50 && ratio >= 0.25)
                    {
                        spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerDamaged,
                            new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                            TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                    }
                    else if (ratio < 0.25 && ratio >= 0.00)
                    {
                        spriteBatch.Draw(TextureBank.EntityTextures.campTextureComputerDestroyed,
                            new Rectangle(onScreen.x - (TextureBank.EntityTextures.campTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.campTextureComputer.Height * 0.80),
                            TextureBank.EntityTextures.campTextureComputer.Width, TextureBank.EntityTextures.campTextureComputer.Height), Color.White);
                    }

                }
            }
        }
    }
}
