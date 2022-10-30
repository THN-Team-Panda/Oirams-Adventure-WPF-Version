using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Engine
{
    public class Exceptions
    {

    }
    public class CannotLoadFiles : FileNotFoundException
    {
        public CannotLoadFiles(string message) : base(message)
        {
        }
    }
}
