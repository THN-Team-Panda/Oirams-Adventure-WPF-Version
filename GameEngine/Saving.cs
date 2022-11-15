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

            if ((!File.Exists("SavingTest.txt"))) //Checking if scores.txt exists or not
            {
                FileStream path = File.Create("SavingLevels.txt"); //Creates Scores.txt
                path.Close(); //Closes file stream
            }


            string[] alllines = new string[4];

            while (!str.EndOfStream)
            {

                for (int i = 0; i < alllines.Length; i++) //content of file is written into an Array
                {
                    alllines[i] = str.ReadLine();
                }

            }
            str.Close();

            StreamWriter stw = new StreamWriter(path);
            //string[] coordinate = new string[4];

            alllines[level] = "1";


            for (int i = 0; i < alllines.Length; i++)
            {
                stw.WriteLine(alllines[i]);

                Console.WriteLine(alllines[i]);
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
            //nochmal überarbeiten
            StreamReader str = new StreamReader(path);
            bool issaved = false;

            while (!str.EndOfStream)
            {
                string[] alllines = new string[4];
                string line = str.ReadLine();

                for (int i = 0; i < alllines.Length; i++) //lines of the file are being put into an array for locating the level of the parameter
                {
                    alllines[i] = str.ReadLine();
                }

                for (int j = 0; j < alllines.Length; j++)
                {
                    if (alllines[level - 1] == "1") // level - 1 because the index is shifted and without it would the return value be incorrect
                    {
                        issaved = true;
                    }
                    else if (line == "0")
                    {
                        issaved = false;
                    }
                    j++;
                }

            }


            str.Close();

            // Console.WriteLine(issaved); //for testing purposes
            return issaved;
        }

    }
}
