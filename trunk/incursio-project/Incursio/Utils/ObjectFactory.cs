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


using Incursio.Managers;
using Incursio.Entities;

namespace Incursio.Utils
{
    public class ObjectFactory
    {
        private static ObjectFactory instance;

        public int heroId = -1;
        public int mainBaseId = -1;
        public int controlPointId = -1;
        public List<int> turretIds = new List<int>(0);

        private List<BaseGameEntityConfiguration> _entities = new List<BaseGameEntityConfiguration>();
        
        public List<BaseGameEntityConfiguration> entities{
            get { return this._entities; }
            set { 
                this._entities = value;

                foreach (BaseGameEntityConfiguration c in _entities)
                {
                    if (c.isHero)
                        heroId = c.classID;
                    else if (c.isMainBase)
                        mainBaseId = c.classID;
                    else if (c.isControlPoint)
                        controlPointId = c.classID;
                    else if (c.isTurret)
                        turretIds.Add(c.classID);
                }
            }
        }

        public static ObjectFactory getInstance(){
            if (instance == null)
                instance = new ObjectFactory();

            return instance;
        }

        private ObjectFactory(){
            
        }

        public void setSpecialEntityIds(out int hId, out int bId, out int cpId){
            hId = heroId;
            bId = mainBaseId;
            cpId = controlPointId;
        }

        public BaseGameEntity create(string entityName, int owningPlayerID){
            for(int i = 0; i < this.entities.Count; i++){
                if(entities[i].className.Equals(entityName)){
                    return this.create(i, owningPlayerID);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates an instance a class using a BaseGameEntityConfiguration stored with ID 'classID'
        /// </summary>
        /// <param name="classID">ID of configuration instance</param>
        /// <param name="owningPlayerID">ID of player who will be set as the owner</param>
        /// <returns>The instantiated entity</returns>
        public BaseGameEntity create(int classID, int owningPlayerID)
        {
            BaseGameEntityConfiguration config = null;

            try{
                config = this.entities[classID];
            }
            catch (ArgumentOutOfRangeException aor)
            {

            }
            catch(IndexOutOfRangeException ior){
                //TODO: WRITE TO ERROR LOG?
                Console.Write(ior.StackTrace);
                return null;
            }

            if (config == null)
                return null;

            // build new <classname>
            BaseGameEntity product = config.buildEntity();

            product.setPlayer(owningPlayerID);

            //check for hero stats to load
            /*
            if (product.isHero && Incursio.getInstance().getHero() != null)
            {

                if (owningPlayer == PlayerManager.getInstance().currentPlayerId)
                {
                    (product as Hero).copyHeroStats(Incursio.getInstance().getHero());

                    Incursio.getInstance().setHero(product as Hero);
                }
                else
                {
                    //computer hero, set stuff
                    (product as Hero).setHero_Badass();
                }
            }
            */

            //product.playEnterBattlefieldSound();

            return product;
        }
    }
}
