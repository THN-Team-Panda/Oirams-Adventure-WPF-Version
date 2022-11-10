using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine
{
    public class Physics
    {
        public Vector Gravity { get; set; }

        public bool IsCollidingWithMap(Map map, GameObject dyGameObject)
        {
            Vector mapPosition = dyGameObject.Position / map.TileSize;


            bool down = (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y + dyGameObject.height) / map.TileSize) + 1, (int)(dyGameObject.Position.X) / map.TileSize] != TileTypes.Void) ||
                        (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y + dyGameObject.height) / map.TileSize) + 1, (int)Math.Floor((double)(dyGameObject.Position.X + dyGameObject.width * 0.9) / map.TileSize)] !=
                            TileTypes.Void);
            bool up = (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y) / map.TileSize + 1), (int)(dyGameObject.Position.X) / map.TileSize] != TileTypes.Void) ||
                      (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y) / map.TileSize + 1), (int)Math.Floor((double)(dyGameObject.Position.X + dyGameObject.width * 0.9) / map.TileSize)] !=
                                            TileTypes.Void);
            bool right = (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y + dyGameObject.height * 0.9) / map.TileSize) + 1, (int)Math.Floor((dyGameObject.Position.X + dyGameObject.width) / map.TileSize)] !=
                         TileTypes.Void) ||
                         (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y) / map.TileSize + 1), (int)Math.Floor((dyGameObject.Position.X + dyGameObject.width) / map.TileSize)] !=
                                             TileTypes.Void);
            bool left = (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y + dyGameObject.height * 0.9) / map.TileSize) + 1, (int)Math.Floor((dyGameObject.Position.X ) / map.TileSize)] !=
                          TileTypes.Void) ||
                         (map.TileMap[(int)Math.Ceiling((double)(dyGameObject.Position.Y) / map.TileSize + 1), (int)Math.Floor((dyGameObject.Position.X ) / map.TileSize)] !=
                          TileTypes.Void);

            //for (int i = 0; i < map.TileMap.GetLength(0); i++)
            //{
            //    for (int k = 0; k < map.TileMap.GetLength(1); k++)
            //    {
            //        if ((int)mapPosition.X == k && (int)mapPosition.Y == i) Console.Write("X");
            //        else if((int)Math.Ceiling((double)(dyGameObject.Position.Y + dyGameObject.height) / map.TileSize) + 1==k &&(int)(dyGameObject.Position.X) / map.TileSize==i)
            //            Console.Write("D");
            //        else if((int)Math.Round((double)(dyGameObject.Position.Y) / map.TileSize + 1)==k &&(int)(dyGameObject.Position.X) / map.TileSize==i)
            //            Console.Write("U");
            //        else if((int)Math.Round((double)(dyGameObject.Position.Y + dyGameObject.height) / map.TileSize) + 1==k&& (int)Math.Floor((dyGameObject.Position.X + dyGameObject.width) / map.TileSize)==i)
            //            Console.Write("R");
            //        else Console.Write((int)map.TileMap[i, k]);
            //    }
            //    Console.WriteLine();
            //}
            if (down)
            {
                if (dyGameObject.Velocity.Y > 0)
                {
                    dyGameObject.Velocity = new Vector(dyGameObject.Velocity.X, 0);
                }
            }
            if (up)
            {
                if (dyGameObject.Velocity.Y < 0)
                {
                    dyGameObject.Velocity = new Vector(dyGameObject.Velocity.X, 0);
                }
            }
            if (right)
            {
                if (dyGameObject.Velocity.X > 0)
                {
                    dyGameObject.Velocity = new Vector(0, dyGameObject.Velocity.Y);
                }
            }
            if (left)
            {
                if (dyGameObject.Velocity.X < 0)
                {
                    dyGameObject.Velocity = new Vector(0, dyGameObject.Velocity.Y);
                }
            }
            return false;
        }
    }
}
//for (int i = 0; i < map.TileMap.GetLength(0); i++)
//{
//for (int k = 0; k < map.TileMap.GetLength(1); k++)
//{
//    //put a single value
//    if((int)mapPosition.X == k&&(int)mapPosition.Y ==i) Console.Write("X");
//    else Console.Write((int)map.TileMap[i, k]);
//}
////next row
//Console.WriteLine();
//}