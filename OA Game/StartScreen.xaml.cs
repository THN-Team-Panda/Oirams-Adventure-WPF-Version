using System;
using GameEngine;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace OA_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartScreen
    {

        public StartScreen()
        {
            InitializeComponent();

            LevelView.Height = Preferences.ViewHeight;
            LevelView.Width = Preferences.ViewWidth;

            Backscreen.Source = new BitmapImage(Assets.GetUri("Images/Environment/back.png"));

            Level1.Background = new ImageBrush(new BitmapImage(Assets.GetUri("Images/StartScreen/Buttons/button_background_1.jpg")));
            Level2.Background = new ImageBrush(new BitmapImage(Assets.GetUri("Images/StartScreen/Buttons/button_background_2.jpg")));
            Level3.Background = new ImageBrush(new BitmapImage(Assets.GetUri("Images/StartScreen/Buttons/button_background_3.jpg")));

            EnableButton();
        }
        /// <summary>
        /// EventHandler for click button. If you click the button the methode OpenLevel is called and gets the level as int.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>

        private void OpenLevel_Click(object sender, RoutedEventArgs e)
        {

            Button senderButton = (Button)sender;
            OpenLevel(senderButton.Content switch
            {
                "Level 1" => 1,
                "Level 2" => 2,
                "Level 3" => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(sender), "You didn't click on a Button HOW??")
            });

        }
        /// <summary>
        /// Call the constructer if Gamescreen to start the level and hide the Startscreen.
        /// </summary>
        /// <param name="clickLevel"></param>
        private void OpenLevel(int clickLevel)
        {
            GameScreen gameScreen = new(clickLevel)
            {
                Owner = this, //the popup-window is a child of the current screen
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize
            };

            Hide();

            gameScreen.ShowDialog(); //show the game screen; dialog is a popup-window

            EnableButton();

            Show();
        }
        /// <summary>
        /// If the prelevel was finished succesfully the next level is possible to play and the button gets enabled.
        /// </summary>
        private void EnableButton()
        {
            Level1.IsEnabled = true;
            Level2.IsEnabled = false;
            Level3.IsEnabled = false;

            Saving saving = new(Preferences.GameDataPath);


            if (saving.AlreadySaved(1)) // Level 2 will be available to play after Level 1 has been saved (after player has finished level 1)
            {
                Level2.IsEnabled = true;
            }

            if (saving.AlreadySaved(2)) // Level 3 will be available to play after Level 2 has been saved (after player has finished level 2)
            {
                Level3.IsEnabled = true;
            }
        }

    }
}
