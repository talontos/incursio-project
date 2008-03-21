using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;
using Incursio.Commands;

namespace Incursio.Managers
{
    public class EntityManager
    {
        private static EntityManager instance;

        private List<BaseGameEntity> entityBank;
        private List<BaseGameEntity> selectedUnits;
        private ObjectFactory factory;

        private int nextKeyId;

        private EntityManager(){
            entityBank = new List<BaseGameEntity>();
            selectedUnits = new List<BaseGameEntity>();
            factory = ObjectFactory.getInstance();
            nextKeyId = 0;
        }

        public static EntityManager getInstance(){
            if (instance == null)
                instance = new EntityManager();
            return instance;
        }

        ////////////Functional Methods////////////

        public void updateAllEntities(GameTime gameTime)
        {
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                e.Update(gameTime, ref e);
            });
        }

        public void updateUnitSelection(Cursor cursorState){

        }

        public BaseGameEntity getEntity(int keyId){
            if(keyId <= this.entityBank.Count)
                return this.entityBank[keyId];

            return null;
        }

        public List<BaseGameEntity> getPlayerEntities(State.PlayerId player){
            List<BaseGameEntity> playerEntities = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e){
                if (e.getPlayer() == player)
                    playerEntities.Add(e);
            });

            return playerEntities;
        }

        public BaseGameEntity createNewEntity(String entityType, State.PlayerId player){
            //TODO: implement this, duh!
            BaseGameEntity product = this.factory.create(entityType, player);
            product.keyId = nextKeyId;
            this.entityBank.Insert(nextKeyId++, product);

            return product;
        }

        /// <summary>
        /// Issues a command to all selected units
        /// </summary>
        /// <param name="commandType">Enumerated command type identifying the command</param>
        /// <param name="args">A list of arguments dependent upon the command type</param>
        public void issueCommand(State.Command commandType, params Object[] args){
            BaseCommand command = null;
            switch (commandType)
            {
                ////////////////////////
                case State.Command.MOVE:
                    //TODO: PATHING!!
                    command = new MoveCommand(args[0] as Coordinate);
                    break;

                ////////////////////////
                case State.Command.ATTACK:
                    command = new AttackCommand(args[0] as BaseGameEntity);
                    break;

                ////////////////////////
                case State.Command.STOP:
                    command = new StopCommand();
                    break;

                ////////////////////////
                case State.Command.FOLLOW:
                    command = new FollowCommand(args[0] as Unit);
                    break;

                ////////////////////////
                case State.Command.GUARD:
                    command = new GuardCommand();
                    break;

                ////////////////////////
            }

            if (command == null) return;

            this.selectedUnits.ForEach(delegate(BaseGameEntity e)
            {
                //add command
                e.issueSingleOrder(command);
            });
        }

        public void battleEntities(BaseGameEntity attacker, BaseGameEntity attackee){

        }

    }
}
