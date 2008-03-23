using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Campaign
{
    public class CampaignMap : MapBase
    {
        public State.CampaignLevel level;
        
        public override void initializeMap(/*PASS A FILE IN DEFINING MAP??*/){

        }

        /// <summary>
        /// This function should be overridden by separate levels to check
        /// win conditions.
        /// </summary>
        /// <returns>TODO: Some result representing the condition;
        /// ***OR*** void, and we'll just act on the conditions here</returns>
        public virtual Object inspectWinConditions(){

            return null;
        }

    }
}
