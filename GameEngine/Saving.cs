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
        public Saving(string path)
        {
            this.path = path;
        }

        public static async Task Save(int level, string path)
        {
            StreamWriter stw = new StreamWriter(path);
            string savingvar = "0";

            for (int j = 0; j <= level; j++) // j entspricht Koordinaten der Level (zeile 1 = level 1 usw.)
            {

                if (j != level)
                {
                    savingvar = "0";
                    stw.WriteLine(savingvar);
                    // diese Methode schreibt die Savingsvariable in die Datei des angegeben Pfades
                }

                else if (j == level) // && j.data == 1 => dafür Array-Ansatz
                {
                    savingvar = "1";
                    stw.WriteLine(savingvar);
                    //await File.WriteAllTextAsync(path, savingvar);
                    // diese Methode schreibt die Savingsvariable in die Datei des angegeben Pfades
                }


            }
            stw.Close();


        }

        public static void AlreadySaved(int level, string path) //gibt dem Benutzer auf Anfrage aus, ob das im Parameter angegebene Level bereits gespeichert wurde
        {
            StreamReader str = new StreamReader(path);

            while (!str.EndOfStream)
            {
                string line = str.ReadLine();

                if (line == "1")
                {
                    Console.WriteLine("Level wurde bereits gespeichert!");
                }
                else if (line == "0")
                {
                    Console.WriteLine("Noch kein Spielfortschritt in diesem Level vorhanden!");
                }
            }

            str.Close();
        }

    }
}
