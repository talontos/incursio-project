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
    class EntityManager
    {
        private static EntityManager instance;

        private List<BaseGameEntity> entityBank;
        private List<BaseGameEntity> selectedUnits;
        private ObjectFactory factory;

        private EntityManager(){
            entityBank = new List<BaseGameEntity>();
            selectedUnits = new List<BaseGameEntity>();
            factory = new ObjectFactory();
        }

        public static EntityManager getInstance(){
            if (instance == null)
                instance = new EntityManager();
            return instance;
        }

        ////////////Functional Methods////////////

        public void updateUnits(GameTime gameTime)
        {
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                e.Update(gameTime);
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
            return this.factory.create(entityType, player);
        }

        /// <summary>
        /// Issues a command to all selected units
        /// </summary>
        /// <param name="commandType">Enumerated command type identifying the command</param>
        /// <param name="args">A list of arguments dependent upon the command type</param>
        public void issueCommand(State.Command commandType, params Object[] args){
            BaseCommand command;
            switch (commandType)
            {
                ////////////////////////
                case State.Command.MOVE:
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
                    command = new FollowCommand(args[0] as BaseGameEntity);
                    break;

                ////////////////////////
                case State.Command.GUARD:
                    command = new GuardCommand();
                    break;

                ////////////////////////
            }

            this.selectedUnits.ForEach(delegate(BaseGameEntity e)
            {
                //add command to e's command Queue
            });
        }

        public void battleEntities(BaseGameEntity attacker, BaseGameEntity attackee){

        }

    }
}
