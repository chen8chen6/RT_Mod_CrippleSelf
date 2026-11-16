namespace CrippleSelf
{
    internal class AssetID
    {
        public static class Trauma
        {
            public const string BROKEN_RIBS         = "d51b30ccb3d447b28c77b7057a5ccfa0";   //BrokenRibs_Trauma
            public const string CONCUSSION          = "30820d5aef9b4251af7579722dc95307";   //Concussion_Trauma
            public const string CRIPPLED_ARM_LEFT   = "e026d82067804bca899de79f0650c6b6";   //CrippledArm_Left_Trauma
            public const string CRIPPLED_ARM_RIGHT  = "b6a0edb0040f450a8a0c85c1300d6548";   //CrippledArm_Right_Trauma
            public const string CRIPPLED_LEG        = "6338756ef76148369f50bc5d6d48ac4a";   //CrippledLeg_Trauma
            public const string NEUROGENIC_SHOCK    = "30b20c0ee9b94aa09af18293c6e1222e";   //NerveDamage_Trauma
            public const string TRAUMA_RANK         = "8c776f738cb2482ba003f93b3af9f506";   //TraumaBuff
        };

        public static readonly string[] ALL_TRAUMAS =
        {
            Trauma.BROKEN_RIBS,
            Trauma.CONCUSSION,
            Trauma.CRIPPLED_ARM_LEFT,
            Trauma.CRIPPLED_ARM_RIGHT,
            Trauma.CRIPPLED_LEG,
            Trauma.NEUROGENIC_SHOCK
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
