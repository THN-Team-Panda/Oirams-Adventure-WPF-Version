using GameEngine;
using System.Windows;
using System.Windows.Media.Imaging;

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

            LevelView.Height = Preferences.ViewHeight;
            LevelView.Width = Preferences.ViewWidth;

            Backscreen.Source = new BitmapImage(Assets.GetUri("Images/StartScreen/Background.jpg"));

            EnableButton();
        }


        private void OpenLevel_Click(object sender, RoutedEventArgs e)
        {
            int clicklevel = 0;

            if (sender.Equals(Level1))
            {
                clicklevel = 1;
            }

            else if (sender.Equals(Level2))
            {
                clicklevel = 2;
            }

            else if (sender.Equals(Level3))
            {
                clicklevel = 3;
            }

            OpenLeveL(clicklevel);

        }

        public void OpenLeveL(int clicklevel)
        {
            GameScreen openscreen = new GameScreen(clicklevel);

            openscreen.Owner = this; //the popup-window is a child of the current screen

            openscreen.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            openscreen.ResizeMode = ResizeMode.NoResize;

            Hide();

            openscreen.ShowDialog(); //show the gamescreen; dialog is a popup-window

            EnableButton();

            Show();
        }

        public void EnableButton()
        {
            Saving saving = new Saving(Preferences.GameDataPath);


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

