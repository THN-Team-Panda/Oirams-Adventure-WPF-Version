using System;
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

            #region BitmapImage:
            //Player Attack Cap
            BitmapImage playerAttackCap1 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_1.png"));
            BitmapImage playerAttackCap2 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_2.png"));
            BitmapImage playerAttackCap3 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_3.png"));
            BitmapImage playerAttackCap4 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_4.png"));
            BitmapImage playerAttackCap5 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_5.png"));
            //Player Attack Normal
            BitmapImage playerAttack1 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_1.png"));
            BitmapImage playerAttack2 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_2.png"));
            BitmapImage playerAttack3 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_3.png"));
            BitmapImage playerAttack4 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_4.png"));
            BitmapImage playerAttack5 = new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_5.png"));
            //Player Damage
            BitmapImage playerDamage1 = new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_1.png"));
            BitmapImage playerDamage2 = new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_2.png"));
            BitmapImage playerDamage3 = new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_3.png"));
            BitmapImage playerDamage4 = new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_4.png"));
            BitmapImage playerDamage5 = new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_5.png"));
            BitmapImage playerDamage6 = new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_6.png"));
            BitmapImage playerDamage7 = new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_7.png"));
            //PLayer Dying Normal
            BitmapImage playerDying1 = new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_1.png"));
            BitmapImage playerDying2 = new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_2.png"));
            BitmapImage playerDying3 = new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_3.png"));
            BitmapImage playerDying4 = new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_4.png"));
            BitmapImage playerDying5 = new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_5.png"));
            BitmapImage playerDying6 = new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_6.png"));
            BitmapImage playerDying7 = new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_7.png"));
            //Player Movement Cap
            BitmapImage playerCapJumping = new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Jumping.png"));
            BitmapImage playerCapStanding = new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Standing.png"));
            BitmapImage playerCap1 = new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap1.png"));
            BitmapImage playerCap2 = new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap2.png"));
            BitmapImage playerCap3 = new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap3.png"));
            //PLayer Movement Normal
            BitmapImage playerJumping = new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Jumping.png"));
            BitmapImage playerStanding = new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Standing.png"));
            BitmapImage player1 = new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player1.png"));
            BitmapImage player2 = new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player2.png"));
            BitmapImage player3 = new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player3.png"));
            #endregion BitmapImage
            
            ImageSource[] playerStandingAry = new ImageSource[] { playerStanding };
            ImageSource[] playerCapStandingAry = new ImageSource[] { playerCapStanding };
            ImageSource[] playerMoveAry = new ImageSource[] { player1, player2, player3 };
            ImageSource[] playerCapMoveAry = new ImageSource[] { playerCap1, playerCap2, playerCap3 };
            ImageSource[] playerJumpAry = new ImageSource[] { playerJumping };
            ImageSource[] playerCapJumpAry = new ImageSource[] { playerCapJumping };
            ImageSource[] playerAttackAry = new ImageSource[] { playerAttack1, playerAttack2, playerAttack3, playerAttack4, playerAttack5 };
            ImageSource[] playerCapAttackAry = new ImageSource[] { playerAttackCap1, playerAttackCap2, playerAttackCap3, playerAttackCap4, playerAttackCap5 };
            ImageSource[] playerDamageAry = new ImageSource[] { playerDamage1, playerDamage2, playerDamage3, playerDamage4, playerDamage5, playerDamage6, playerDamage7 };
            ImageSource[] playerDyingAry = new ImageSource[] { playerDying1,playerDying2,playerDying3,playerDying4,playerDying5,playerDying6,playerDying7 };

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
        }

        /// <summary>
        /// Check if Player is dead or in finish.
        /// </summary>
        private void GameOver()
        {
            if (player.Position.X > level.EndPoint.X)
            {
                Console.WriteLine("Im Ziel");
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

        public void CheckCollisionWithMovingObjects()
        {
            foreach (DrawableObject obj in level.SpawnedObjects)
            {
                Console.WriteLine(Physics.CheckCollisionBetweenGameObjects(player, obj));
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
