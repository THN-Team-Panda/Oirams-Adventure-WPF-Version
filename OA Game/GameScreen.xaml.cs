using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEngine;
using GameEngine.GameObjects;
using OA_Game.Enemies;
using OA_Game.Items;

namespace OA_Game
{
    /// <summary>
    /// Logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        /// <summary>
        /// Inherit the Player Object
        /// </summary>
        private readonly Player player;

        /// <summary>
        /// Inherit the Map Object
        /// </summary>
        private readonly Map map;

        /// <summary>
        /// Inherit the ViewPort Object
        /// </summary>
        private readonly ViewPort camera;

        /// <summary>
        /// Inherit the LoopDispatcher Object
        /// </summary>
        private readonly LoopDispatcher gameLoop = new(TimeSpan.FromMilliseconds(10));

        /// <summary>
        /// Level id if the current Level
        /// </summary>
        public int levelId;

        /// <summary>
        /// The GameScreen is the main game window.
        /// The Constructor loads all nessesary objects!
        /// </summary>
        /// <param name="levelId"></param>
        public GameScreen(int levelId)
        {
            this.levelId = levelId;
            InitializeComponent();

            /**
             * Init the map
             * Note: Map means the map obj, not the canvas map
             */
            map = new Map($"Level{levelId}.tmx", Assets.GetPath("Level_Panda"), Preferences.MapGroundTileIds, Preferences.MapObstacleTileIds);

            //render tiles and save image of tilemap in x
            Image tileMapImage = map.RenderTiles(); 

            mapCanvas.Children.Add(tileMapImage);

            Canvas.SetLeft(tileMapImage, 0); 
            Canvas.SetTop(tileMapImage, 0);
            Panel.SetZIndex(tileMapImage, 1);

            // Apply the map width/height to the canvas
            mapCanvas.Width = map.MapWidth;
            mapCanvas.Height = map.MapHeight;

            // Set the background image
            mapCanvas.Children.Add(new Image()
            {
                Source = map.BackgroundImage,
                Height = map.MapHeight,
                Width = map.MapWidth,
                Stretch = Stretch.Fill,
            });


            /**
             * Init the Player
             */
            player = new Player(32, 32, new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Standing.png")));
            mapCanvas.Children.Add(player.Rectangle); // a x to the canvas
            player.Position = new Vector(100, 100);


            /**
             * Init the Camera
             */
            viewPort.Focus();

            // Set the height/width from the preferences
            viewPort.Height = Preferences.ViewHeight;
            viewPort.Width = Preferences.ViewWidth;

            // Init the camera at player start position
            camera = new ViewPort(viewPort, mapCanvas, (Point)player.Position);


            /**
             * Init the Game Loop Dispatcher
             */
            gameLoop.Events += InputKeyboard;
            gameLoop.Events += UpdateCamera;
            gameLoop.Events += MovePlayer;
            gameLoop.Events += SpawnObjects;
            gameLoop.Events += GameOver;
            gameLoop.Events += CheckCollisionWithMovingObjects;
            gameLoop.Events += CollectGarbage;
            gameLoop.Start();
        }

        /// <summary>
        /// Move the Player and Physics Stuff 
        /// check if the player gets damage from touching a obstacle
        /// </summary>
        private void MovePlayer()
        {
            player.Velocity = player.Velocity with { X = player.Velocity.X * .9 };
            player.Velocity += Physics.Gravity;
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, player);
            if (collidedWithWhat.Contains(TileTypes.Obstacle))
            {
                if (player.GetDamage())
                {
                    if (player.DirectionLeft)
                    {
                        player.PlaySequence("dying", true, false);
                    }
                    else if (!player.DirectionLeft)
                    {
                        player.PlaySequence("dying", false, false);
                    }

                    player.ObjectIsTrash = true;
                }
                else if (!player.GetDamage())
                {
                    if (player.DirectionLeft)
                    {
                        player.PlaySequenceAsync("damage", true, true);
                    }
                    else if (!player.DirectionLeft)
                    {
                        player.PlaySequenceAsync("damage", false, true);
                    }
                }
            }

            if (collidedWithWhat[0] == TileTypes.Ground) player.CanJump = true;
            else player.CanJump = false;

            player.Position += player.Velocity;

            if (player.HasHat)
            {
                if (!player.CanJump)
                {
                    player.PlayPlayerSpriteMovement("jumpCap");
                }
                player.PlayPlayerSpriteMovement("moveCap");
            }
            else
            {
                if (!player.CanJump)
                {
                    player.PlayPlayerSpriteMovement("jump");
                }
                player.PlayPlayerSpriteMovement("move");
            }
        }
        /// <summary>
        /// Check if Player is dead or in finish or out of map
        /// </summary>
        private void GameOver()
        {

            //checks if Player is dead
            if (player.ObjectIsTrash)
            {
                gameLoop.Stop();

                Close();
            }

            //if Player reaches goal
            else if (player.Position.X > map.EndPoint.X)
            {
                Saving save = new Saving(Preferences.GameDataPath);

                save.Save(levelId);

                gameLoop.Stop();

                Close();
            }

            ////if Player falls out of map
            else if (player.Position.Y >= map.MapHeight)
            {
                gameLoop.Stop();

                Close();
            }

        }

