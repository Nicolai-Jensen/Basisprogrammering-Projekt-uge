using System;
using System.Linq;

namespace BasisProgrammering___Projekt_uge
{

    enum Ship { Miss, Ship, Shiphit, water }
    internal class Program
    {
        // field variables for MineSweeper 
        static int[,] boardMS;
        static bool[,] isRevealedBoardMS;
        static bool[,] isMarkedAsBomb;
        static int numberOfBombs;
        static int numberOfCoveredPositions;
        static int numberOfMarkedPositions;


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
            Console.WriteLine("\t\tMineSweeper");

            Console.WriteLine("______________________________________________________________________");

            // how are bombs visualized 
            Console.WriteLine("How the game is visualized: ");
            Console.WriteLine("\tBombs: \t\t\tX");
            Console.WriteLine("\tMarked positions: \tM");
            Console.WriteLine("\tCovered positions: \t-");
            Console.WriteLine("\tUncovered postions: \t0 to 8");

            // rules 
            Console.WriteLine("______________________________________________________________________");
            Console.WriteLine("How to play:");
            Console.WriteLine("\tUse keyboard.");
            Console.WriteLine("\tMark or Uncover position.");

            Console.WriteLine("\n\tMark: \n\t\tPress:m \n\t\tSelect horizontal position. \n\t\tSelect vertical position. \n\t\tPosition will be marked with an M.");

            Console.WriteLine("\n\tUncover: \n\t\tPress: u \n\t\tSelect horizontal position. \n\t\tSelect vertical position. \n\t\tPosition will be uncovered.");

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
            // max is set to 78 to prevent call stack overflow 
            Console.WriteLine("How big should the board be? \n\tMin: 1 \n\tMax: 78");
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

