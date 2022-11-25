using System.Windows;
using GameEngine;

namespace OA_Game
{
    /// <summary>
    /// Logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        private Player Player;
        private Map Level;
        private ViewPort Camera;

        public GameScreen(int level)
        {
            InitializeComponent();

            // Presetting for the view
            viewPort.Height = Preferences.ViewHeight;
            viewPort.Width = Preferences.ViewWidth;
            viewPort.Focus();


            // More Code comes here
            
        }

        /// <summary>
        /// Check if Player is dead or in finish.
        /// </summary>
        private void GameOver()
        {

        }
        /// <summary>
        /// Check the user input to move the player or attack.
        /// </summary>
        private void InputKeyboard()
        {

        }
        /// <summary>
        /// Update ViewPort to the current Position of Player.
        /// </summary>
        private void UpdateCamera()
        {
            Camera.SmartCamera((Point)Player.Position);
        }
        /// <summary>
        /// Delet all collected items, dead Enemies or shooted notes.
        /// </summary>
        private void CollectGarbage()
        {

        }
        /// <summary>
        /// Spawn Items if player is in range.
        /// </summary>
        private void SpawnItem()
        {

        }
        /// <summary>
        /// Spawn Enemies if player is in range.
        /// </summary>
        private void SpawnEnemies()
        {

        }
        /// <summary>
        /// collect item if player is in range and has space in his inventory.
        /// </summary>
        private void CollectItem()
        {

        }
    }
}