        /// <summary>
        /// Check the user input to move the player or attack.
        /// </summary>
        private void InputKeyboard()
        {
            if (Keyboard.IsKeyDown(Key.W) && player.CanJump)
            {
                player.CanJump = false;
                player.Velocity = player.Velocity with { Y = -5 };
            }
            if (Keyboard.IsKeyDown(Key.A))
            {
                player.Velocity = player.Velocity with { X = -1.4 };
            }
            if (Keyboard.IsKeyDown(Key.D))
            {
                player.Velocity = player.Velocity with { X = 1.4 };
            }
        }
        /// <summary>
        /// Checks collision with items and enemies
        /// gets damage if collision with enemie
        /// and the animation is started
        /// </summary>
        public void CheckCollisionWithMovingObjects()
        {
            foreach (DrawableObject obj in map.SpawnedObjects)
            {
                if (Physics.CheckCollisionBetweenGameObjects(player, obj))
                {

                    if (obj is Items.Item)
                    {
                        if (player.Collect((Items.Item)obj))
                        {
                            obj.ObjectIsTrash = true;
                        }
                    }

                    if (obj is Enemies.Enemie)
                    {
                        if (player.GetDamage((Enemie)obj))
                        {
                            if (player.DirectionLeft)
                            {
                                player.PlaySequence("dying", true, false);
                            }
                            else if (!player.DirectionLeft)
                            {
                                player.PlaySequence("dying", false, false);
                            }
                            player.ObjectIsTrash = true;
                        }

                        else if (player.GetDamage((Enemie)obj) == false)
                        {
                            if (player.DirectionLeft)
                            {
                                player.PlaySequenceAsync("damage", true, true);
                            }
                            else if (!player.DirectionLeft)
                            {
                                player.PlaySequenceAsync("damage", false, true);
                            }
                            obj.ObjectIsTrash = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update ViewPort to the current Position of Player.
        /// </summary>
        private void UpdateCamera()
        {
            camera.SmartCamera((Point)player.Position);
        }

        /// <summary>
        /// Delete all collected items, dead Enemies or shot notes.
        /// </summary>
        private void CollectGarbage()
        {
            for (int i = map.SpawnedObjects.Count-1; i >=0; i--)
            {
                if(map.SpawnedObjects[i].ObjectIsTrash == true)
                {
                    mapCanvas.Children.Remove(map.SpawnedObjects[i].Rectangle);
                    map.SpawnedObjects.Remove(map.SpawnedObjects[i]); 
                }
                
            }          
            
        }

        /// <summary>
        /// Spawn Items and Ememies if player is in range.
        /// </summary>
        private void SpawnObjects()
        {
            NotSpawnedObject? toSpawn = map.SpawnObjectNearby(player.Position, Preferences.ViewWidth);
            if (toSpawn == null) return;
            var newObject = toSpawn.ClassName switch
            {
                "Enemy" => toSpawn.Name switch
                {
                    "Skeleton" => new Skeleton(32, 32, new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_1.png"))),
                    "FliegeVieh" => throw new NotImplementedException(),
                    "KonkeyDong" => throw new NotImplementedException(),
                    _ => throw new ArgumentException("Enemy Not Known")

                },
                "Item" => toSpawn.Name switch
                {
                    "Hat" => throw new NotImplementedException(),
                    "Note" => throw new NotImplementedException(),
                    _ => throw new ArgumentException("Item Not Known")

                },
                _ => throw new ArgumentException("Class Not Known"),

            };
            newObject.Position = toSpawn.Position;
            map.SpawnedObjects.Add(newObject);
            mapCanvas.Children.Add(newObject.Rectangle);
        }
    }
}
