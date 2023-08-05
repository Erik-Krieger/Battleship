namespace Battleship
{
    public class Program
    {
        public static void Main(string[] theArgs)
        {
            Console.WriteLine("Battleship v1");

            var aBoard = new Board(10);
            aBoard.PlaceShipsRandom();

            UserInterface.MainLoop(aBoard);
        }
    }
}