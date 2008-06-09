using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    public class TerrainTextures : TextureSet
    {
        #region TEXTURE_DEF'NS
        public GameTexture grass1;
        //...

        public GameTexture shore1;
        //...

        public GameTexture openWater;
        public GameTexture shoreDown;
        public GameTexture shoreLeft;
        public GameTexture shoreRight;
        public GameTexture shoreUp;
        public GameTexture shoreLowerLeftCorner;
        public GameTexture shoreLowerRightCorner;
        public GameTexture shoreUpperLeftCorner;
        public GameTexture shoreUpperRightCorner;
        public GameTexture shoreOpenLowerLeftCorner;
        public GameTexture shoreOpenLowerRightCorner;
        public GameTexture shoreOpenUpperLeftCorner;
        public GameTexture shoreOpenUpperRightCorner;
        //...

        public GameTexture tree1;
        public GameTexture groupOfTrees;
        //...
        public GameTexture roadHorizontal;
        public GameTexture roadVertical;
        public GameTexture roadElbowUpRight;
        public GameTexture roadElbowUpLeft;
        public GameTexture roadElbowDownRight;
        public GameTexture roadElbowDownLeft;

        //...
        public GameTexture rockSmall;
        public GameTexture rockMedium;
        public GameTexture rockBig;
        public GameTexture dock;
        public GameTexture building1;
        public GameTexture building2;
        public GameTexture building3;
        public GameTexture buildingGroup;
        public GameTexture buildingGroupEndRight;
        public GameTexture buildingGroupEndLeft;
        #endregion

        public override void addTexture(string type, string name, int frameWidth, int frameHeight)
        {
            base.addTexture(type, name, frameWidth, frameHeight);

            switch(type){
                case "grass1": this.grass1= this.makeGameTexture(name, frameWidth, frameHeight); break;
                //...

                case "shore1": this.shore1= this.makeGameTexture(name, frameWidth, frameHeight); break;
                //...

                case "openWater": this.openWater= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreDown": this.shoreDown= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreLeft": this.shoreLeft= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreRight": this.shoreRight= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreUp": this.shoreUp= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreLowerLeftCorner": this.shoreLowerLeftCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreLowerRightCorner": this.shoreLowerRightCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreUpperLeftCorner": this.shoreUpperLeftCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreUpperRightCorner": this.shoreUpperRightCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreOpenLowerLeftCorner": this.shoreOpenLowerLeftCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreOpenLowerRightCorner": this.shoreOpenLowerRightCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreOpenUpperLeftCorner": this.shoreOpenUpperLeftCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "shoreOpenUpperRightCorner": this.shoreOpenUpperRightCorner= this.makeGameTexture(name, frameWidth, frameHeight); break;
                //...

                case "tree1": this.tree1= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "groupOfTrees": this.groupOfTrees= this.makeGameTexture(name, frameWidth, frameHeight); break;
                //...
                case "roadHorizontal": this.roadHorizontal= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "roadVertical": this.roadVertical= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "roadElbowUpRight": this.roadElbowUpRight= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "roadElbowUpLeft": this.roadElbowUpLeft= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "roadElbowDownRight": this.roadElbowDownRight= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "roadElbowDownLeft": this.roadElbowDownLeft= this.makeGameTexture(name, frameWidth, frameHeight); break;

                //...
                case "rockSmall": this.rockSmall= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "rockMedium": this.rockMedium= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "rockBig": this.rockBig= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "dock": this.dock= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "building1": this.building1= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "building2": this.building2= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "building3": this.building3= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "buildingGroup": this.buildingGroup= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "buildingGroupEndRight": this.buildingGroupEndRight= this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "buildingGroupEndLeft": this.buildingGroupEndLeft= this.makeGameTexture(name, frameWidth, frameHeight); break;
            }
        }
    }
}
