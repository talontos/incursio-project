using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

using Incursio.Managers;

namespace Incursio.Utils
{
    public class ObjectFactory
    {
        private Incursio incursio;

        private static ObjectFactory instance;

        public static ObjectFactory getInstance(){
            if (instance == null)
                instance = new ObjectFactory(Incursio.getInstance());

            return instance;
        }

        private ObjectFactory(){

        }

        //TODO: REMOVE CONSTRUCTOR
        public ObjectFactory(Incursio main){
            this.incursio = main;
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
            if(product is Hero && owningPlayer == State.PlayerId.HUMAN && Incursio.getInstance().getHero() != null){
                (product as Hero).copyHeroStats(Incursio.getInstance().getHero());

                Incursio.getInstance().setHero(product as Hero);
            }

            product.playEnterBattlefieldSound();

            return product;

        }

    }
}
