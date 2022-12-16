namespace GameEngine.Exceptions
{
    /// <summary>
    /// Exception for the map class
    /// </summary>
    public class CannotLoadFiles : FileNotFoundException
    {
        /// <summary>
        /// Instance a new CannotLoadFiles Exception
        /// </summary>
        /// <param name="message">Excpetion message</param>
        public CannotLoadFiles(string message) : base(message)
        {
        }
    }
}
