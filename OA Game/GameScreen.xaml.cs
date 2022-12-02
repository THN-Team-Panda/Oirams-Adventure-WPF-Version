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
using OA_Game.Enemies;
using Vector = System.Windows.Vector;

namespace OA_Game
{
    /// <summary>
    /// Logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        private readonly Player player;
        private readonly Map level;
        private readonly ViewPort camera;
        private readonly LoopDispatcher gameLoop = new(TimeSpan.FromMilliseconds(10));
        public int level_id;
        public GameScreen(int level)
        {
            InitializeComponent();

            // Presetting for the view
            viewPort.Height = Preferences.ViewHeight;
            viewPort.Width = Preferences.ViewWidth;
            viewPort.Focus();


            //load Map
            this.level = new Map($"Level{level}.tmx", Assets.GetPath("Level_Panda"), Preferences.MapGroundTileIds, Preferences.MapObstacleTileIds); //create map
            Image tileMapImage = this.level.RenderTiles(); //render tiles and save image of tilemap in x
            map.Children.Add(tileMapImage); // a x to the canvas
            Canvas.SetLeft(tileMapImage, 0); // position x in 0,0
            Canvas.SetTop(tileMapImage, 0);
            Canvas.SetZIndex(tileMapImage, 1); //set x before bg in the z position

            this.map.Width = level.Width;
            this.map.Height = level.Height;

            // init Player
            player = new Player(32, 32, new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Standing.png")));
            map.Children.Add(player.Rectangle); // a x to the canvas
            player.Position = new Vector(100, 100);

            //Player.AddSequence("Idle",new PlayableSequence(new []{0,0}){Between = TimeSpan.FromMilliseconds(5)});
            //Player.PlaySequenceAsync("Idle");
            //init camera
            camera = new ViewPort(viewPort, map, (Point)player.Position);

            // Loopti Loop
            gameLoop.Events += InputKeyboard;
            gameLoop.Events += UpdateCamera;
            gameLoop.Events += MovePlayer;
            gameLoop.Events += SpawnObjects;
            gameLoop.Events += GameOver;
            gameLoop.Events += CheckCollisionWithMovingObjects;
            gameLoop.Start();
        }
        /// <summary>
        /// Move the Player and Physics Stuff
        /// </summary>
        private void MovePlayer()
        {
            player.Velocity = player.Velocity with { X = player.Velocity.X * .9 };
            player.Velocity += Physics.Gravity;
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(level, player);
            if (collidedWithWhat.Contains(TileTypes.Obstacle))
            {
                Console.WriteLine("TOT");
            }

            if (collidedWithWhat[0] == TileTypes.Ground) player.CanJump = true;
            else player.CanJump = false;
            //Console.WriteLine(whichSideTouched);

            player.Position += player.Velocity;

            if (player.HasHat) // Sprites WITH cap (wrong image nameing)
            {
                if (!player.CanJump)
                {
                    player.PlayPlayerSpriteMovement("jump");
                }
                player.PlayPlayerSpriteMovement("move");
            }
            else // Sprites WITHOUT cap (wrong image nameing)
            {
                if (!player.CanJump)
                {
                    player.PlayPlayerSpriteMovement("jumpCap");  
                }
                player.PlayPlayerSpriteMovement("moveCap");
            }
        }
        /// <summary>
        /// Check if Player is dead or in finish.
        /// </summary>
        private void GameOver()
        {
/*
            //checks if Player is dead
            if (player.ObjectIsTrash)
            {
                Close();
            }

            //if Player reaches goal
            else if (player.Position.X > level.EndPoint.X)
            {
                Saving save = new Saving(Preferences.GameDataPath);
                save.Save(level_id);

                Close();
            }

            //if Player falls out of map
            else if (player.Position.Y <= level.MapHeight)
            {
                Close();
            }
            if (player.ObjectIsTrash == true)
            {

            }
*/
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

        public void CheckCollisionWithMovingObjects()
        {
            foreach (DrawableObject obj in level.SpawnedObjects)
            {
                Console.WriteLine(Physics.CheckCollisionBetweenGameObjects(player, obj));
                if(Physics.CheckCollisionBetweenGameObjects(player, obj))
                {
                    if(player.HasHat == false)
                    {
                        
                        
                        player.PlaySequence("dying");
                        player.ObjectIsTrash = true;
                    }
                    else
                    {
                        player.HasHat = false;
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

        }
        /// <summary>
        /// Spawn Items and Ememies if player is in range.
        /// </summary>
        private void SpawnObjects()
        {
            NotSpawnedObject? toSpawn = level.SpawnObjectNearby(player.Position, Preferences.ViewWidth);
            if (toSpawn == null) return;
            var newObject = toSpawn.ClassName switch
            {
                "Enemy" => toSpawn.Name switch
                {
                    "Skeleton" => new Skeleton(16, 16, new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_1.png"))),
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
            level.SpawnedObjects.Add(newObject);
            map.Children.Add(newObject.Rectangle);
        }

        /// <summary>
        /// collect item if player is in range and has space in his inventory.
        /// </summary>
        private void CollectItem()
        {

        }
    }
}
