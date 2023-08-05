using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Ships
{
    internal enum Orientation
    {
        Horizontal,
        Vertical
    }

    internal enum ShipType
    {
        Carrier = 1,
        Battleship = 2,
        Destroyer = 3,
        Submarine = 4,
        PatrolBoat = 5,
    }

    internal abstract class Ship
    {
        private ShipType m_Type;
        private int m_Length;
        private int m_HitCount;
        private char m_LetterRepresentation;
        private int m_XPos = -1;
        private int m_YPos = -1;
        private Orientation m_Orientation;

        public ShipType Type { get { return m_Type; } }

        public Ship(ShipType theType, int theLength, char theLetterRepresentation, int theXPos, int theYPos)
        {
            m_Type = theType;
            m_Length = theLength;
            m_HitCount = 0;
            m_XPos = theXPos;
            m_YPos = theYPos;
            m_LetterRepresentation = theLetterRepresentation;
        }

        public bool IsHit(int theXPos, int theYPos, bool isPlacementOnly = false)
        {
            if (m_XPos == -1 || m_YPos == -1)
            {
                return false;
            }

            bool isHit;
            if (m_Orientation == Orientation.Horizontal)
            {
                isHit = (theYPos == m_YPos && theXPos >= m_XPos && theXPos < m_XPos + m_Length);
            }
            else
            {
                isHit = (theXPos == m_XPos && theYPos >= m_YPos && theYPos < m_YPos + m_Length);
            }

            if (isHit && !isPlacementOnly) {
                m_HitCount++;
            }

            return isHit;
        }

        public int GetLength()
        {
            return m_Length;
        }

        public bool IsHorizontal()
        {
            return m_Orientation == Orientation.Horizontal;
        }

        public bool IsSunk()
        {
            return m_HitCount >= m_Length;
        }

        public char GetLetter()
        {
            return m_LetterRepresentation;
        }

        public int GetXPos()
        {
            return m_XPos;
        }

        public int GetYPos()
        {
            return m_YPos;
        }

        public void SetXPos(int theXPos)
        {
            m_XPos = theXPos;
        }

        public void SetYPos(int theYPos)
        {
            m_YPos = theYPos;
        }

        public void SetPosition(int theXPos, int theYPos)
        {
            m_XPos = theXPos;
            m_YPos = theYPos;
        }

        public void SetOrientation(Orientation theOrientation)
        {
            m_Orientation = theOrientation;
        }
    }
}
