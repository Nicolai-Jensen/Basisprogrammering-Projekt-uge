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

            // set the bombs on the board using function 
            SetBombsMS(board, numberOfBombs);

            BombsAroundMS(board);

            DrawBoardMS(board); 

        }

        static void DrawBoardMS(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i,j]+" "); 
                }
                Console.WriteLine(); 
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that sets the bombs on the board 
        /// </summary>
        /// <param name="board">predefined board from multidimentional array</param>
        /// <param name="numberOfBombs">predefined number of bombs that has to be placed on the board</param>
        static void SetBombsMS(int[,] board, int numberOfBombs)
        {
            // set bomb randomly on board 
            Random random = new Random();

            // set all the bombs on the board 
            while (numberOfBombs > 0)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        // only set bomb on position if there is no bomb present 
                        // only set if allowed by random 
                        if (board[i,j] == 0 && random.Next(5) == 1)
                        {
                            // mark the bomb on the board 
                            board[i, j] = 35; // ascii: 35 = # 
                            // decrease amount of bombs 
                            numberOfBombs--; 
                        }
                    }
                }
            }
        }

        /// <summary>
        /// MineSweeper
        /// function that checks how many bombs are around each spot 
        /// </summary>
        /// <param name="board"></param>
        static void BombsAroundMS(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    // save the position 
                    int boardPosI = i;
                    int boardPosJ = j;

                    // corner cases 
                    if (i == 0 && j == 0)
                    {
                        // if position at top left corner of board 
                        // positions to check 
                        int[] checkPositionXtl = new int[2] { 0, 1 };
                        int[] checkPositionYtl = new int[2] { 0, 1 };
                        // set number on board, of how many bombs are nearby, with function 
                        Console.WriteLine("Hej");
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXtl, checkPositionYtl);
                    }
                    else if (i == 0 && j == board.GetLength(1) - 1)
                    {
                        // if position at top right corner of board 
                        // positions to check 
                        int[] checkPositionXtr = new int[2] { 0, 1 };
                        int[] checkPositionYtr = new int[2] { -1, 0 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXtr, checkPositionYtr);
                    }
                    else if (i == board.GetLength(0) - 1 && j == 0)
                    {
                        // if position at bottom left corner 
                        // positions to check 
                        int[] checkPositionXbl = new int[2] { -1, 0 };
                        int[] checkPositionYbl = new int[2] { 0, 1 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXbl, checkPositionYbl);
                    }
                    else if (i == board.GetLength(0) - 1 && j == board.GetLength(1) - 1)
                    {
                        // if position at bottom right corner 
                        // positions to check 
                        int[] checkPositionXbr = new int[2] { -1, 0 };
                        int[] checkPositionYbr = new int[2] { -1, 0 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXbr, checkPositionYbr);
                    }
                    // edge cases 
                    else if (i == 0)
                    {
                        // if position at top edge of board 
                        // positions to check 
                        int[] checkPositionXtop = new int[2] { 0, 1 };
                        int[] checkPositionYtop = new int[3] { -1, 0, 1 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXtop, checkPositionYtop);
                    }
                    else if (i == board.GetLength(0)-1)
                    {
                        // if position at bottom edge of board
                        // positions to check 
                        int[] checkPositionXbottom = new int[2] { -1, 0 };
                        int[] checkPositionYbottom = new int[3] { -1, 0, 1 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXbottom, checkPositionYbottom);
                    }
                    else if(j == 0)
                    {
                        // if position at left edge of board
                        // positions to check 
                        int[] checkPositionXleft = new int[3] { -1, 0, 1 };
                        int[] checkPositionYleft = new int[2] { 0, 1 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXleft, checkPositionYleft);
                    }
                    else if(j == board.GetLength(1) - 1)
                    {
                        // if position at right edge of board
                        // positions to check 
                        int[] checkPositionXright = new int[3] { -1, 0, 1 };
                        int[] checkPositionYright = new int[2] { -1, 0 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i, j] = CountBombs(boardPosI, boardPosJ, board, checkPositionXright, checkPositionYright);
                    }
                    
                    // cases everywhere on the board 
                    else
                    {
                        // positions to check 
                        int[] checkPositionX = new int[3] { -1, 0, 1 };
                        int[] checkPositionY = new int[3] { -1, 0, 1 };
                        // set number on board, of how many bombs are nearby, with function 
                        board[i,j] = CountBombs(boardPosI, boardPosJ, board, checkPositionX, checkPositionY); 
                    }
                }
            }
        }
        static int CountBombs(int x, int y, int[,] board, int[] positionX, int[] positionY)
        {
            // set initial bomb count to 0 
            int bombCount = 0; 

            for (int i = 0; i < positionX.Length; i++)
            {
                for (int j = 0; j < positionY.Length; j++)
                {
                    // if there is a bomb on the board at position i,j 
                    if (board[x + positionX[i], y + positionY[j]] == 35)
                    {
                        // increment bomb count 
                        bombCount++; 
                    }
                }
            }

            // return the total amount of bombs nearby position 
            return bombCount; 
        }



        static void Chess()
        {

        }

        static void BattleShip()
        {


        }

        





    }
}
