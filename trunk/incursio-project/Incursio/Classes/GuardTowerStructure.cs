using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Classes
{
  public class GuardTowerStructure : Structure
    {
        public GuardTowerStructure() : base(){
            this.setType(State.EntityName.GuardTower);
        }
    }
}
