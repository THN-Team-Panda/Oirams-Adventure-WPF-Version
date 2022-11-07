using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Saving
    {
        private static string? Path; //Wert ist nullable
        int zeilencounter = 0;


        public Saving(string path)
        {
            Path = path;
        }
        StreamWriter stw = new StreamWriter(Path);
        StreamReader str = new StreamReader(Path);

        void Save(int level)
        {
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
        void AlreadySaved(int level)
        {
            string zeile = str.ReadLine();

            zeilencounter += 1;

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
