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

            // promt user for board size 
            Console.WriteLine("How big should the board be?");
            int boardHeight = Convert.ToInt32(Console.ReadLine());
            int boardWidth = Convert.ToInt32(Console.ReadLine()); 
            
            // set board size with user input 
            int[,] board = new int[boardHeight,boardWidth];

            // promt user for how many bombs
            Console.WriteLine("How many bombs? \n\tMin: 0 \n\tMax: {0}", boardHeight*boardWidth);
            int numberOfBombs = Convert.ToInt32(Console.ReadLine());

            SetBombsMS(board, numberOfBombs); 


        }

        static void SetBombsMS(int[,] board, int numberOfBombs)
        {
            // set the bombs on the board 
            while (numberOfBombs > 0)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        // set bomb randomly on board 
                        Random random = new Random();
                        if (board[i,j] == 0 && random.Next(1) == 1)
                        {
                            board[i, j] = 35; // ascii: 35 = # 
                            numberOfBombs--; 
                        }
                    }
                }
            }
        }





        static void Chess()
        {

        }

        static void BattleShip()
        {


        }

        





    }
}
