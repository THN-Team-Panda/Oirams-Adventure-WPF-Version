using System;
using System.Collections.Generic;
using System.Linq;
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
        public GameScreen()
        {
            InitializeComponent();

            

            

            map.Children.Add(Map_Generator.Map_Loading(3));
            Canvas.SetTop(Map_Generator.Map_Loading(3), 0);
            Canvas.SetLeft(Map_Generator.Map_Loading(3), 0);           
            

        }


    }

    public class Map_Generator 
    {

        public int level_id = 1;

        public static Image Map_Loading(int level_id)
        {
            string file_name = $"Level" + level_id + ".tmx";
            int[] groundTiles = { 0, 1, 2, 3 };
            Map level = new Map(file_name, "C:\\Users\\Vincent\\source\\repos\\level-design\\Level_Panda",new int[] { 2 }, new int[] { 3 }, new int[] { 4 });

            Image MapImage = level.RenderTiles();
            return MapImage;


        }

        public static ImageBrush Set_CanvasBackground(int level_id)
        {
            ImageBrush background = new ImageBrush();
            string file_name = "Background" + level_id;
            background.ImageSource = new BitmapImage(new Uri(background_path));
            return background;

        }

    }
    
    
}
