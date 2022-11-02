using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
  
    public class CannotLoadFiles : FileNotFoundException
    {
        public CannotLoadFiles(string message) : base(message)
        {
        }
    }
}
