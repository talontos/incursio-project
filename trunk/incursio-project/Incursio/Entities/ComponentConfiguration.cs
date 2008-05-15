using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Entities.Components;

namespace Incursio.Entities
{
    public class ComponentConfiguration
    {
        public string type;
        public List<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>();

        public ComponentConfiguration(string type){
            this.type = type;
        }

        public void addAttribute(KeyValuePair<string, string> att){
            if (!attributes.Contains(att))
                attributes.Add(att);
        }

        public bool addToEntity( BaseGameEntity e){
            BaseComponent bc;
            switch(this.type){
                case "AudioComponent":      bc = new AudioComponent(e);         e.audioComponent =      (AudioComponent)bc;      break;
                case "CombatComponent":     bc = new CombatComponent(e);        e.combatComponent =     (CombatComponent)bc;     break;
                case "ExperienceComponent": bc = new ExperienceComponent(e);    e.experienceComponent = (ExperienceComponent)bc; break;
                case "FactoryComponent":    bc = new FactoryComponent(e);       e.factoryComponent =    (FactoryComponent)bc;    break;
                case "HealComponent":       bc = new HealComponent(e);          e.healComponent =       (HealComponent)bc;       break;
                case "MovementComponent":   bc = new MovementComponent(e);      e.movementComponent =   (MovementComponent)bc;   break;
                case "RenderComponent":     bc = new RenderComponent(e);        e.renderComponent =     (RenderComponent)bc;     break;
                case "ResourceComponent":   bc = new ResourceComponent(e);      e.resourceComponent =   (ResourceComponent)bc;   break;
                default: return false;
            }

            bc.setAttributes(attributes);
            return true;
        }
    }
}
