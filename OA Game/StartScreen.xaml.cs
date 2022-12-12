using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// File where GameSave is located
        /// </summary>
        private Saving save;

        public StartScreen()
        {
            InitializeComponent();

            LevelView.Height = Preferences.ViewHeight;
            LevelView.Width = Preferences.ViewWidth;

            Backscreen.Source = new BitmapImage(Assets.GetUri("Images/Environment/back.png"));
            save = new(Preferences.GameDataPath);


            CreateLevelButtons();
        }


        /// <summary>
        /// Generate Buttons 
        /// </summary>
        private void CreateLevelButtons()
        {
            // Clear all existing Buttons
            Levels.Children.RemoveRange(0, Levels.Children.Count);

            //read background files
            List<string> backgroundFiles =
                Directory.EnumerateFiles(Assets.GetPath("Images/StartScreen/Buttons"), "*.jpg").ToList();
            //read File names in Level Panda Directory 
            List<string> levelFiles = Directory.EnumerateFiles(Assets.GetPath("Level_Panda"), "*.tmx").ToList();
            for (int i = 0; i < levelFiles.Count; i++) // remove Path from filename
            {
                levelFiles[i] = Path.GetFileName(levelFiles[i]);
            }

            //get bitmap images
            BitmapImage[] brushes = new BitmapImage[backgroundFiles.Count];
            for (int i = 0; i < brushes.Length; i++)
            {
                brushes[i] = new BitmapImage(new Uri(backgroundFiles[i]));
            }

            //read highscores of level
            TimeSpan[] levelTimes = save.ReadLevels();
            for (int i = 0; i < levelFiles.Count; i++)
            {
                //check if level
                TimeSpan levelTime;
                try
                {
                    levelTime = levelTimes[i];
                }
                catch (IndexOutOfRangeException)
                {
                    levelTime = TimeSpan.Zero;
                }

                string levelName = levelFiles[i][..^4];
                //create button
                Button btn = new()
                {
                    Name = $"{levelName}",
                    Style = Resources["MenuButtons"] as Style,
                    Tag = (i + 1),
                    IsEnabled = (!(levelTime == TimeSpan.Zero) || i == levelTimes.Length),
                };
                //create stackpanel inside btn
                StackPanel btnStackPanel = new()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = btn.IsEnabled switch
                    {
                        false => new ImageBrush(new FormatConvertedBitmap(brushes[Math.Clamp(i, 0, brushes.Length)],
                            PixelFormats.Gray4, null, 0)),
                        true => new ImageBrush(brushes[Math.Clamp(i, 0, brushes.Length)]),
                    },
                    Width = btn.Width
                };
                //add level name label
                btnStackPanel.Children.Add(new Label()
                {
                    Margin = new Thickness(0, -10, 0, 0),
                    Content = $"{levelName}",
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Foreground = btn.IsEnabled == false ? new SolidColorBrush(Colors.Black) : btn.Foreground
                });
                //add level highscore label
                btnStackPanel.Children.Add(new Label()
                {
                    Margin = new Thickness(0, -13, 0, 0),
                    Content = $"{levelTime:g}",
                    FontSize = 12, HorizontalContentAlignment = HorizontalAlignment.Center,
                    Foreground = btn.IsEnabled == false ? new SolidColorBrush(Colors.Black) : btn.Foreground
                });

                btn.Content = btnStackPanel;
                //subscribe events
                btn.Click += OpenLevel_Click;
                btn.MouseEnter += Btn_GotFocus;
                btn.MouseLeave += Btn_MouseLeave;
                //add level
                Levels.Children.Add(btn);
            }
        }

        /// <summary>
        /// Function that gets triggered when mouse is leaving button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((Button)sender).Opacity = 1;
        }

        /// <summary>
        /// Function that is triggered when mouse hovers over button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_GotFocus(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Opacity = 0.75;
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
            OpenLevel((int)senderButton.Tag);
        }

        /// <summary>
        /// Call the constructor if Gamescreen to start the level and hide the Startscreen.
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

            CreateLevelButtons();

            Show();
        }
    }
}