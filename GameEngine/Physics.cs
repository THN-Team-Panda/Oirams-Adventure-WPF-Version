using System.Windows;
using GameEngine.GameObjects;

namespace GameEngine
{
    public class Physics
    {
        /// <summary>
        /// Vector to represent the Gravity
        /// </summary>
        public static readonly Vector Gravity = new(0, 0.1);

        /// <summary>
        /// checks collision between a game object and the map, and changes the velocity of game object if needed
        /// </summary>
        /// <param name="map"></param>
        /// <param name="gameObject"></param>
        /// <returns>a array that indicates which sides had collided with the map | 0: bottom of the object | 1: left side of object | 2 : top side | 3 right side| if all fours are set then its in the void</returns>
        public static bool[] IsCollidingWithMap(Map map, DrawableObject gameObject)
        {
            Vector position = gameObject.Position + gameObject.Velocity + new Vector(0, -32);
            bool[] sidesCollidedWith = new bool[4]; //number that indicates which side collided with the map

            if (position.X >= map.TileColumns * map.TileSize - map.TileSize || position.X < 0 || position.Y < 0 ||
                position.Y > map.TileRows * map.TileSize - 3 * map.TileSize)
            {
                return new[] { true, true, true, true };
            }
            //check if there is a collision with each sides
            bool down = (map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height) / map.TileSize) + 1, (int)(position.X) / map.TileSize] != TileTypes.Void) ||
                        (map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height) / map.TileSize) + 1, (int)Math.Floor((position.X + gameObject.Width * 0.9) / map.TileSize)] !=
                            TileTypes.Void);
            bool up = (map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)(position.X) / map.TileSize] != TileTypes.Void) ||
                      (map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)Math.Floor((position.X + gameObject.Width * 0.9) / map.TileSize)] !=
                                            TileTypes.Void);
            bool right = (map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height * 0.9) / map.TileSize) + 1, (int)Math.Floor((position.X + gameObject.Width) / map.TileSize)] !=
                         TileTypes.Void) ||
                         (map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)Math.Floor((position.X + gameObject.Width) / map.TileSize)] !=
                                             TileTypes.Void);
            bool left = (map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height * 0.9) / map.TileSize) + 1, (int)Math.Floor((position.X) / map.TileSize)] !=
                          TileTypes.Void) ||
                         (map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)Math.Floor((position.X) / map.TileSize)] !=
                          TileTypes.Void);

            //set velocity to 0 if moving in this direction
            if (down && gameObject.Velocity.Y > 0)
            {
                gameObject.Velocity = gameObject.Velocity with { Y = 0 };
                sidesCollidedWith[0] = true;
            }
            if (up && gameObject.Velocity.Y < 0)
            {
                gameObject.Velocity = gameObject.Velocity with { Y = 0 };
                sidesCollidedWith[2] = true;
            }
            if (right && gameObject.Velocity.X > 0)
            {
                gameObject.Velocity = gameObject.Velocity with { X = 0 };
                sidesCollidedWith[1] = true;
            }
            if (left && gameObject.Velocity.X < 0)
            {
                gameObject.Velocity = gameObject.Velocity with { X = 0 };
                sidesCollidedWith[3] = true;
            }
            return sidesCollidedWith;
        }
    }
}
