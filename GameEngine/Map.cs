using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEngine.Exceptions;
using TiledCS;

namespace GameEngine
{
    /// <summary>
    /// Types of possible Tiles
    /// </summary>
    public enum TileTypes
    {
        Void,
        Ground,
        Obstacle,
        EnemySpawn,
    }

    /// <summary>
    /// holds the TileMap data and renders the Map as an image
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Size of an Tile in pixel
        /// </summary>
        public int TileSize { get; }

        /// <summary>
        /// number of Tiles in the horizontal direction
        /// </summary>
        public int TileColumns { get; }

        /// <summary>
        /// number of Tiles in the vertical direction
        /// </summary>
        public int TileRows { get; }

        /// <summary>
        /// Raw Data Array with Tile Ids
        /// </summary>
        public int[] TileMapDataRaw { get; }

        /// <summary>
        /// Two Dimensional Array with TileTypes based on TilemapDataRaw
        /// </summary>
        public TileTypes[,] TileMap { get; }

        /// <summary>
        /// height of the Tilemap in pixel
        /// </summary>
        public int MapHeight { get; }

        /// <summary>
        /// width of the Tilemap in pixel
        /// </summary>
        public int MapWidth { get; }

        /// <summary>
        /// the Image of the Tileset that is being used
        /// </summary>
        public BitmapImage TileSetImg { get; }

        /// <summary>
        /// Image of the Background 
        /// </summary>
        public BitmapImage? BackgroundImage { get; }

        /// <summary>
        /// Ids of Tiles that are Ground
        /// </summary>
        public int[] GroundTileIds { get; }

        /// <summary>
        /// Ids of Tiles that represent the Enemy Position
        /// </summary>
        public int[] EnemyTileIds { get; }

        /// <summary>
        /// Ids of Tiles that represent obstacles
        /// </summary>
        public int[] ObstacleTileIds { get; }

        private readonly TiledMap _map;
        private readonly TiledTileset _tileset;

        /// <summary>
        /// Sets all Properties and creates the TileMap Array
        /// </summary>
        /// <param name="mapFileName">filename of the map.tmx file</param>
        /// <param name="mapDirectory">directory where the map files are stored</param>
        /// <param name="groundTilesIds">id of tiles that belong to the ground </param>
        /// <param name="enemyTilesIds">id of tiles that should spawn an enemy</param>
        /// <param name="obstacleTilesIds">id of tiles that belong to obstacles</param>
        /// <exception cref="CannotLoadFiles">occurs when an file could not be loaded</exception>
        public Map(string mapFileName, string mapDirectory, int[] groundTilesIds, int[] enemyTilesIds,
            int[] obstacleTilesIds)
        {
            GroundTileIds = groundTilesIds;
            EnemyTileIds = enemyTilesIds;
            ObstacleTileIds = obstacleTilesIds;

            //load files
            try
            {
                _map = new TiledMap($"{mapDirectory}/{mapFileName}");
                _tileset = new TiledTileset($"{mapDirectory}/{_map.Tilesets[0].source}");
                TileSetImg = new BitmapImage(new Uri($"{Path.GetFullPath(mapDirectory)}/{_tileset.Image.source}"));
                //gets the layer which contains the backgroundImage, if there is no Background layer then null
                TiledLayer? backgroundLayer =
                    _map.Layers.FirstOrDefault(l => l.type == TiledLayerType.ImageLayer, null);
                if (backgroundLayer != null)
                {
                    BackgroundImage =
                        new BitmapImage(new Uri($"{Path.GetFullPath(mapDirectory)}/{backgroundLayer.image.source}"));
                }
            }
            catch (FileNotFoundException e)
            {
                throw new CannotLoadFiles(e.Message);
            }
            catch (TiledException e)
            {
                throw new CannotLoadFiles(e.Message);
            }

            //set Properties
            TileSize = _tileset.TileWidth;
            TiledLayer layer = _map.Layers.First(l =>
                    l.type == TiledLayerType.TileLayer); // get first layer of tile map that is a tile layer
            TileMapDataRaw = layer.data; //2 dimensional array of tile ids
            TileRows = layer.height;
            TileColumns = layer.width;
            MapWidth = TileColumns * TileSize; //Size of the Tilemap in pixel
            MapHeight = TileRows * TileSize;
            TileMap = new TileTypes[TileRows, TileColumns];


            CreateTileMapArray();
        }

        /// <summary>
        /// Creates an Wpf Image Element of the Map
        /// </summary>
        /// <returns></returns>
        public Image RenderTiles()
        {
            DrawingGroup imageDrawings = new();

            for (int i = 0; i < TileMapDataRaw.Length; i++)
            {
                // -1 because tmx void is 0 instead of -1
                int data = TileMapDataRaw[i]-1;
                if (data == -1) continue;
                int posX = i % TileColumns;
                int posY = i / TileColumns;
                //calculate the position of the tile image in the tileset image
                int xInTileSetImage = data % _tileset.Columns * TileSize;
                int yInTileSetImage = data / _tileset.Columns * TileSize;
                //crop Tile out of Tileset Image
                CroppedBitmap tileImage = new(TileSetImg,
                    new Int32Rect(xInTileSetImage, yInTileSetImage, TileSize, TileSize));
                ImageDrawing tile = new()
                {
                    Rect = new Rect(posX * TileSize, posY * TileSize, TileSize, TileSize),
                    ImageSource = tileImage
                };
                imageDrawings.Children.Add(tile);
            }

            //merge all Tiles to one Image
            DrawingImage drawingImageSource = new(imageDrawings);

            // Freeze the DrawingImage for performance benefits.
            drawingImageSource.Freeze();

            //do not touch
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(drawingImageSource, new Rect(new Point(0, 0), new Size(TileColumns * TileSize, TileRows * TileSize)));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)TileColumns * TileSize, (int)TileRows * TileSize, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            


            Image finalImage = new()
            {
                Stretch = Stretch.None,
                Width = TileColumns * TileSize,
                Height = TileRows * TileSize,
                Source = bmp
            };
            return finalImage;
        }


        /// <summary>
        /// simplify the raw Tile ids in the Raw data and merge them into a handful of types
        /// </summary>
        private void CreateTileMapArray()
        {
            //convert raw data to 
            for (int i = 0; i < TileMapDataRaw.Length; i++)
            {
                int data = TileMapDataRaw[i];
                if (data == 0) continue;
                int posX = i % TileColumns;
                int posY = i / TileColumns;
                if (Array.IndexOf(GroundTileIds, data) > -1) TileMap[posY, posX] = TileTypes.Ground;
                else if (Array.IndexOf(EnemyTileIds, data) > -1) TileMap[posX, posY] = TileTypes.EnemySpawn;
                else if (Array.IndexOf(ObstacleTileIds, data) > -1) TileMap[posX, posY] = TileTypes.Obstacle;
            }
        }
    }
}
