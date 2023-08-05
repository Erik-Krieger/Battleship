using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    sealed internal class PatrolBoat : Ship
    {
        public static readonly ShipType TYPE = ShipType.PatrolBoat;
        public static readonly int LENGTH = 2;

        public PatrolBoat(int theXPos, int theYPos) : base(TYPE, LENGTH, 'p', theXPos, theYPos) { }
        public PatrolBoat() : this(-1, -1) { }
    }
}
