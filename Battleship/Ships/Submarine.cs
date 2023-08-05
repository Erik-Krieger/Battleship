using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    sealed internal class Submarine : Ship
    {
        public static readonly ShipType TYPE = ShipType.Submarine;
        public static readonly int LENGTH = 3;
        public Submarine(int theXPos, int theYPos) : base(TYPE, LENGTH, 's', theXPos, theYPos) { }
        public Submarine() : this(-1, -1) { }
    }
}
