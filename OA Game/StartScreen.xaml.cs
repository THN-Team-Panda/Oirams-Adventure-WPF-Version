using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OA_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        public StartScreen()
        {
            InitializeComponent();

            /*GameScreen gs = new GameScreen(1);

            gs.Show();
            */
        }

        private bool button1WasClicked = false;
        private void Level1_Click(object sender, RoutedEventArgs e)
        {
            //switch from Startscreen to GameScreen

            button1WasClicked = true;
        }

        private bool button2WasClicked = false;
        private void Level2_Click(object sender, RoutedEventArgs e)
        {
            //switch from Startscreen to GameScreen

            button2WasClicked = true;
        }

        private bool button3WasClicked = false;
        private void Level3_Click(object sender, RoutedEventArgs e)
        {
            //switch from Startscreen to GameScreen

            button3WasClicked = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartLevel_Click(object sender, RoutedEventArgs e)
        {
            int[] level_id = {1, 2, 3};

            List<int> id_list = level_id.ToList(); //ToList macht level_id Array zu Liste

            //the following if-else cases enable buttons when the previous button is finished

            if (SavingTest.AlreadySaved(1)) // Level 2 will be available to play after Level 1 has been saved (after player has finished level 1)
            {
                Level2.IsEnabled = true;
            }          

            if(SavingTest.AlreadySaved(2)) // Level 3 will be available to play after Level 2 has been saved (after player has finished level 2)
            {
                Level3.IsEnabled = true;
            }

            if (button1WasClicked)
            {
                //commit Levelid to Gamescreen
            }

            if(button2WasClicked)
            {
                 //commit Levelid to Gamescreen
            }

            if (button3WasClicked)
            {
                 //commit Levelid to Gamescreen
            }
        }
    }
}