            // check if number of total bombs is less than half the board area 
            if (numberOfBombs < (boardMS.GetLength(0) * boardMS.GetLength(1)) / 2)
            {
                // the remaining bombs that should be placed 
                int remainingBombs = numberOfBombs;

                // as long as there are still bombs to be placed, run the loop 
                while (remainingBombs > 0)
                {
                    // get random x and y positions 
                    posX = random.Next(boardMS.GetLength(0));
                    posY = random.Next(boardMS.GetLength(1));

                    // check if the position on the board has a bomb 
                    if (boardMS[posX, posY] == 0)
                    {
                        // set a bomb on the random position on the board 
                        boardMS[posX, posY] = -1; // bombs are set as -1

                        // decrease amount of bombs remaining 
                        remainingBombs--;
                    }
                }
            }
            else
            {
                // if there are less white space than bombs in total 
                // set all positions on the board to be bombs 
                for (int i = 0; i < boardMS.GetLength(0); i++)
                {
                    for (int j = 0; j < boardMS.GetLength(1); j++)
                    {
                        boardMS[i, j] = -1; // bombs are set as -1
                    }
                }

                // then place white spaces 
                // the remaining white space 
                int remainingPositions = boardMS.GetLength(0) * boardMS.GetLength(1) - numberOfBombs;

                while (remainingPositions > 0)
                {
                    // get random x and y positions 
                    posX = random.Next(boardMS.GetLength(0));
                    posY = random.Next(boardMS.GetLength(1));

                    // check if the position on the board has a bomb 
                    if (boardMS[posX, posY] != 0)
                    {
                        // remove bomb on the random position on the board 
                        boardMS[posX, posY] = 0;

                        // decrease amount remaining 
                        remainingPositions--;
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
                    if (boardMS[i, j] == -1)
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
                            if (boardMS[x, y] != -1)
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
            // setup the board 
            BoardSetup();

            // as long as the player is alive, run the loop 
            bool isAlive = true;
            while (isAlive)
            {
                Console.Clear();
                // draw the board 
                DrawBoard();

                // check if board if full, and marked positions are not bombs 
                if(!IsMarkedBombs() && (numberOfCoveredPositions + numberOfMarkedPositions) == (boardMS.GetLength(0) * boardMS.GetLength(1)))
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    //Console.ForegroundColor = ConsoleColor.Black;
                    // there are positions marked that are not bombs 
                    Console.WriteLine("Some of the marked positions are not bombs.");
                    Console.ResetColor();
                }

                // uncover or mark 
                Console.WriteLine("Do you wish to uncover or mark a position? \n\tMark: \tm \n\tUncover: u");
                char sChoice = ReadInputForChoice();

                if (sChoice == 'm')
                {
                    Mark();
                }
                else if (sChoice == 'u')
                {
                    // uncover the board 
                    isAlive = Uncover();
                }

                // when player hit a bomb 
                if (!isAlive)
                {
                    Console.Clear();

                    // display text in color 
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Oh no! You hit a bomb. ");
                    Console.ResetColor();

                    // draw the revealed board 
                    DrawBoard();
                    break;
                }

                // player wins when the number of covered positions and marked positions is equal to the total number of positions 
                if ((numberOfCoveredPositions + numberOfMarkedPositions) == (boardMS.GetLength(0) * boardMS.GetLength(1)))
                {
                    // check if marked areas are bombs 
                    if (IsMarkedBombs())
                    {
                        Console.Clear();

                        // display text in color 
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("You won!");
                        Console.ResetColor();

                        // draw the revealed board 
                        DrawBoard();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that sets up isRevealedBoardMS and isMarkedAsBomb arrays 
        /// </summary>
        static void BoardSetup()
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
            // set the number of covered positions 
            numberOfCoveredPositions = 0;

            // multidimentional array of booleans, that shows positions on the board that are marked
            isMarkedAsBomb = new bool[boardMS.GetLength(0), boardMS.GetLength(1)];
            for (int j = 0; j < isMarkedAsBomb.GetLength(1); j++)
            {
                for (int i = 0; i < isMarkedAsBomb.GetLength(0); i++)
                {
                    isMarkedAsBomb[i, j] = false;
                }
            }
            // set the number of marked positions
            numberOfMarkedPositions = 0;
        }

        /// <summary>
        /// MineSweeper 
        /// function that draws the board in the console 
        /// </summary>
        static void DrawBoard()
        {
            // display number of bombs left on the board 
            Console.WriteLine("Number of bombs left: " + (numberOfBombs - numberOfMarkedPositions));
            Console.WriteLine("______________________________________________________________________");

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
                        if (boardMS[i, j] == -1)
                        {
                            // write the bomb as X
                            Console.Write("X ");
                        }
                        else
                        {
                            // write the number of bombs at the position of the board 
                            Console.Write(boardMS[i, j] + " ");
                        }
                    }
                    else if (isMarkedAsBomb[i, j])
                    {
                        Console.Write("M ");
                    }
                    else
                    {
                        // if position is covered, write: -
                        Console.Write("- ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("______________________________________________________________________");
        }

        /// <summary>
        /// MineSweeper 
        /// function that marks positions 
        /// </summary>
        static void Mark()
        {
            // promt player to uncover at position 
            Console.WriteLine("Mark at position: ");
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

            // check if uncovered 
            if (!isRevealedBoardMS[posX, posY])
            {
                // check if already marked
                if (isMarkedAsBomb[posX, posY])
                {
                    // unmark position 
                    isMarkedAsBomb[posX, posY] = false;
                    // decrement the number of marked positions 
                    numberOfMarkedPositions--;
                }
                else
                {
                    // mark position 
                    isMarkedAsBomb[posX, posY] = true;
                    // increment the number of marked positions 
                    numberOfMarkedPositions++;
                }
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
            if (boardMS[posX, posY] == -1)
            {
                // if player hits a bomb, reveal the entire board 
                // update the boardShow to reveal the board 
                for (int j = 0; j < isRevealedBoardMS.GetLength(1); j++)
                {
                    for (int i = 0; i < isRevealedBoardMS.GetLength(0); i++)
                    {
                        // set position to true, hence uncovered 
                        isRevealedBoardMS[i, j] = true;

                        // increment the number of covered positions 
                        numberOfCoveredPositions++;
                    }
                }

                // return that the player is dead 
                return false;
            }
            else if (!isRevealedBoardMS[posX, posY])
            {
                // uncover recursively using neighbours 
                UncoverNext(posX, posY);

                // return that the game should go on 
                return true;
            }
            else
            {
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
            // check if marked 
            if (isMarkedAsBomb[posX, posY])
            {
                isMarkedAsBomb[posX, posY] = false;
                numberOfMarkedPositions--;
            }

            // set this position to be uncovered 
            isRevealedBoardMS[posX, posY] = true;

            // increment the number of covered positions 
            numberOfCoveredPositions++;

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
                    if (boardMS[x, y] != -1 && !isRevealedBoardMS[x, y])
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
        /// function that check if marked positions are bombs 
        /// </summary>
        /// <returns>returns a boolean, true if all maked positions are bombs</returns>
        static bool IsMarkedBombs()
        {
            for (int i = 0; i < isMarkedAsBomb.GetLength(0); i++)
            {
                for (int j = 0; j < isMarkedAsBomb.GetLength(1); j++)
                {
                    // check if marked is a bomb
                    if (isMarkedAsBomb[i, j] && boardMS[i, j] != -1)
                    {
                        // return false, if marked is not a bomb 
                        return false;
                    }
                }
            }

            // return true, if marked were all bombs 
            return true;
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
                // test input with try parse 
                int i;
                if (int.TryParse(input, out i))
                {
                    // check if input is inside bounds of screen
                    if (Convert.ToInt32(input) < 1 || Convert.ToInt32(input) > 78)
                    {
                        Console.WriteLine("Please enter input that can fit in the screen.");
                        // call function again 
                        return ReadInputNumber();
                    }

                    // return valid input 
                    return input;
                }
                else
                {
                    Console.WriteLine("Please enter a valid integer");
                    // call function again 
                    return ReadInputNumber();
                }
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
                // test input with try parse 
                int i;
                if (int.TryParse(input, out i))
                {
                    // check if input is inside the specified constraints 
                    if (Convert.ToInt32(input) < 0 || Convert.ToInt32(input) > boardMS.GetLength(0) * boardMS.GetLength(1))
                    {
                        Console.WriteLine("Please enter an amount of bombs a specified.");
                        // call function again 
                        return ReadInputNumberBombs();
                    }

                    // return valid input 
                    return input;
                }
                else
                {
                    Console.WriteLine("Please enter a valid integer");
                    // call function again 
                    return ReadInputNumberBombs();
                }
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
                // test with try parse 
                int i;
                if (int.TryParse(input, out i))
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
                else
                {
                    Console.WriteLine("Please enter valid integer.");
                    // call function again 
                    return ReadInputForVertical();
                }

            }
        }

        /// <summary>
        /// MineSweeper 
        /// function that ensures valid input for choice of marking or uncovering 
        /// </summary>
        /// <returns>return valid input</returns>
        static char ReadInputForChoice()
        {
            // get player input 
            char input = Console.ReadKey().KeyChar;
            Console.ReadLine();

            // check if input is empty, and is valid 
            if (input.Equals(' '))
            {
                Console.WriteLine("Please enter valid input.");
                // call function again 
                return ReadInputForChoice();
            }
            else
            {
                // check if input is allowed 
                if (input.Equals('m') || input.Equals('u'))
                {
                    // return valid input 
                    return input;
                }
                else
                {
                    Console.WriteLine("Please enter one of the options.");
                    // call function again 
                    return ReadInputForChoice();
                }
            }
        }
    }
}
