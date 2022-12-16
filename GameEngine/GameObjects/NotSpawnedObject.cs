using System.Windows;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// class to store the Information about an Game object that didn't spawn yet
    /// </summary>
    public class NotSpawnedObject
    {
        /// <summary>
        /// Instance of NotSpawnedObject
        /// </summary>
        /// <param name="name">Name of the Object</param>
        /// <param name="className">Class of the Object</param>
        /// <param name="position">Position of the Object in the Map</param>
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
