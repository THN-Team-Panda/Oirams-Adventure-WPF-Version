using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameEngine;

namespace OA_Game
{
    /// <summary>
    /// Interaktionslogik für GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        Player player;
        Map level;
        ViewPort vp;

        public GameScreen()
        {
            InitializeComponent();
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
