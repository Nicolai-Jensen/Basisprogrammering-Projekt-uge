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

            // TODO: make a menu for the games 

            MineSweeper(); 

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

            DrawBoardMS(board, numberOfBombs); 
        }

        static void DrawBoardMS(int[,] board, int numberOfBombs)
        {

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {

                }

            }




        }





    }
}
