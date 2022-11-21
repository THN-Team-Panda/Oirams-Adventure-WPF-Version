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

        private void StartLevel_Click(object sender, RoutedEventArgs e)
        {
            //Punkt 2
            GameScreen openlvl = new GameScreen(Level1);
            

            //Punkt 3
            if(Savings.AlreadySaved())
            {
                //Level ist spielbar
            }
            else if(Savings.AlreadySaved())
            {
                //Level ist nicht spielbar
            }

            //Punkt 4
            List<T> id_list = new List<T>();

            //Werte in Liste hinzufügen
            id_list.Add();
            id_list.Add();
            id_list.Add();

            //einzelnen Wert aus Liste auslesen
            T wert1 = id_list[0];

            //alle Werte aus Liste ausgeben
            foreach(var wert in id_list)
            {
                Console.WriteLine(wert);
            }
        }
    }
}
