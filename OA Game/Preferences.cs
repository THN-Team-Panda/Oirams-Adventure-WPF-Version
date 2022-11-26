using System;

namespace OA_Game
{
    /// <summary>
    /// Static class with preferences for the game
    /// </summary>
    public static class Preferences
    {
        /// <summary>
        /// Size of all tiles in the game
        /// </summary>
        public const int TileSize = 16;

        /// <summary>
        /// Total height of the camera
        /// </summary>
        public const int ViewHeight = 30 * TileSize;

        /// <summary>
        /// Total width of the camera
        /// </summary>
        public const int ViewWidth = 40 * TileSize;

        /// <summary>
        /// Absolute path for placing the game data files such as saving files
        /// </summary>
        public static readonly string GameDataPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Map Obstacle ids from the tiled level design
        /// </summary>
        public static readonly int[] MapObstacleTileIds = (int[])Enum.GetValues(typeof(MapCollections.ObstacleTiles));

        /// <summary>
        /// Map Ground ids from the tiled level design
        /// </summary>
        public static readonly int[] MapGroundTileIds = (int[])Enum.GetValues(typeof(MapCollections.GroundTiles));
    }
}
