using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    sealed internal class Carrier : Ship
    {
        public static readonly ShipType TYPE = ShipType.Carrier;
        public static readonly int LENGTH = 5;

        public Carrier(int theXPos, int theYPos) : base(TYPE, LENGTH, 'c', theXPos, theYPos) { }
        public Carrier() : this(-1, -1) { }
    }
}
