using System;

namespace OA_Game
{
    /// <summary>
    /// Helper class to handle the assets
    /// </summary>
    public static class Assets
    {
        /// <summary>
        /// Helper method to get a uri object of a given absolute asset path
        /// </summary>
        /// <param name="asset">Absolute assets path</param>
        /// <returns>New uri object with the correct schema</returns>
        public static Uri GetUri(string asset) => new Uri("pack://siteoforigin:,,," + asset);
    }
}
