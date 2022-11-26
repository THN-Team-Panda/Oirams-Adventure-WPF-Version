namespace GameEngine
{
    /// <summary>
    /// TODO Explain
    /// </summary>
    public class Saving
    {
        /// <summary>
        /// File name for the level save file
        /// </summary>
        private const string SaveFileName = "level.panda";

        /// <summary>
        /// a path, that is being given by the user when creating a Saving object; 
        /// private because it is only needed within this class
        /// </summary>
        private string path;

        /// <summary>
        /// a constructor that requires the user to add a path when creating a saving object
        /// Note: The argument must be a directory
        /// </summary>
        /// <param name="path">Path to the directory where the save file is located</param>
        public Saving(string path)
        {
            // Check if the argument is a dir
            if (!Directory.Exists(path))
                throw new ArgumentException("The path argument must be a directory");

            // Create path to save file
            this.path = $"{path}/{SaveFileName}";

            if ((File.Exists(this.path))) return; //Checking if scores.txt exists or not

            FileStream file = File.Create(this.path);
            file.Close(); //Closes file stream
        }

        /// <summary>
        /// a function that saves the current game progress in a data file;
        /// it needs a level as parameter in order to know which level so save
        /// </summary>
        /// <param name="level"></param>
        public void Save(int level)
        {
            //TODO English
            //erst öffnen StreamReader dann StreamWriter
            //und Exceptions für versch. Fälle machen                    

            StreamReader str = new(path);

            string[] allLines = new string[4];

            while (!str.EndOfStream)
            {

                for (int i = 0; i < allLines.Length; i++) //content of file is written into an Array
                {
                    allLines[i] = str.ReadLine();
                }

            }
            str.Close();

            StreamWriter stw = new StreamWriter(path);
            //string[] coordinate = new string[4];

            allLines[level] = "1";


            for (int i = 0; i < allLines.Length; i++)
            {
                stw.WriteLine(allLines[i]);

                Console.WriteLine(allLines[i]);
            }



            stw.Close();

        }

        /// <summary>
        /// this function checks on request whether a level has already been saved or not 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool AlreadySaved(int level)
        {
            
            //TODO nochmal überarbeiten
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
                    try
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
                    catch
                    {
                        throw new IndexOutOfRangeException("Level muss zwischen einschließlich 0 und 3 liegen!");
                    }



                }

            }


            str.Close();

            return issaved;
        }

    }
}