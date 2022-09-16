using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisProgrammering___Projekt_uge
{
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

        static void BattleShip()
        {

        }
    }
}
