using System.Windows;
using GameEngine.GameObjects;
namespace GameEngine
{
    public static class Physics
    {
        /// <summary>
        /// Vector to represent the Gravity
        /// </summary>
        public static readonly Vector Gravity = new(0, 0.1);

        /// <summary>
        /// checks collision between a game object and the map and return with what it collided
        /// </summary>
        /// <param name="map"></param>
        /// <param name="gameObject"></param>
        /// <returns>a array that indicates which sides had collided with the map | 0: bottom of the object | 1: left side of object | 2 : top side | 3 right side| if all fours are set then its in the void</returns>
        public static TileTypes[] IsCollidingWithMap(Map map, DrawableObject gameObject)
        {
            Vector position = gameObject.Position + gameObject.Velocity + new Vector(0, -map.TileSize * 2);
            //number that indicates which side collided with the map

            if (position.X >= map.TileColumns * map.TileSize - map.TileSize || position.X < 0 || position.Y < 0 ||
                position.Y > map.TileRows * map.TileSize - 3 * map.TileSize)
            {
                return new[] { TileTypes.Void, TileTypes.Void, TileTypes.Void, TileTypes.Void };
            }
            //check if there is a collision with each sides

            TileTypes down1 = map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height) / map.TileSize) + 1, (int)(position.X) / map.TileSize];
            TileTypes down2 = map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height) / map.TileSize) + 1, (int)Math.Floor((position.X + gameObject.Width * 0.9) / map.TileSize)];
            TileTypes down3 = map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height) / map.TileSize) + 1, (int)Math.Floor((position.X + gameObject.Width * 0.5) / map.TileSize)];
            TileTypes down = FindTileInDirection(FindTileInDirection(down1, down2), down3);

            TileTypes up1 = map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)(position.X) / map.TileSize];
            TileTypes up2 = map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)Math.Floor((position.X + gameObject.Width * 0.9) / map.TileSize)];
            TileTypes up3 = map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)Math.Floor((position.X + gameObject.Width * 0.5) / map.TileSize)];
            TileTypes up = FindTileInDirection(FindTileInDirection(up1, up2), up3);

            TileTypes right1 = map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)Math.Floor((position.X + gameObject.Width) / map.TileSize)];
            TileTypes right2 = map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height * 0.9) / map.TileSize) + 1, (int)Math.Floor((position.X + gameObject.Width) / map.TileSize)];
            TileTypes right3 = map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height * 0.5) / map.TileSize) + 1, (int)Math.Floor((position.X + gameObject.Width) / map.TileSize)];
            TileTypes right = FindTileInDirection(FindTileInDirection(right1, right2),right3);

            TileTypes left1 = map.TileMap[(int)Math.Ceiling((position.Y) / map.TileSize + 1), (int)Math.Floor((position.X) / map.TileSize)];
            TileTypes left2 = map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height * 0.9) / map.TileSize) + 1, (int)Math.Floor((position.X) / map.TileSize)];
            TileTypes left3 = map.TileMap[(int)Math.Ceiling((position.Y + gameObject.Height * 0.5) / map.TileSize) + 1, (int)Math.Floor((position.X) / map.TileSize)];
            TileTypes left = FindTileInDirection(FindTileInDirection(left1, left2),left3);

            if (down is TileTypes.Ground or TileTypes.Obstacle && gameObject.Velocity.Y > 0)
            {
                gameObject.Velocity = gameObject.Velocity with { Y = 0 };
            }
            if (up is TileTypes.Ground or TileTypes.Obstacle && gameObject.Velocity.Y < 0)
            {
                gameObject.Velocity = gameObject.Velocity with { Y = 0 };
            }
            if (right is TileTypes.Ground or TileTypes.Obstacle && gameObject.Velocity.X > 0)
            {
                gameObject.Velocity = gameObject.Velocity with { X = 0 };
            }
            if (left is TileTypes.Ground or TileTypes.Obstacle && gameObject.Velocity.X < 0)
            {
                gameObject.Velocity = gameObject.Velocity with { X = 0 };
            }
            return new[] { down, left, up, right };
            //set velocity to 0 if moving in this direction

        }
        /// <summary>
        /// checks if two Gameobjects collide with each other
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>

        public static bool CheckCollisionBetweenGameObjects(DrawableObject obj1, DrawableObject obj2)
        {
            return (obj1.Position.X + obj1.Width >= obj2.Position.X && obj1.Position.X < obj2.Position.X + obj2.Width) &&
                   (obj1.Position.Y + obj1.Height >= obj2.Position.Y && obj1.Position.Y < obj2.Position.Y + obj2.Height);
        }

        /// <summary>
        /// Combines two Tiles into one with a priority
        /// </summary>
        /// <param name="tile1"></param>
        /// <param name="tile2"></param>
        /// <returns></returns>
        private static TileTypes FindTileInDirection(TileTypes tile1, TileTypes tile2)
        {
            return (tile1, tile2) switch
            {
                (TileTypes.Obstacle, _) => TileTypes.Obstacle,
                (_, TileTypes.Obstacle) => TileTypes.Obstacle,
                (TileTypes.Ground, _) => TileTypes.Ground,
                (_, TileTypes.Ground) => TileTypes.Ground,
                _ => TileTypes.Void,
            };
        }
    }
}
