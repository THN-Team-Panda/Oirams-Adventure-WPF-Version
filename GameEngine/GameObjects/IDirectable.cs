using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// An interface for all directable game objects
    /// </summary>
    public interface IDirectable
    {
        /// <summary>
        /// Default should be false, means direction: right
        /// </summary>
        public bool DirectionLeft { get; set; } 
    }
}
