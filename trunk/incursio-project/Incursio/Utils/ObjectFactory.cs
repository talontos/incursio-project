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
using Incursio.Classes;

using Incursio.Managers;
using Incursio.Entities;

namespace Incursio.Utils
{
    public class ObjectFactory
    {
        public List<BaseGameEntityConfiguration> entities = new List<BaseGameEntityConfiguration>();

        private static ObjectFactory instance;

        public static ObjectFactory getInstance(){
            if (instance == null)
                instance = new ObjectFactory();

            return instance;
        }

        private ObjectFactory(){
            
        }

        /// <summary>
        /// Creates an instance of the desired BaseGameEntity, inserts it in to the 
        /// main program's entityBank, and returns it.
        /// </summary>
        /// <param name="classname">the FULLY QUALIFIED NAME of the class. 
        /// Example: Incursio.Classes.BaseGameEntity</param>
        /// <returns>The newly created instance</returns>
        public BaseGameEntity create(String classname){

            // parse classname
            Type classType = Type.GetType(classname, false, true);

            if (classType == null)
                return null;

            // build new <classname>
            BaseGameEntity product = Activator.CreateInstance(classType) as BaseGameEntity;

            product.setMap(MapManager.getInstance().currentMap);

            return product;
        }

        /// <summary>
        /// Creates an instance of the desired BaseGameEntity, inserts it in to the 
        /// main program's entityBank, and returns it.
        /// </summary>
        /// <param name="classname">the FULLY QUALIFIED NAME of the class. 
        /// <param name="owningPlayer">Enumerated PlayerID to be assigned as the owner</param>
        /// <returns>The newly created instance</returns>
        public BaseGameEntity create(String classname, State.PlayerId owningPlayer)
        {
            // parse classname
            Type classType = Type.GetType(classname, false, true);

            if (classType == null)
                return null;

            // build new <classname>
            BaseGameEntity product = Activator.CreateInstance(classType) as BaseGameEntity;

            product.setPlayer(owningPlayer);

            product.setMap(MapManager.getInstance().currentMap);

            //check for hero stats to load
            if(product is Hero && Incursio.getInstance().getHero() != null){

                if(owningPlayer == State.PlayerId.HUMAN){
                    (product as Hero).copyHeroStats(Incursio.getInstance().getHero());

                    Incursio.getInstance().setHero(product as Hero);
                }
                else{
                    //computer hero, set stuff
                    (product as Hero).setHero_Badass();
                }
            }

            product.playEnterBattlefieldSound();

            return product;

        }

        /// <summary>
        /// Creates an instance of the desired BaseGameEntity, inserts it in to the 
        /// main program's entityBank, and returns it.
        /// </summary>
        /// <param name="classname">the FULLY QUALIFIED NAME of the class. 
        /// <param name="owningPlayer">Enumerated PlayerID to be assigned as the owner</param>
        /// <returns>The newly created instance</returns>
        public BaseEntity create(int classID, int owningPlayerID)
        {
            BaseGameEntityConfiguration config;

            try{
                config = this.entities[classID];
            }
            catch(IndexOutOfRangeException ior){
                //TODO: WRITE TO ERROR LOG?
                return null;
            }

            if (config == null)
                return null;

            // build new <classname>
            BaseEntity product = config.buildEntity();

            product.setPlayer(owningPlayerID);

            //check for hero stats to load
            /*
            if (product is Hero && Incursio.getInstance().getHero() != null)
            {

                if (owningPlayer == State.PlayerId.HUMAN)
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

            product.playEnterBattlefieldSound();

            return product;
        }
    }
}
