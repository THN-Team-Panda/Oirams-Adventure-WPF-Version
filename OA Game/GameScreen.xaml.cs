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
using OA_Game.Items;
using System.Security.Cryptography.X509Certificates;

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

            // init Player
            player = new Player(32, 32, new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Standing.png")));
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
        /// check if the player gets damage from touching a obstacle
        /// </summary>
        private void MovePlayer()
        {
            player.Velocity = player.Velocity with { X = player.Velocity.X * .9 };
            player.Velocity += Physics.Gravity;
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(level, player);
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
            else if (player.Position.X > level.EndPoint.X)
            {
                Saving save = new Saving(Preferences.GameDataPath);
                save.Save(level_id);

                gameLoop.Stop();

                Close();
            }

            ////if Player falls out of map
            else if (player.Position.Y >= level.MapHeight)
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
            foreach (DrawableObject obj in level.SpawnedObjects)
            {
                if (Physics.CheckCollisionBetweenGameObjects(player, obj))
                {

                    if (obj is Items.Items)
                    {
                        if (player.Collect((OA_Game.Items.Items)obj))
                        {
                            obj.ObjectIsTrash = true;
                        }
                    }

                    if (obj is Enemies.Enemie)
                    {
                        if (player.GetDamage((Enemies.Enemie)obj))
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

                        else if (player.GetDamage((Enemies.Enemie)obj) == false)
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
            level.SpawnedObjects.Add(newObject);
            map.Children.Add(newObject.Rectangle);
        }

    }
}
