using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Entities.Components
{
    public class ExperienceComponent : BaseComponent
    {
        public int level = 1;
        public int experiencePoints = 0;
        public int pointsToNextLevel = 1000;

        public int healthIncrement = 0;
        public int damageIncrement = 0;
        public int armorIncrement = 0;

        public ExperienceComponent(BaseGameEntity entity) : base(entity){

        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i].Key)
                {
                    //TODO: EXP AND LVL ALWAYS NEED TO BE PROPORTIONATE!!!!!
                    case "level": level = int.Parse(attributes[i].Value); break;
                    case "experiencePoints": experiencePoints = int.Parse(attributes[i].Value); break;
                    case "pointsToNextLevel": pointsToNextLevel = int.Parse(attributes[i].Value); break;
                    case "healthIncrement": healthIncrement = int.Parse(attributes[i].Value); break;
                    case "damageIncrement": damageIncrement = int.Parse(attributes[i].Value); break;
                    case "armorIncrement": armorIncrement = int.Parse(attributes[i].Value); break;
                    default: break;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public double getPercentageLvlUp()
        {
            return (float)experiencePoints / pointsToNextLevel;
        }

        public void gainExperience(int exp)
        {
            this.experiencePoints += exp;

            //CHECK FOR LEVEL-UP
            if (experiencePoints >= pointsToNextLevel)
            {
                level++;

                //increment health
                this.bgEntity.maxHealth += this.healthIncrement;

                //increment damage
                if(this.bgEntity.combatComponent != null)
                    this.bgEntity.combatComponent.damage += this.damageIncrement;
                
                //increment defense
                this.bgEntity.armor += this.armorIncrement;

                PlayerManager.getInstance().notifyPlayer(this.bgEntity.owner,
                    new GameEvent(State.EventType.LEVEL_UP, this.bgEntity, SoundManager.getInstance().AudioCollection.messages.lvlUp, (this.bgEntity.entityName + " Level Up!"), this.bgEntity.location));

                //TODO: Review this number - we might want to make it smaller
                pointsToNextLevel *= level;
            }
        }
    }
}
