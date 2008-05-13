using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities
{
    /// <summary>
    /// This is a class used to house all information required to construct an instance of an entity
    /// </summary>
    public class EntityConfiguration
    {
        public int classID;
        public string className;
        public List<KeyValuePair<string, uint>> attributes;

        public EntityConfiguration(int id, string name, params string[] args){
            this.classID = id;
            this.className = name;

            //args?
        }

        public void addAttrubute(string name, uint value){
            KeyValuePair<string, uint> newPair = new KeyValuePair<string, uint>(name, value);

            if(!attributes.Contains(newPair))
                attributes.Add(newPair);            
        }
    }
}
