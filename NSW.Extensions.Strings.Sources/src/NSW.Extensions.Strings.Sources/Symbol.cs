namespace NSW.Extensions.Internal
{
    /// <summary>
    /// Web special symbols
    /// </summary>
    internal static class Symbol
    {
        /// <summary>
        /// No-break space
        /// </summary>
        public static string Nbsp => GetSymbol(160);

        /// <summary>
        /// &#169;
        /// </summary>
        public static string Copyright => GetSymbol(169);

        /// <summary>
        /// &#174;
        /// </summary>
        public static string Registered => GetSymbol(174);

        /// <summary>
        /// &#8482;
        /// </summary>
        public static string TradeMark => GetSymbol(8482);

        /// <summary>
        /// &#8226;
        /// </summary>
        public static string Bullet => GetSymbol(8226);

        /// <summary>
        /// &#8227;
        /// </summary>
        public static string TriangularBullet => GetSymbol(8227);

        /// <summary>
        /// &#8259;
        /// </summary>
        public static string HyphenBullet => GetSymbol(8259);

        /// <summary>
        /// &#8251;
        /// </summary>
        public static string ReferenceMark => GetSymbol(8251);

        /// <summary>
        /// &#8230;
        /// </summary>
        public static string Ellipsis => GetSymbol(8230);

        /// <summary>
        /// &#170;
        /// </summary>
        public static string Feminine => GetSymbol(170);

        /// <summary>
        /// &#167;
        /// </summary>
        public static string Sect => GetSymbol(167);

        /// <summary>
        /// &#10003;
        /// </summary>
        public static string Check => GetSymbol(10003);

        /// <summary>
        /// &#10004;
        /// </summary>
        public static string HeavyCheck => GetSymbol(10004);

        /// <summary>
        /// &#10005;
        /// </summary>
        public static string Ballot => GetSymbol(10005);

        /// <summary>
        /// &#10006;
        /// </summary>
        public static string HeavyBallot => GetSymbol(10006);

        /// <summary>
        /// &#162;
        /// </summary>
        public static string Cent => GetSymbol(162);

        /// <summary>
        /// &#36;
        /// </summary>
        public static string Dollar => GetSymbol(36);

        /// <summary>
        /// &#163;
        /// </summary>
        public static string Pound => GetSymbol(163);

        /// <summary>
        /// &#165;
        /// </summary>
        public static string Yen => GetSymbol(165);

        /// <summary>
        /// &#8364;
        /// </summary>
        public static string Euro => GetSymbol(8364);

        /// <summary>
        /// Return specific symbol by Unicode code point
        /// </summary>
        /// <param name="symbolCode">Unicode code point</param>
        /// <returns>Result symbol</returns>
        public static string GetSymbol(int symbolCode) => ((char)symbolCode).ToString();
    }
}
