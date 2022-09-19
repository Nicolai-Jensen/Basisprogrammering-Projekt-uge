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
                    BattleShip();
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
            // TODO: make sure player can not put ENTER as the input 

            // promt player for board size 
            Console.WriteLine("How big should the board be?");
            string sWidth = "";
            string sHeight = "";

            // getting the width of the board: 
            // boolean to check if player input is allowed 
            bool isAllowedInput = true;
            do
            {
                // promt player for width input 
                Console.Write("Width: \t\t");
                sWidth = Console.ReadLine();
                // check if player input is a string that can be converted to an integer, and input is non-negative 
                if (sWidth.All(char.IsDigit) && Convert.ToInt32(sWidth) >= 0)
                {
                    isAllowedInput = false;
                }
                else
                {
                    // if input is not allowed
                    Console.WriteLine("Input is not allowed, try again.");
                    Console.ReadLine();
                }
                //ask player for input again if the previous input was not allowed
            } while (isAllowedInput);

            // getting the height of the board: 
            // boolean to check if player input is allowed 
            isAllowedInput = true;
            do
            {
                // promt the player for height input 
                Console.Write("Height: \t");
                sHeight = Console.ReadLine();
                // check if player input is a string that can be converted to an integer, and input is non-negative 
                if (sHeight.All(char.IsDigit) && Convert.ToInt32(sHeight) >= 0)
                {
                    isAllowedInput = false;
                }
                else
                {
                    // if input is not allowed  
                    Console.WriteLine("Input is not allowed, try again.");
                    Console.ReadLine();
                }
                //ask player for input again if the previous input was not allowed
            } while (isAllowedInput);

            // set the width and height of the board 
            int boardWidth = Convert.ToInt32(sWidth);
            int boardHeight = Convert.ToInt32(sHeight);

            // set board size with player input 
            boardMS = new int[boardWidth, boardHeight];

            // getting the total amount of bombs on the board: 
            string sBombs = "";
            isAllowedInput = true;
            do
            {
                // promt player for how many bombs should be on the board 
                Console.WriteLine("How many bombs? \n\tMin: 0 \n\tMax: {0}", boardWidth * boardHeight);
                sBombs = Console.ReadLine();
                // check if player input is a string that can be converted to an integer, and input is non-negative or not above max 
                if (sBombs.All(char.IsDigit) && Convert.ToInt32(sBombs) >= 0 && Convert.ToInt32(sBombs) <= boardWidth * boardHeight)
                {
                    isAllowedInput = false;
                }
                else
                {
                    // if input is not allowed 
                    Console.WriteLine("Input is not allowed, try again.");
                    Console.ReadLine();
                    isAllowedInput = true;
                }
                //ask player for input again if the previous input was not allowed
            } while (isAllowedInput);

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
            // TODO: maybe make better random placement of bombs 
            // make the object random using the Random class 
            Random random = new Random();

            // the remaining bombs that should be placed 
            int remainingBombs = numberOfBombs;

            // as long as there are still bombs to be placed, run the loop 
            while (remainingBombs > 0)
            {
                // set all the bombs on the board 
                for (int j = 0; j < boardMS.GetLength(1); j++)
                {
                    for (int i = 0; i < boardMS.GetLength(0); i++)
                    {
                        // only set bomb on position if there is no bomb present 
                        // only set if allowed by random 
                        if (remainingBombs != 0 && boardMS[i, j] == 0 && random.Next(boardMS.GetLength(0) * boardMS.GetLength(1)) == 1)
                        {
                            // mark the bomb on the board 
                            boardMS[i, j] = 35; // ascii: 35 = # 

                            // decrease amount of bombs remaining 
                            remainingBombs--;
                        }
                    }
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
                        int[,] neighbours = GetNeighbours(i, j);

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
            // boolean to check if index is inside the bounds 
            bool isIndexSizeAllowed = true;
            do
            {
                // promt player to uncover at position 
                Console.WriteLine("Uncover at position: ");
                string sHorizontal = "";
                string sVertical = "";

                // getting the horizontal position: 
                // boolean to check if player input is allowed 
                bool isAllowed = true;
                do
                {
                    // promt player for horizontal position input 
                    Console.Write("Horizontal: \t");
                    sHorizontal = Console.ReadLine() + "0";
                    // check if player input is a string with a letter in first position and integer at the second position 
                    if (Char.IsLetter(sHorizontal, 0) && Char.IsDigit(sHorizontal, 1))
                    {
                        isAllowed = false;
                    }
                    //ask player for input again if the previous input was not allowed
                } while (isAllowed);

                // getting the horizontal position: 
                // boolean to check if player input is allowed 
                isAllowed = true;
                do
                {
                    // promt player for vertical position input 
                    Console.Write("Vertical: \t");
                    sVertical = Console.ReadLine();
                    // check if player input is a string that can be converted to an integer
                    if (sVertical.All(char.IsDigit))
                    {
                        isAllowed = false;
                    }
                    //ask player for input again if the previous input was not allowed
                } while (isAllowed);

                // set the positions 
                // convert characters in string to int 
                int posX = Convert.ToInt32(sHorizontal[0]) - 97 + (Convert.ToInt32(sHorizontal[1]) - 48) * 26;
                int posY = Convert.ToInt32(sVertical) - 1;

                // check if player input is inside the bounds of the board 
                if (posX < 0 || posX > boardMS.GetLength(0) - 1 || posY < 0 || posY > boardMS.GetLength(1) - 1)
                {
                    Console.WriteLine("Position is not on the board, try again.");
                    Console.ReadLine();
                    Console.Clear();
                    DrawBoard();
                }
                else
                {
                    // input is allowed in all aspects 
                    isIndexSizeAllowed = false;

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
                        UncoverRecursive(posX, posY);

                        // return that the game should go on 
                        return true;
                    }
                }
                //ask player for input again if the previous input was not allowed
            } while (isIndexSizeAllowed);
            // return true, to make sure all routes has a return TODO 
            return true;
        }

        /// <summary>
        /// MineSweeper 
        /// function that calls itself to uncover neighbours that are not bombs 
        /// </summary>
        /// <param name="posX">the x position to check neighbours from</param>
        /// <param name="posY">the y position to check neighbours from</param>
        static void UncoverRecursive(int posX, int posY)
        {
            // set this position to be uncovered 
            isRevealedBoardMS[posX, posY] = true;

            // count down uncovered positions 
            numberOfUncoveredPositions--;

            // check if this position on the board is a zero 
            if (boardMS[posX, posY] == 0)
            {
                // get the neighbours for this position 
                int[,] neighbours = GetNeighbours(posX, posY);

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
                        UncoverRecursive(x, y);
                    }
                }
            }
        }


        // TODO: make better, less if-statements --> make smarter 
        /// <summary>
        /// MineSweeper 
        /// function for getting the neighbours of a certain position on the board 
        /// </summary>
        /// <param name="x">the x position on the board</param>
        /// <param name="y">the y position on the board</param>
        /// <returns>multidimentional array of the given positions neighbours</returns>
        static int[,] GetNeighbours(int x, int y)
        {
            // set max values 
            int xMax = boardMS.GetLength(0) - 1;
            int yMax = boardMS.GetLength(1) - 1;

            // corner cases 
            if (x == 0 && y == 0)
            {
                // upper left corner 
                int[,] neighbours = { { x + 1, y }, { x, y + 1 }, { x + 1, y + 1 } };
                return neighbours;
            }
            else if (x == xMax && y == 0)
            {
                // upper right corner 
                int[,] neighbours = { { x - 1, y }, { x - 1, y + 1 }, { x, y + 1 } };
                return neighbours;
            }
            else if (x == 0 && y == yMax)
            {
                // lower left corner 
                int[,] neighbours = { { x, y - 1 }, { x + 1, y - 1 }, { x + 1, y } };
                return neighbours;
            }
            else if (x == xMax && y == yMax)
            {
                // lower right corner 
                int[,] neighbours = { { x - 1, y - 1 }, { x, y - 1 }, { x - 1, y } };
                return neighbours;
            }
            // edge cases 
            else if (y == 0)
            {
                // top edge 
                int[,] neighbours = { { x - 1, y }, { x + 1, y }, { x - 1, y + 1 }, { x, y + 1 }, { x + 1, y + 1 } };
                return neighbours;
            }
            else if (y == yMax)
            {
                // bottom edge 
                int[,] neighbours = { { x - 1, y - 1 }, { x, y - 1 }, { x + 1, y - 1 }, { x - 1, y }, { x + 1, y } };
                return neighbours;
            }
            else if (x == 0)
            {
                // left edge 
                int[,] neighbours = { { x, y - 1 }, { x + 1, y - 1 }, { x + 1, y }, { x, y + 1 }, { x + 1, y + 1 } };
                return neighbours;
            }
            else if (x == xMax)
            {
                // right edge 
                int[,] neighbours = { { x - 1, y - 1 }, { x, y - 1 }, { x - 1, y }, { x - 1, y + 1 }, { x, y + 1 } };
                return neighbours;
            }
            // everywhere else 
            else
            {
                int[,] neighbours = { { x - 1, y - 1 }, { x, y - 1 }, { x + 1, y - 1 }, { x - 1, y }, { x + 1, y }, { x - 1, y + 1 }, { x, y + 1 }, { x + 1, y + 1 } };
                return neighbours;
            }
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
