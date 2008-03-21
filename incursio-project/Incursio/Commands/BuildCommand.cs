using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Utils;

namespace Incursio.Commands
{
    public class BuildCommand : BaseCommand
    {
        public String buildClass;

        //NOTE: for building, subject will be a structure; probably a camp (get player from subject)
        public override void execute(ref global::Incursio.Classes.BaseGameEntity subject)
        {
            //TODO: EXPAND ON THIS
            ObjectFactory.getInstance().create(buildClass, subject.owner);
        }
    }
}
