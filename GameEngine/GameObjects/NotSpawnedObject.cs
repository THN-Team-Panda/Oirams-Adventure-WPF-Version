using System.Windows;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// class to store the Information about an Game object that didn't spawn yet
    /// </summary>
    public class NotSpawnedObject
    {
        public NotSpawnedObject(string name, string className, Vector position)
        {
            Name = name;
            ClassName = className;
            Position = position;
        }

        /// <summary>
        /// Name of the Object
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Class of the Object
        /// </summary>
        public string ClassName { get; }
        /// <summary>
        /// Position of the Object in the Map
        /// </summary>
        public Vector Position { get; }
    }
}
