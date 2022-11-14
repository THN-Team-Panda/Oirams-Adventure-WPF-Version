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
        /// <summary>
        /// a path, that is being given by the user when creating a Saving object; 
        /// private because it is only needed within this class
        /// </summary>
        private string path;

        /// <summary>
        /// a constructor that requires the user to add a path when creating a saving object
        /// </summary>
        /// <param name="path"></param>
        public Saving(string path)
        {
            this.path = path;
        }


        /// <summary>
        /// a function that saves the current game progress in a data file;
        /// it needs a level as parameter in order to know which level so save
        /// </summary>
        /// <param name="level"></param>
        public void Save(int level)
        {
            //erst öffnen StreamReader dann StreamWriter
            //und Exceptions für versch. Fälle machen

            StreamReader str = new StreamReader(path);

            while (!str.EndOfStream)
            {
                string line = str.ReadLine();

            }
            str.Close();

            StreamWriter stw = new StreamWriter(path);
            string[] coordinate = new string[4];

            for (int j = 0; j < coordinate.Length; j++) // j correlates to the coordinates of the levels (i.e. line 1 = level 1 etc.)
            {

                if (j != level)
                {
                    coordinate[j] = "0";

                }

                if (j == level) // && j.data == 1 => dafür Array-Ansatz (auch für AlreadySaved-Funktion)
                {
                    coordinate[j] = "1";

                    //note for myself: await File.WriteAllTextAsync(path, savingvar);
                }


            }

            for (int i = 0; i < coordinate.Length; i++)
            {
                stw.Write(coordinate[i]);
            }



            stw.Close();


        }

        /// <summary>
        /// this function checks on request wether a level has already been saved or not 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool AlreadySaved(int level)
        {
            //eventuelle Änderung: statt jedes Level anzuzeigen, nur den Speicherfortschritt des angefragten Levels anzeigen
            StreamReader str = new StreamReader(path);
            bool issaved = false;

            while (!str.EndOfStream)
            {
                string line = str.ReadLine();

                if (line == "1")
                {
                    issaved = true;
                }
                else if (line == "0")
                {
                    issaved = false;
                }
            }

            str.Close();

            return issaved;
        }

    }
}
