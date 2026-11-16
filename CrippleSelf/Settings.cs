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

        public bool[] GetIsToApply()
        {
            //Order must be same as AssetID.ALL_TRAUMAS
            return
            [
                IsToApply_BrokenRibs,
                IsToApply_Concussion,
                IsToApply_CrippledArm_Left,
                IsToApply_CrippledArm_Right,
                IsToApply_CrippledLeg,
                IsToApply_NeurogenicShock,
            ];
        }

        public int GetHash()
        {
            int res = 0;
            int mask = 1;
            var optionList = GetIsToApply();
            foreach (bool isToApply in optionList)
            {
                if (isToApply)
                    res += mask;
                mask *= 2;
            }
            return res;
        }
    }
}
