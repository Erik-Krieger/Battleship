using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    sealed internal class Battleship : Ship
    {
        public static readonly ShipType TYPE = ShipType.Battleship;
        public static readonly int LENGTH = 4;

        public Battleship(int theXPos, int theYPos) : base(TYPE, LENGTH, 'b', theXPos, theYPos) { }
        public Battleship() : this(-1, -1) { }
    }
}
