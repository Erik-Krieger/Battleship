using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    sealed internal class Destroyer : Ship
    {
        public static readonly ShipType TYPE = ShipType.Destroyer;
        public static readonly int LENGTH = 3;

        public Destroyer(int theXPos, int theYPos) : base(TYPE, LENGTH, 'd', theXPos, theYPos) { }
        public Destroyer() : this(-1, -1) { }
    }
}
