using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Utils;
using Incursio.Classes;

namespace Incursio.Commands
{
    public class BuildCommand : BaseCommand
    {
        //TODO: IMPLEMENT BUILDING USING STRING
        public String buildClass;
        public BaseGameEntity toBeBuilt;

        public BuildCommand(BaseGameEntity toBeBuilt){
            this.toBeBuilt = toBeBuilt;
        }

        //NOTE: for building, subject will be a structure; probably a camp
        public override void execute(ref global::Incursio.Classes.BaseGameEntity subject)
        {
            //TODO: EXPAND ON THIS
            //Right now, camps are only building entities
            if (subject is CampStructure)
            {
                if (!(subject as Structure).isBuilding())
                {
                    (subject as CampStructure).build(toBeBuilt);
                    this.finishedExecution = true;
                }
                else
                {
                    //wait for building to stop
                    this.finishedExecution = false;
                }

            }
            else
                this.finishedExecution = true;
        }
    }
}
