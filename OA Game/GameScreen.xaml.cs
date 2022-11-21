using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        private int level_id;
        public GameScreen(int level_id = 1)
        {
            InitializeComponent();

            
            this.level_id = level_id;
            

            string file_name = $"Level" + 1 + ".tmx";

            string mapdirectory = Assets.GetPath("Level_Panda");

            Map level = new Map(file_name, mapdirectory, new int[] {1,2,3,4,13,14,15,26,27,28 }, new int[] {84,78,91 }, new int[] {104,105,106,107,108,109,117,118,76});

            Image MapImage = level.RenderTiles();

            map.Children.Add(MapImage);
            Canvas.SetTop(MapImage, 0);
            Canvas.SetLeft(MapImage, 0);
           


        }

        public static void Set_CanvasBackground(int level_id)
        {
            ImageBrush background = new ImageBrush();
            string file_name = "Background" + level_id;
            background.ImageSource = new BitmapImage(Assets.GetUri("Platzhalter"));
            

        }


    }

    
    
    
}
