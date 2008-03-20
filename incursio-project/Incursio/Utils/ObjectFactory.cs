using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Utils
{
    public class ObjectFactory
    {
        private Incursio incursio;

        public ObjectFactory(){

        }

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

            if (product is Unit)
                (product as Unit).setMap(Incursio.getInstance().currentMap);

            this.incursio.addEntity(ref product);
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

            if (product is Unit)
                (product as Unit).setMap(Incursio.getInstance().currentMap);

            this.incursio.addEntity(ref product);
            return product;
        }

    }
}
