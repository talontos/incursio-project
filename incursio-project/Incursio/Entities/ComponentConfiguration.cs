using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities
{
    public class ComponentConfiguration
    {
        public string type;
        public List<KeyValuePair<string, object>> attributes = new List<KeyValuePair<string, object>>();

        public ComponentConfiguration(string type){
            this.type = type;
        }

        public void addAttribute(KeyValuePair<string, object> att){
            if (!attributes.Contains(att))
                attributes.Add(att);
        }
    }
}
