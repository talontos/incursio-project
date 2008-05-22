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
        /// Creates an instance a class using a BaseGameEntityConfiguration stored with ID 'classID'
        /// </summary>
        /// <param name="classID">ID of configuration instance</param>
        /// <param name="owningPlayerID">ID of player who will be set as the owner</param>
        /// <returns>The instantiated entity</returns>
        public BaseGameEntity create(int classID, int owningPlayerID)
        {
            BaseGameEntityConfiguration config;

            try{
                config = this.entities[classID];
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
