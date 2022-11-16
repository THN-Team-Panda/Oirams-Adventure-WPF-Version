using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using GameEngine.GameObjects;

namespace OA_Game.DrawableObjects.Obstacles
{
    public class Obstacles : DrawableObject
    {

        public Obstacles(Rectangle mapObject) : base(mapObject)
        {
        }

        /// <summary>
        /// If player hit obstacles he gets demage
        /// </summary>
        public void damage()
        {

        }
    }
}
