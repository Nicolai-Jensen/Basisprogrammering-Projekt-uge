using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisProgrammering___Projekt_uge
{

    enum Ship { Miss, Ship, Shiphit, water }
    internal class Program
    {
        // field variables for MineSweeper 
        static int[,] boardMS;
        static bool[,] isRevealedBoardMS;
        static int numberOfBombs;
        static int numberOfUncoveredPositions;


        static void Main(string[] args)
        {
            // the menu 
            // boolean to check if player input is allowed 
            bool isAllowedInput = true;
            do
            {
                // promt user to pick game 
                Console.WriteLine("Pick a game to play, enter 1, 2, or 3: \n\t1. MineSweeper\n\t2. BattleShip\n\t3. Chess");
                Console.WriteLine("Or exit: \n\te for Exit");

                char c = Console.ReadKey().KeyChar;
                Console.ReadLine();

                if (c == '1')
                {
                    Console.Clear();
                    MineSweeper();
                    Console.Clear();
                }
                else if (c == '2')
                {
                    Console.Clear();
                    //BattleShip();
                    Console.Clear();
                }
                else if (c == '3')
                {
                    Console.Clear();
                }
                else if (c == 'e')
                {
                    isAllowedInput = false;
                }
                //ask player for input again if the previous input was not allowed
            } while (isAllowedInput);
        }


        /// <summary>
        /// MineSweeper 
        /// made by: Ida Marcher Jensen 
        /// </summary>
        static void MineSweeper()
        {
            // show startscreen to player 
            StartScreen();

            // boolean to check if player is still playing 
            bool playing = true;
            while (playing)
            {
                // initialize the board 
                InitializeBoard();

                // make a clear console 
                Console.Clear();

                // gameplay 
                GameLoop();

                // boolean to check if player input is allowed 
                bool isAllowedInput = true;
                do
                {
                    // promt the player to play again 
                    Console.WriteLine("\nPlay again? \n\tYes: y \n\tNo: n");
                    char c = Console.ReadKey().KeyChar;
                    if (c == 'n')
                    {
                        playing = false;
                        isAllowedInput = false;
                    }
                    else if (c == 'y')
                    {
                        isAllowedInput = false;
                    }
                    //ask player for input again if the previous input was not allowed
                } while (isAllowedInput);
                Console.ReadLine();
                Console.Clear();
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that draws the start screen 
        /// </summary>
        static void StartScreen()
        {
            Console.WriteLine("MineSweeper");

            // how are bombs visualized 
            Console.WriteLine("\nThe bombs are shown as: #\n");

            // rules 
            Console.WriteLine("______________________________________________________________________");
            Console.WriteLine("Rules:");
            Console.WriteLine("Bombs: \n\tFind the bombs. \n\tBombs can not be marked.");
            Console.WriteLine("Controls: \n\tUse keyboard. \n\tEnter horizontal index first. (will be promted) \n\tEnter vertical index second. (will be promted) ");
            Console.WriteLine("______________________________________________________________________");

            Console.WriteLine("\nPlay game: \n\tPress ENTER ");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// MineSweeper 
        /// function that sets up the board 
        /// </summary>
        static void InitializeBoard()
        {
            // promt player for board size 
            Console.WriteLine("How big should the board be?");
            // promt player for width input 
            Console.Write("Width: \t\t");
            string sWidth = ReadInputNumber();
            // promt the player for height input 
            Console.Write("Height: \t");
            string sHeight = ReadInputNumber();

            // set the width and height of the board 
            int boardWidth = Convert.ToInt32(sWidth);
            int boardHeight = Convert.ToInt32(sHeight);

            // set board size with player input 
            boardMS = new int[boardWidth, boardHeight];

            // promt player for how many bombs should be on the board 
            Console.WriteLine("How many bombs? \n\tMin: 0 \n\tMax: {0}", boardWidth * boardHeight);
            string sBombs = ReadInputNumberBombs();

            // set the number of total bombs 
            numberOfBombs = Convert.ToInt32(sBombs);

            // place the bombs randomly around the board 
            PlaceBombs();

            // place bomb count 
            BombsAround();
        }

        /// <summary>
        /// MineSweeper 
        /// function that randomly sets the bombs around the board 
        /// </summary>
        static void PlaceBombs()
        {
            // make the object random using the Random class 
            Random random = new Random();

            // initialize position variables 
            int posX;
            int posY;

            // the remaining bombs that should be placed 
            int remainingBombs = numberOfBombs;

            // as long as there are still bombs to be placed, run the loop 
            while (remainingBombs > 0)
            {
                // get random x and y positions 
                posX = random.Next(boardMS.GetLength(0) - 1);
                posY = random.Next(boardMS.GetLength(1) - 1);

                // check if the position on the board has a bomb 
                if (boardMS[posX, posY] == 0)
                {
                    // set a bomb on the random position on the board 
                    boardMS[posX, posY] = 35; // ascii: 35 = # 

                    // decrease amount of bombs remaining 
                    remainingBombs--;
                }
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that counts nearby bombs for each position on the board 
        /// </summary>
        static void BombsAround()
        {
            // check the entire board 
            for (int j = 0; j < boardMS.GetLength(1); j++)
            {
                for (int i = 0; i < boardMS.GetLength(0); i++)
                {
                    // check if position has a bomb 
                    if (boardMS[i, j] == 35)
                    {
                        // find the neighbours of the bomb 
                        int[,] neighbours = GetNeighboursOfPoint(i, j);

                        // count bombs at each neighbour 
                        for (int k = 0; k < neighbours.GetLength(0); k++)
                        {
                            // neighbour positions on board 
                            int x = neighbours[k, 0];
                            int y = neighbours[k, 1];

                            // check if neighbour is a bomb 
                            if (boardMS[x, y] != 35)
                            {
                                // count bomb for neighbour 
                                boardMS[x, y]++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that runs the gameplay, what the player sees when playing 
        /// </summary>
        static void GameLoop()
        {
            // multidimentional array of booleans, that shows positions on the board that are uncovered 
            isRevealedBoardMS = new bool[boardMS.GetLength(0), boardMS.GetLength(1)];
            for (int j = 0; j < isRevealedBoardMS.GetLength(1); j++)
            {
                for (int i = 0; i < isRevealedBoardMS.GetLength(0); i++)
                {
                    isRevealedBoardMS[i, j] = false;
                }
            }
            // set the number of uncovered positions to the total 
            numberOfUncoveredPositions = isRevealedBoardMS.GetLength(0) * isRevealedBoardMS.GetLength(1);

            // as long as the player is alive, run the loop 
            bool isAlive = true;
            while (isAlive)
            {
                Console.Clear();
                // draw the board 
                DrawBoard();

                Console.WriteLine("______________________________________________________________________");
                // uncover the board 
                isAlive = Uncover();

                // when player hit a bomb 
                if (!isAlive)
                {
                    Console.Clear();
                    Console.WriteLine("Oh no! You hit a bomb");

                    // draw the revealed board 
                    DrawBoard();
                    break;
                }

                // player wins when the number of undcovered positions is equal to the total number of bombs 
                if (numberOfUncoveredPositions == numberOfBombs)
                {
                    Console.Clear();
                    Console.WriteLine("Player won!");

                    // draw the revealed board 
                    DrawBoard();
                    break;
                }
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that draws the board in the console 
        /// </summary>
        static void DrawBoard()
        {
            // writing the horizontal positions at the top 
            char[] chars = new char[26] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            Console.Write(" \t");
            for (int i = 0; i < boardMS.GetLength(0); i++)
            {
                Console.Write(chars[i - 26 * (i / 26)] + " ");
            }
            Console.Write("\n \t");
            // if the board is very big (bigger than 26) then we want to write: a1, b1, etc. 
            for (int i = 0; i < boardMS.GetLength(0); i++)
            {
                if (i < 26)
                {
                    Console.Write("  ");
                }
                else
                {
                    Console.Write(i / 26 + " ");
                }
            }
            Console.WriteLine("\n");

            // draw the board 
            for (int j = 0; j < boardMS.GetLength(1); j++)
            {
                // write the vertical position to the left  
                Console.Write(j + 1 + "\t");

                // write the board to the console 
                for (int i = 0; i < boardMS.GetLength(0); i++)
                {
                    // check if uncovered 
                    if (isRevealedBoardMS[i, j])
                    {
                        // check if position on the board has a bomb 
                        if (boardMS[i, j] == 35)
                        {
                            // write the bomb as ascii format: 35=# 
                            Console.Write(Convert.ToChar(35) + " ");
                        }
                        else
                        {
                            // write the number of bombs at the position of the board 
                            Console.Write(boardMS[i, j] + " ");
                        }
                    }
                    else
                    {
                        // if position is covered, write: -
                        Console.Write("- ");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that uncovers the board 
        /// </summary>
        /// <returns>returns a boolean, used to tell if player is alive/hit a bomb</returns>
        static bool Uncover()
        {
            // promt player to uncover at position 
            Console.WriteLine("Uncover at position: ");
            // promt player for horizontal position input 
            Console.Write("Horizontal: \t");
            string sHorizontal = ReadInputForHorizontal();
            // promt player for vertical position input 
            Console.Write("Vertical: \t");
            string sVertical = ReadInputForVertical();

            // set the positions 
            // convert characters in string to int 
            int posX = Convert.ToInt32(sHorizontal[0]) - 97 + (Convert.ToInt32(sHorizontal[1]) - 48) * 26;
            int posY = Convert.ToInt32(sVertical) - 1;

            // check if position on board has a bomb 
            if (boardMS[posX, posY] == 35)
            {
                // if player hits a bomb, reveal the entire board 
                // update the boardShow to reveal the board 
                for (int j = 0; j < isRevealedBoardMS.GetLength(1); j++)
                {
                    for (int i = 0; i < isRevealedBoardMS.GetLength(0); i++)
                    {
                        // set position to true, hence uncovered 
                        isRevealedBoardMS[i, j] = true;

                        // count down uncovered positions 
                        numberOfUncoveredPositions--;
                    }
                }

                // return that the player is dead 
                return false;
            }
            else
            {
                // uncover recursively using neighbours 
                UncoverNext(posX, posY);

                // return that the game should go on 
                return true;
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that calls itself to uncover neighbours that are not bombs 
        /// </summary>
        /// <param name="posX">the x position to check neighbours from</param>
        /// <param name="posY">the y position to check neighbours from</param>
        static void UncoverNext(int posX, int posY)
        {
            // set this position to be uncovered 
            isRevealedBoardMS[posX, posY] = true;

            // count down uncovered positions 
            numberOfUncoveredPositions--;

            // check if this position on the board is a zero 
            if (boardMS[posX, posY] == 0)
            {
                // get the neighbours for this position 
                int[,] neighbours = GetNeighboursOfPoint(posX, posY);

                // for every neighbour, 
                for (int i = 0; i < neighbours.GetLength(0); i++)
                {
                    // neighbour position on board 
                    int x = neighbours[i, 0];
                    int y = neighbours[i, 1];

                    // check if neighbour is a bomb, and if neighbour is shown
                    if (boardMS[x, y] != 35 && !isRevealedBoardMS[x, y])
                    {
                        // set neighbour to be shown
                        isRevealedBoardMS[x, y] = true;

                        // uncover the neighbour neighbours 
                        UncoverNext(x, y);
                    }
                }
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function for getting the neighbours of a given position on the board 
        /// </summary>
        /// <param name="x">the x position on the board</param>
        /// <param name="y">the y position on the board</param>
        /// <returns>multidimentional array of the given positions neighbours</returns>
        static int[,] GetNeighboursOfPoint(int x, int y)
        {
            // set neighbours to be empty at the start
            int[,] neighbours = new int[0, 2];

            // loop i and j from -1 to 1, as what needs to be added to the current position 
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        // do not add itself 
                        continue;
                    }
                    // check if position {x+i, y+j} is on the board 
                    if (IsOnBoard(x + i, y + j))
                    {
                        // add the position as a valid neighbour to the array 
                        neighbours = AddItemToArray(x + i, y + j, neighbours);
                    }
                }
            }

            // return the array of neighbours 
            return neighbours;
        }

        /// <summary>
        /// MineSweeper 
        /// function to check if given position is on the board 
        /// </summary>
        /// <param name="x">the x position</param>
        /// <param name="y">the y position</param>
        /// <returns>returns a boolean to see if statement is true or false</returns>
        static bool IsOnBoard(int x, int y)
        {
            return x >= 0 && y >= 0 && x < boardMS.GetLength(0) && y < boardMS.GetLength(1);
        }

        /// <summary>
        /// MineSweeper 
        /// function that adds an item to an array 
        /// </summary>
        /// <param name="x">the x position</param>
        /// <param name="y">the y position</param>
        /// <param name="arr">the given array that should be added to</param>
        /// <returns>returns the array with the given element {x,y}</returns>
        static int[,] AddItemToArray(int x, int y, int[,] arr)
        {
            // make new array, and increase size 
            int[,] newArr = new int[arr.GetLength(0) + 1, 2];

            // get all from old array into new array 
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                // x position 
                newArr[i, 0] = arr[i, 0];
                // y position 
                newArr[i, 1] = arr[i, 1];
            }
            // set the last element into new array 
            newArr[newArr.GetLength(0) - 1, 0] = x;
            newArr[newArr.GetLength(0) - 1, 1] = y;

            // return the new array 
            return newArr;
        }

        /// <summary>
        /// MineSweeper
        /// function that ensures valid number input from the player 
        /// </summary>
        /// <returns>returns the valid input</returns>
        static string ReadInputNumber()
        {
            // get player input 
            string input = Console.ReadLine();

            // check if input is empty, and is a valid number 
            if (input.Equals("") || input.Contains(" ") || !input.All(char.IsDigit))
            {
                Console.WriteLine("Please enter valid input.");
                // call function again 
                return ReadInputNumber();
            }
            else
            {
                // check if input is greater than zero 
                if (Convert.ToInt32(input) < 0)
                {
                    Console.WriteLine("Please enter non-negative input.");
                    // call function again 
                    return ReadInputNumber();
                }

                // return valid input 
                return input;
            }
        }

        /// <summary>
        /// MineSweeper
        /// function that ensures valid number of bombs input from the player 
        /// </summary>
        /// <returns>returns the valid input</returns>
        static string ReadInputNumberBombs()
        {
            // get player input 
            string input = Console.ReadLine();

            // check if input is empty, and is a valid number 
            if (input.Equals("") || input.Contains(" ") || !input.All(char.IsDigit))
            {
                Console.WriteLine("Please enter valid input.");
                // call function again 
                return ReadInputNumberBombs();
            }
            else
            {
                if (Convert.ToInt32(input) < 0 || Convert.ToInt32(input) > boardMS.GetLength(0) * boardMS.GetLength(1))
                {
                    Console.WriteLine("Please enter an amount of bombs a specified.");
                    // call function again 
                    return ReadInputNumberBombs();
                }

                // return valid input 
                return input;
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that ensures valid input for horizontal position from player
        /// </summary>
        /// <returns>returns the valid input</returns>
        static string ReadInputForHorizontal()
        {
            // get player input 
            string input = Console.ReadLine() + "0";

            // check if input is empty, has a letter followed by a number, and is inside the bounds 
            if (input.Equals("") || input.Contains(" ") || input.Length < 2 || !Char.IsLetter(input[0]) || !Char.IsDigit(input[1]))
            {
                Console.WriteLine("Please enter valid input.");
                // call function again 
                return ReadInputForHorizontal();
            }
            else
            {
                // convert characters in string to int - using ascii 
                int posX = Convert.ToInt32(input[0]) - 97 + (Convert.ToInt32(input[1]) - 48) * 26;

                // check if inside the bounds of the board 
                if (posX < 0 || posX > boardMS.GetLength(0) - 1)
                {
                    Console.WriteLine("Please enter a position inside the board.");
                    // call function again 
                    return ReadInputForHorizontal();
                }

                // return valid input 
                return input;
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that ensures valid input for vertical position from player 
        /// </summary>
        /// <returns>returns valid input</returns>
        static string ReadInputForVertical()
        {
            // get player input 
            string input = Console.ReadLine();

            // check if input is empty, and is a valid number 
            if (input.Equals("") || input.Contains(" ") || !input.All(char.IsDigit))
            {
                Console.WriteLine("Please enter valid input.");
                // call function again 
                return ReadInputForVertical();
            }
            else
            {
                // check if inside the bounds of the board 
                int posY = Convert.ToInt32(input) - 1;
                if (posY < 0 || posY > boardMS.GetLength(1) - 1)
                {
                    Console.WriteLine("Please enter a position inside the board.");
                    // call function again 
                    return ReadInputForVertical();
                }

                // return valid input 
                return input;
            }
        }
    }
}
