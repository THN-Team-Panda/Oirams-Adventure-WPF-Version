using System;
using System.Collections.Generic;
using System.Linq;
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

            static Image Map_Loading(int level_id)
            {
                string file_name = $"Level" + level_id + "_tmx";
                int[] groundTiles = { 0, 1, 2, 3 };
                Map level = new Map(file_name, "", groundTiles, groundTiles, groundTiles);

                Image MapImage = level.RenderTiles();
                return MapImage;
                

            }

            map.Children.Add(Map_Loading(3));
            Canvas.SetTop(Map_Loading(3), 0);
            Canvas.SetLeft(Map_Loading(3), 0);
            

        }

        


    }

    

    
    
}
