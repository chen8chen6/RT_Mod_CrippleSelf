using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace CrippleSelf
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool IsToApply_BrokenRibs = true;
        public bool IsToApply_Concussion = true;
        public bool IsToApply_CrippledArm_Left = false;
        public bool IsToApply_CrippledArm_Right = false;
        public bool IsToApply_CrippledLeg = true;
        public bool IsToApply_NeurogenicShock = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
