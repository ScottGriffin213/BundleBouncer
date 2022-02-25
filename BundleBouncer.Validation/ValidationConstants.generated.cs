namespace BundleBouncer.Validation
{
    internal class ValidationConstants
    {

        /// <summary>
        /// Which minPlayer header values are permitted in AssetBundles
        /// </summary>
        public static readonly System.Collections.Generic.HashSet<string> ASSETBUNDLE_HEADER_ALLOWED_MIN_PLAYER_VERSIONS = new System.Collections.Generic.HashSet<string>()
        {
            "5.x.x",
        };

        /// <summary>
        /// Which file engine versions are permitted in AssetBundles
        /// </summary>
        public static readonly System.Collections.Generic.HashSet<string> ASSETBUNDLE_HEADER_ALLOWED_CUR_PLAYER_VERSIONS = new System.Collections.Generic.HashSet<string>()
        {
            "2017.4.15f1",
            "2017.4.28f1",
            "2017.4.39f1",
            "2018.4.12f1",
            "2018.4.14f1",
            "2018.4.17f1",
            "2018.4.20f1",
            "2018.4.9f1",
            "2019.4.29f1",
            "2019.4.30f1",
            "2019.4.31f1",
            "2019.4.31f1c1",
            "5.6.3p1",
        };
        
        public const uint MIN_PLAYER_VERSION_LENGTH_MIN = 5;
        public const uint MIN_PLAYER_VERSION_LENGTH_MAX = 5;
        public const uint CUR_PLAYER_VERSION_LENGTH_MIN = 7;
        public const uint CUR_PLAYER_VERSION_LENGTH_MAX = 13;
    }
}