using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEngine;
using GameEngine.GameObjects;
using OA_Game.AnimatedObjects;
using OA_Game.AnimatedObjects.Items;
using OA_Game.AnimatedObjects.Enemies;
using OA_Game.AnimatedObjects.Bullets;

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
        /// Stop the time in game
        /// </summary>
        public Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// The GameScreen is the main game window.
        /// The Constructor loads all nessesary objects!
        /// </summary>
        /// <param name="levelId"></param>
        public GameScreen(int levelId)
        {
            this.levelId = levelId;
            InitializeComponent();

           
            //Init the map
             
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


            // Init the Player
            
            player = new Player(32, 32, new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Standing.png")));
            mapCanvas.Children.Add(player.Rectangle); // a x to the canvas
            player.Position = (Vector)map.StartPoint;


            //Init the Camera
            
            viewPort.Focus();

            // Set the height/width from the preferences
            viewPort.Height = Preferences.ViewHeight;
            viewPort.Width = Preferences.ViewWidth;

            // Init the camera at player start position
            camera = new ViewPort(viewPort, mapCanvas, (Point)player.Position);

            // Init StatusBar Icons
            
            StatusBarClockIcon.Fill = new ImageBrush(new BitmapImage(Assets.GetUri("Images/Clock/Clock_1.png")));
            StatusBarHatIcon.Fill = new ImageBrush(new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png")));
            StatusBarAmmoIcon.Fill = new ImageBrush(new BitmapImage(Assets.GetUri("Images/Note/Note_big.png")));


            // Start the stopwatch
             
            stopwatch.Start();

            //Init the Game Loop Dispatcher
             
            gameLoop.Events += InputKeyboard;
            gameLoop.Events += UpdateCamera;
            gameLoop.Events += MovePlayer;
            gameLoop.Events += SpawnObjects;
            gameLoop.Events += GameOver;
            gameLoop.Events += CheckCollisionWithMovingObjects;
            gameLoop.Events += MoveInteractableObjects;
            gameLoop.Events += CollectGarbage;
            gameLoop.Events += UpdateStatusBar;
            gameLoop.Start();
        }

        /// <summary>
        /// Move the Player and Physics Stuff 
        /// check if the player gets damage from touching a obstacle
        /// </summary>
        private void MovePlayer()
        {
            player.Move(map);

            // Bugfix: https://git.informatik.fh-nuernberg.de/team-panda/oa-game/-/issues/102
            // Make sure that the player is unable to leave the viewPort Area
            if (player.Position.X < -camera.CurrentAngelHorizontal)
                player.Position = new Vector(-camera.CurrentAngelHorizontal, player.Position.Y);
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

                save.SaveLevel(levelId,new TimeSpan(stopwatch.ElapsedTicks));

                gameLoop.Stop();

                Close();
            }

            // if Player falls out of map
            else if (player.Position.Y >= map.MapHeight)
            {
                gameLoop.Stop();

                Close();
            }
        }

        /// <summary>
        /// Move all objects
        /// </summary>
        private void MoveInteractableObjects()
        {
            foreach (AnimatedObject obj in map.SpawnedObjects)
                if (obj is IInteractable moveable)
                    moveable.Move(map);
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
            if (Keyboard.IsKeyDown(Key.Space) || Keyboard.IsKeyDown(Key.E))
            {
                player.Shoot(map);
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
                    if (obj is Item item)
                        player.Collect(item);

                    if (obj is Enemy enemy)
                        ((IInteractable)enemy).Attack(player);
                }
            }
        }

        /// <summary>
        /// Update ViewPort to the current Position of Player.
        /// </summary>
        private void UpdateCamera() => camera.SmartCamera((Point)player.Position);

        /// <summary>
        /// Delete all collected items, dead Enemies or shot notes.
        /// </summary>
        private void CollectGarbage()
        {
            for (int i = map.SpawnedObjects.Count - 1; i >= 0; i--)
            {
                if (map.SpawnedObjects[i].ObjectIsTrash == true)
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
            AnimatedObject newObject = toSpawn.ClassName switch
            {
                "Enemy" => toSpawn.Name switch
                {
                    "Skeleton" => new Skeleton(32, 32, new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_1.png"))),
                    "FliegeVieh" => new FliegeVieh(32, 32, new BitmapImage(Assets.GetUri("Images/FliegeVieh/FliegeVieh_1.png"))),
                    "KonkeyDong" => new KonkeyDong(32, 32, new BitmapImage(Assets.GetUri("Images/KonkeyDong/Movement/KonkeyDong.png")), map, toSpawn.Position),
                    "Boombox" => new Boombox(20, 32, new BitmapImage(Assets.GetUri("Images/KonkeyDong/Boombox/Boombox_1.png"))),
                    _ => throw new ArgumentException("Enemy Not Known")

                },
                "Item" => toSpawn.Name switch
                {
                    "Hat" => new Hat(32, 32, new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png"))),
                    "Note" => new Note(32, 32, new BitmapImage(Assets.GetUri("Images/Note/Note_1.png"))),
                    _ => throw new ArgumentException("Item Not Known")

                },
                "Bullet" => toSpawn.Name switch
                {
                    "Tone" => new Tone(16, 16, new BitmapImage(Assets.GetUri("Images/Note/Note_1.png")), player.DirectionLeft),
                    "Egg" => throw new NotImplementedException(),
                    _ => throw new ArgumentException("Item Not Known")
                },

                _ => throw new ArgumentException("Class Not Known"),

            };
            newObject.Position = toSpawn.Position;
            map.SpawnedObjects.Add(newObject);
            mapCanvas.Children.Add(newObject.Rectangle);
        }

        /// <summary>
        /// Apply the latest status bar stats
        /// </summary>
        private void UpdateStatusBar()
        {
            StatusBarHatLabel.Content = $"{(player.HasHat ? "1" : "0")}/1";
            StatusBarAmmoLabel.Content = $"{player.Munition}/{Player.MaxMunition}";
            StatusBarClockLabel.Content = $"{stopwatch.Elapsed.Minutes:00}:{stopwatch.Elapsed.Seconds:00}";
        }
    }
}
