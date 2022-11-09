using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Saving
    {
        private string path;
        int zeilencounter = 0;

        public Saving(string path) //Konstruktor
        {
            this.path = path;
            StreamWriter stw = new StreamWriter(path);
            StreamReader str = new StreamReader(path);
        }


        public static void Save(int level, string path) //Methode zum Speichern
        {
            StreamWriter stw = new StreamWriter(path);
            StreamReader str = new StreamReader(path);

            bool saved = true;

            for (int i = 1; !str.EndOfStream; i++)
            {

                if (saved)
                {
                    stw.WriteLine("1");
                }
                if (!saved)
                {
                    stw.WriteLine("0");
                }
            }

            str.Close();
            stw.Close();
        }

        //zurückgeben, ob level bereits gespeichert wurde
        void AlreadySaved(int level, string path) 
        {
            StreamWriter stw = new StreamWriter(path);
            StreamReader str = new StreamReader(path);

            string zeile = str.ReadLine();

            zeilencounter += 1; //zeilencounter ist äquivalent zu leveln: zeile 1, level 1; zeile 2, level 2 etc.

            if (zeilencounter == level && zeile == "1")
            {
                Console.WriteLine("Level wurde bereits gespeichert!");
            }
            else
            {
                Console.WriteLine("Kein Spielfortschritt in diesem Level festgehalten!");
            }
        }

    }
}
