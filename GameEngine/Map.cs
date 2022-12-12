using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEngine.Exceptions;
using GameEngine.GameObjects;
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
        private int[] GroundTileIds { get; }

        /// <summary>
        /// Ids of Tiles that represent obstacles
        /// </summary>
        private int[] ObstacleTileIds { get; }

        /// <summary>
        /// Position where the Map starts
        /// </summary>
        public Point StartPoint { get; private set; }
        /// <summary>
        /// position where the Map ends
        /// </summary>
        public Point EndPoint { get; private set; }
        /// <summary>
        /// List of all Spawned GameObjects in the Map
        /// </summary>
        public List<DrawableObject> SpawnedObjects { get; } = new();

        /// <summary>
        /// List of all GameObjects that are on the Map but not yet spawned
        /// </summary>
        private List<NotSpawnedObject> NotSpawnedObjects { get; set; } = new();

        private readonly TiledTileset tileset;

        /// <summary>
        /// Sets all Properties and creates the TileMap Array
        /// </summary>
        /// <param name="mapFileName">filename of the map.tmx file</param>
        /// <param name="mapDirectory">directory where the map files are stored</param>
        /// <param name="groundTilesIds">id of tiles that belong to the ground </param>
        /// <param name="obstacleTilesIds">id of tiles that belong to obstacles</param>
        /// <exception cref="CannotLoadFiles">occurs when an file could not be loaded</exception>
        public Map(string mapFileName, string mapDirectory, int[] groundTilesIds,
            int[] obstacleTilesIds)
        {
            TiledMap map;
            GroundTileIds = groundTilesIds;
            ObstacleTileIds = obstacleTilesIds;

            //load files
            try
            {
                map = new TiledMap($"{mapDirectory}/{mapFileName}");
                tileset = new TiledTileset($"{mapDirectory}/{map.Tilesets[0].source}");
                TileSetImg = new BitmapImage(new Uri($"{Path.GetFullPath(mapDirectory)}/{tileset.Image.source}"));
                //gets the layer which contains the backgroundImage, if there is no Background layer then null
                TiledLayer? backgroundLayer =
                    map.Layers.FirstOrDefault(l => l is { type: TiledLayerType.ImageLayer }, null);
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
            TileSize = tileset.TileWidth;
            TiledLayer layer = map.Layers.First(l =>
                    l.type == TiledLayerType.TileLayer); // get first layer of tile map that is a tile layer
            TileMapDataRaw = layer.data; //2 dimensional array of tile ids
            TileRows = layer.height;
            TileColumns = layer.width;
            MapWidth = TileColumns * TileSize; //Size of the Tilemap in pixel
            MapHeight = TileRows * TileSize;
            TileMap = new TileTypes[TileRows, TileColumns];

            CreateTileMapArray();


            TiledLayer objectLayer = map.Layers.First(l => l.type == TiledLayerType.ObjectLayer);
            GetObjectsFromObjectLayer(objectLayer);

        }

        /// <summary>
        /// Function to extracted all objects from the Tilemap
        /// </summary>
        /// <param name="layer">object Layer</param>
        private void GetObjectsFromObjectLayer(TiledLayer layer)
        {
            //get start and end of lvl
            var lvlStart = layer.objects.First(obj => obj.@class == "Player" && obj.name == "Start");
            var lvlEnd = layer.objects.First(obj => obj.@class == "Player" && obj.name == "End");
            StartPoint = new Point(lvlStart.x, lvlStart.y);
            EndPoint = new Point(lvlEnd.x, lvlEnd.y);

            // get other objects

            foreach (TiledObject obj in layer.objects)
            {
                if (obj.@class == "Player") continue;

                NotSpawnedObjects.Add(new NotSpawnedObject(obj.name, obj.@class, new Vector(obj.x, obj.y)));

            }                        
            // sort list by X coordinate
            NotSpawnedObjects = NotSpawnedObjects.OrderBy(obj => obj.Position.X).ToList();

        }

        /// <summary>
        /// Add an element to the NotSpawnedObjects list
        /// </summary>
        /// <param name="obj">element to add</param>
        public void AddNotSpawnedObject(NotSpawnedObject obj)
        {
            NotSpawnedObjects.Add(obj);
            NotSpawnedObjects = NotSpawnedObjects.OrderBy(obj => obj.Position.X).ToList();
        }

        /// <summary>
        /// Checks if an Object should Spawn and returns the Information of an object if it should spawn
        /// </summary>
        /// <param name="position">Position from where to check</param>
        /// <param name="maxDistance">Distance to the right at which point a Game object should Spawn</param>
        /// <returns></returns>
        public NotSpawnedObject? SpawnObjectNearby(Vector position, double maxDistance)
        {
            if (NotSpawnedObjects.Count == 0) return null;
            if (!(NotSpawnedObjects[0].Position.X - position.X < maxDistance)) return null;
            //get object and remove at from the List
            NotSpawnedObject objectToSpawn = NotSpawnedObjects[0];
            NotSpawnedObjects.RemoveAt(0);
            return objectToSpawn;

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
                int data = TileMapDataRaw[i] - 1;
                if (data == -1) continue;
                int posX = i % TileColumns;
                int posY = i / TileColumns;
                //calculate the position of the tile image in the tileset image
                int xInTileSetImage = data % tileset.Columns * TileSize;
                int yInTileSetImage = data / tileset.Columns * TileSize;
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
            DrawingVisual drawingVisual = new();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(drawingImageSource, new Rect(new Point(0, 0), new Size(TileColumns * TileSize, TileRows * TileSize)));
            drawingContext.Close();

            RenderTargetBitmap bmp = new(TileColumns * TileSize, TileRows * TileSize, 96, 96, PixelFormats.Pbgra32);
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
                else if (Array.IndexOf(ObstacleTileIds, data) > -1) TileMap[posY, posX] = TileTypes.Obstacle;
            }
        }
    }


}
