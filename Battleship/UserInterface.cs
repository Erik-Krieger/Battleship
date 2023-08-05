using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class UserInterface
    {
        public UserInterface() { }

        public static int MainLoop(Board theBoard)
        {
            Console.WriteLine();

            while (true)
            {
                theBoard.PrintBoard();
                Console.WriteLine();
                Console.WriteLine("Make your move Admiral:");

                var aLine = Console.ReadLine();

                if (aLine == null)
                {
                    continue;
                }

                if (aLine == "quit" || aLine == "exit")
                {
                    break;
                }

                bool isGameOver = theBoard.Shoot(aLine);
                if (isGameOver)
                {
                    Console.WriteLine("Congratulations Admiral, you've defeated the enemy fleet.");
                    break;
                }
            }

            return 0;
        }
    }
}
