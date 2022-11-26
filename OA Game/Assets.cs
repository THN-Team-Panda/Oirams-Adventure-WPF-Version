using System;

namespace OA_Game
{
    /// <summary>
    /// Helper class to handle the assets
    /// </summary>
    public static class Assets
    {
        /// <summary>
        /// Assets sub dir folder
        /// </summary>
        private const string Dir = "Assets";

        /// <summary>
        /// Helper method to get a uri object of a given absolute asset path
        /// Note: Use this without /Assets/
        /// </summary>
        /// <param name="asset">Absolute assets path</param>
        /// <returns>New uri object with the correct schema</returns>
        public static Uri GetUri(string asset) => new($"pack://siteoforigin:,,,/{Dir}/{asset}");

        /// <summary>
        /// Helper method to the the absolute path of an asset
        /// Note: Use this without /Assets/
        /// </summary>
        /// <param name="asset">Absolute assets path</param>
        /// <returns>Full asset path</returns>
        public static string GetPath(string asset) => $"{AppDomain.CurrentDomain.BaseDirectory}{Dir}/{asset}";
        
    }
}
