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
        /// Read File and return all Level Times as Timespan
        /// </summary>
        /// <returns></returns>
        public TimeSpan[] ReadLevels()
        {
            string[] levels = File.ReadAllLines(path); 
            TimeSpan[] levelsTime = new TimeSpan[levels.Length];
            for (int i = 0; i < levels.Length; i++) //conert
            {
                levelsTime[i] = TimeSpan.FromMilliseconds(Convert.ToDouble(levels[i]));
            }

            return levelsTime;
        }

        /// <summary>
        /// Write Timespan to file 
        /// </summary>
        /// <param name="levelNumber">level what got finished</param>
        /// <param name="newTime"></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when you want to write a level without finishing the level before</exception>
        public async void SaveLevel(int levelNumber, TimeSpan newTime)
        {
            levelNumber--;
            string[] levels = await File.ReadAllLinesAsync(path); //read
            if (levelNumber == levels.Length) //append
            {
                Array.Resize(ref levels, levels.Length + 1);
            }
            else if (levelNumber > levels.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(levelNumber), "value is to big ");
            }
            // check if current time is bigger than the saved one

            if (TimeSpan.FromMilliseconds(Convert.ToDouble(levels[levelNumber])) <= newTime && levels[levelNumber] != null) return; 
            levels[levelNumber] = newTime.TotalMilliseconds.ToString();
            await File.WriteAllLinesAsync(path, levels);
        }
    }
}