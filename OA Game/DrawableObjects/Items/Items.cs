using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using GameEngine.GameObjects;

namespace OA_Game.DrawableObjects
{
    public class Items : DrawableObject
    {
        /// <summary>
        /// shows if item is collected
        /// </summary>
        public bool collected = false;

        public bool visible = true; // evtl in Gameoject class da es für alle elemente wichtig ist ob sie sichtbar sind oder nicht 

        public Items(int height, int width) : base(height, width)
        {
        }

        /// <summary>
        /// when the item is collected it is no longer visible on the map and cannot be collected a second time 
        /// </summary>
        /// <param name="item"></param>
        public void getCollected(Items item)
        {
            item.collected = true;
            item.visible = false;
        }
    }
}
