namespace GameEngine.Exceptions
{

    public class CannotLoadFiles : FileNotFoundException
    {
        public CannotLoadFiles(string message) : base(message)
        {
        }
    }
}
