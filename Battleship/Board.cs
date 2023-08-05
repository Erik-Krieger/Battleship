using Battleship.Ships;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Board
    {
        private readonly char[,] m_Board;
        private readonly List<Ship> m_Ships;
        private readonly int m_BoardSize;

        public Board(int theSize)
        {
            if (theSize < 5 || theSize > 26)
            {
                throw new ArgumentOutOfRangeException("The board size has to be between 5 and 26 cells squared.");
            }

            m_BoardSize = theSize;
            m_Board = GetEmptyBoard(theSize);
            m_Ships = new List<Ship>()
            {
                new Carrier(),
                new Ships.Battleship(),
                new Ships.Battleship(),
                new Destroyer(),
                new Destroyer(),
                new Submarine(),
                new Submarine(),
                new PatrolBoat(),
                new PatrolBoat(),
                new PatrolBoat(),
            };
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="theTarget"></param>
        /// <returns>true if the all ships have been sunk.</returns>
        public bool Shoot(string theTarget)
        {
            if (theTarget == null || theTarget.Length < 2)
            {
                Console.WriteLine("Invalid move. Type '<Letter><Digit>' e.g. 'A1'.");
                return false;
            }

            char aLetter = theTarget[0];
            string aNumber = theTarget.Substring(1);

            // Check if the first character is a letter.
            if (!char.IsLetter(aLetter))
            {
                Console.WriteLine($"'{aLetter}' is not a valid first character. Please use only letters.");
                return false;
            }

            // Convert the Digits to an integer
            if (!int.TryParse(aNumber, out int anYPos))
            {
                Console.WriteLine($"'{aNumber}' is not a valid second character. Please use only numbers.");
                return false;
            }

            // Convert the letter to a grind index.
            int anXPos = ConvertLetterToCellIndex(aLetter);

            // Decrement the Y-Position, due to a difference in starting index.
            anYPos--;

            // Bounds Check
            if (!IsInBounds(anXPos, anYPos))
            {
                Console.WriteLine($"The Position ({(char)(anXPos+65)}/{anYPos+1}) is outside the play area.");
                return false;
            }

            if (m_Board[anXPos, anYPos] != 'w')
            {
                Console.WriteLine("Invalid Target, that cell has already been attacked.");
                return false;
            }

            foreach (var aShip in m_Ships)
            {
                if (aShip.IsHit(anXPos, anYPos))
                {
                    if (aShip.IsSunk())
                    {
                        MarkShipSunk(aShip);
                        m_Ships.Remove(aShip);

                        return m_Ships.Count == 0;
                    }

                    SetCell(anXPos, anYPos, 'h');

                    return false;
                }
            }

            SetCell(anXPos, anYPos, 'm');

            return false;
        }

        private void MarkShipSunk(Ship theShip)
        {
            for (int anIdx = 0; anIdx < theShip.GetLength(); anIdx++)
            {
                if (theShip.IsHorizontal())
                {
                    SetCell(theShip.GetXPos() + anIdx, theShip.GetYPos(), theShip.GetLetter());
                }
                else
                {
                    SetCell(theShip.GetXPos(), theShip.GetYPos() + anIdx, theShip.GetLetter());
                }
            }
        }

        public static char[,] GetEmptyBoard(int theSize)
        {
            char[,] aBoard = new char[theSize, theSize];

            for (int anYPos = 0; anYPos < theSize; anYPos++)
            {
                for (int anXPos = 0; anXPos < theSize; anXPos++)
                {
                    aBoard[anXPos, anYPos] = 'w';
                }
            }

            return aBoard;
        }

        public void PlaceShipsRandom()
        {
            var anRng = new Random();

            foreach (var aShip in m_Ships)
            {
                // Repeat as long as the position collides with another Ship
                do
                {
                    Orientation anOrientation = anRng.Next() % 2 == 0 ? Orientation.Horizontal : Orientation.Vertical;

                    int anXPos;
                    int anYPos;

                    if (anOrientation == Orientation.Horizontal)
                    {
                        anXPos = anRng.Next(0, 10 - aShip.GetLength());
                        anYPos = anRng.Next(0, 10);
                    }
                    else
                    {
                        anXPos = anRng.Next(0, 10);
                        anYPos = anRng.Next(0, 10 - aShip.GetLength());
                    }

                    aShip.SetOrientation(anOrientation);
                    aShip.SetPosition(anXPos, anYPos);
                }
                while (CollidesWithAnotherShip(aShip));


            }
        }

        private bool CollidesWithAnotherShip(Ship theShip)
        {
            foreach (var aShip in m_Ships)
            {
                // Skip to the next iteration, when comparing with itself,
                // since any ship will always collide with itself.
                if (theShip == aShip)
                {
                    continue;
                }

                for (int anIdx = 0; anIdx < theShip.GetLength(); anIdx++)
                {
                    if (theShip.IsHorizontal())
                    {
                        if (aShip.IsHit(theShip.GetXPos() + anIdx, theShip.GetYPos(), true))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (aShip.IsHit(theShip.GetXPos(), theShip.GetYPos() + anIdx, true))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void DrawShipsOnGrid()
        {
            foreach (var aShip in m_Ships)
            {
                for (var aIdx = 0; aIdx < aShip.GetLength(); aIdx++)
                {
                    if (aShip.IsHorizontal())
                    {
                        SetCell(aShip.GetXPos() + aIdx, aShip.GetYPos(), aShip.GetLetter());
                    }
                    else
                    {
                        SetCell(aShip.GetXPos(), aShip.GetYPos() + aIdx, aShip.GetLetter());
                    }
                }
            }
        }

        public bool IsInBounds(int theXPos, int theYPos)
        {
            return (theXPos >= 0 && theXPos < m_BoardSize && theYPos >= 0 && theYPos < m_BoardSize);
        }

        public char GetValueAtCell(int theXPos, int theYPos)
        {
            if (!IsInBounds(theXPos, theYPos))
            {
                return '\0';
            }

            return m_Board[theXPos, theYPos];
        }

        public char GetValueAtCell(char theLetter, int theNumber)
        {
            int anXPos = ConvertLetterToCellIndex(theLetter);

            return GetValueAtCell(anXPos, theNumber);
        }

        private void SetCell(int theXPos , int theYPos, char theValue) 
        {
            if (!IsInBounds(theXPos, theYPos))
            {
                return;
            }

            m_Board[theXPos, theYPos] = theValue;
        }

        private void SetCell(char theLetter, int theNumber, char theValue)
        {
            int anXPos = ConvertLetterToCellIndex(theLetter);

            SetCell(anXPos , theNumber, theValue);
        }

        private int ConvertLetterToCellIndex(char theLetter)
        {
            if (!char.IsLetter(theLetter))
            {
                return -1;
            }

            int anIndex;
            if (char.IsUpper(theLetter))
            {
                anIndex = theLetter - 65;
            }
            else
            {
                anIndex = theLetter - 97;
            }

            return anIndex;
        }

        public void PrintBoard()
        {
            PrintColumnLetters(m_BoardSize);

            for (int anYPos = 0;  anYPos < m_BoardSize; anYPos++)
            {
                PrintRowNumber(anYPos + 1);

                for (int anXPos = 0; anXPos < m_BoardSize; anXPos++)
                {
                    char aChar = m_Board[anXPos, anYPos];
                    PrintCharInProperColor(aChar);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        public static void PrintCharInProperColor(char theLetter)
        {
            switch (theLetter)
            {
                case 'w':
                    PrintChar(theLetter, ConsoleColor.Blue); break;
                case 'c':
                case 'b':
                case 'd':
                case 's':
                case 'p':
                    PrintChar(theLetter, ConsoleColor.DarkGray); break;
                case 'm':
                    PrintChar(theLetter, ConsoleColor.Gray); break;
                case 'h':
                    PrintChar(theLetter, ConsoleColor.Red); break;
                default:
                    throw new Exception($"There was invalid Character within the playing board. ( {theLetter} )");
            }
        }

        public static void PrintChar(char theLetter, ConsoleColor theColor = ConsoleColor.White)
        {
            Console.ForegroundColor = theColor;
            Console.Write(theLetter);
            Console.ResetColor();
        }

        public static void PrintRowNumber(int theRowNumber)
        {
            if (theRowNumber >= 10)
            {
                Console.Write($"{theRowNumber} ");
            }
            else if (theRowNumber <= 0) 
            {
                Console.Write("   ");
            }
            else
            {
                Console.Write($" {theRowNumber} ");
            }
        }

        public static void PrintColumnLetters(int theSize)
        {
            PrintRowNumber(-1);

            char anChar = 'A';

            for (int aColumnIndex = 0; aColumnIndex < theSize; aColumnIndex++)
            {
                Console.Write($"{anChar++} ");
            }

            Console.WriteLine();
        }
    }
}
