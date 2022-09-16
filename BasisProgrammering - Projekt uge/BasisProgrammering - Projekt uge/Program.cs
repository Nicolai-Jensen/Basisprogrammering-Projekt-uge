using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BasisProgrammering___Projekt_uge
{
    enum Ship {Miss, Ship, Shiphit, water}
    internal class Program
    {
        static void Main(string[] args)
        {

            //Main is where our start menu is located

            Console.WriteLine("Welcome to our Main Menu");
            Console.WriteLine("Choose a game \n");

            Console.WriteLine("1. MineSweeper");
            Console.WriteLine("2. Chess");
            Console.WriteLine("3. BattleShip");
            int input = int.Parse(Console.ReadLine());
            Console.Clear();

            if (input == 1)
            {
                MineSweeper();
            }

            else if (input == 2)
            {
                Chess();
            }

            else if (input == 3)
            {
                BattleShip();
            }

            Console.ReadLine();
        }

        static void MineSweeper()
        {

        }

        static void Chess()
        {

        }
        /// <summary>
        /// This is the BattleShip game, mainly the resposibility of Nicolai in the group.
        /// </summary>
        static void BattleShip()
        {
            Console.WriteLine("BattleShip \n");
            

            int[,] battleshipBoard1 = new int[10, 10];

            int[,] battleshipBoard2 = new int[10, 10];



            //This for loop draws out both Arrays along the x and y axis which are labeled i and j in the code
            //The i is used only once as it indicates each Line downwards, meanwhile the j is used twice both boards need to be next to each other horizontally
            BattleShipBoardcall(battleshipBoard1, battleshipBoard2);

            Random random = new Random();
            int row = random.Next(battleshipBoard1.GetLength(0));
            int column = random.Next(battleshipBoard1.GetLength(1));

            int randomShip = battleshipBoard1[row, column];

            battleshipBoard1[row, column] = (int)Ship.Ship;

            Console.WriteLine();
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("BattleShip \n");

            BattleShipBoardcall(battleshipBoard1, battleshipBoard2);
            Console.ReadLine();
        }

        

        static void BattleShipBoardcall(int[,] battleshipBoard1, int[,] battleshipBoard2)
        {
            //This for loop draws out both Arrays along the x and y axis which are labeled i and j in the code
            //The i is used only once as it indicates each Line downwards, meanwhile the j is used twice both boards need to be next to each other horizontally

            for (int i = 0; i < battleshipBoard1.GetLength(0); i++)
            {
                for (int j = 0; j < battleshipBoard1.GetLength(1); j++)
                {
                    Console.Write(battleshipBoard1[i, j] + "  ");
                }
                Console.Write("\t \t");

                for (int j = 0; j < battleshipBoard2.GetLength(0); j++)
                {
                    Console.Write(battleshipBoard2[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }
    }
}
