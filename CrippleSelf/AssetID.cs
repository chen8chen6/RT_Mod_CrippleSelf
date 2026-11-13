using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrippleSelf
{
    internal class AssetID
    {
        public static class Trauma
        {
            public const string BROKEN_RIBS     = "d51b30ccb3d447b28c77b7057a5ccfa0";   //BrokenRibs_Trauma
            public const string TRAUMA_BUFF     = "8c776f738cb2482ba003f93b3af9f506";   //TraumaBuff
        };

        public static class Feature
        {
            public const string FACING_THE_END  = "2b2f1b1ed2784eb78b4d3a6f5a9f7df5";   //ReaperTalent4Feature
            public const string WOUNDED_BEAST   = "6ee56b90ea554830b83d2331bceeb3b8";   //DeathWorld_Talent3
        };

        public static class Ability
        {
            public const string MOD_CRIPPLE_SELF = "2A4C974E3D6642FCA59BD21CD498FA22";
        }

        public static class LocalizedStringKey
        {
            public const string CRIPPLE_SELF_NAME = "mod.crippleSelf.name";
            public const string CRIPPLE_SELF_DESC = "mod.crippleSelf.desc";
        }
    }
}
