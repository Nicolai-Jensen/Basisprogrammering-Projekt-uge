using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasisProgrammering___Projekt_uge
{

    enum Ship {Water, Ship}
    enum shots { O, M, H }

    internal class Program
    {
        // field variables for MineSweeper 
        static int[,] boardMS;
        static bool[,] isRevealedBoardMS;
        static int numberOfBombs;
        static int numberOfUncoveredPositions;

        //field variables for BattleShip
        static int numberOfHitsPlayer1;
        static int numberOfHitsPlayer2;


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
        /// This is the BattleShip game Function. The code for the Arrays and the randomised ships are here.
        /// </summary>
        static void BattleShip()
        {
            //startup show
            Console.WriteLine("BattleShip \n");

            //The 4 Arrays used throughout Battleship
            int[,] battleshipBoard1 = new int[10, 10];
            int[,] battleshipBoard2 = new int[10, 10];
            shots[,] boardPlayer1 = new shots[10, 10];
            shots[,] boardPlayer2 = new shots[10, 10];

            // The Menu of the game
            Console.WriteLine("This game requires 2 players who will take turns shooting coordinates \n");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("Instructions:");
            Console.WriteLine("\t The Goal is to destroy all the opposing players ships");
            Console.WriteLine("\t There are 5 ships equaling 16 unique positions");
            Console.WriteLine("\t Shoot by inputing the X and Y Value when prompted");
            Console.WriteLine("\t The ships are randomly placed for each player beforehand");
            Console.WriteLine("Board lingo:");
            Console.WriteLine("\t O: Indicates you haven't interacted with the space");
            Console.WriteLine("\t H: Indicates a ship has been hit");
            Console.WriteLine("\t M: Indicates a missed shot");
            Console.WriteLine("---------------------------------------------------------------------");            
            Console.Write("\t Player ones board");
            Console.Write("\t \t \t");
            Console.WriteLine(" Player two board");
            Console.WriteLine();

            //Show Hidden boards
            PlayerShipBoardcall(boardPlayer1, boardPlayer2);
            Console.WriteLine();
            Console.WriteLine("Press any button load random ship placements");
            Console.ReadLine();
            //Apply Ships to Board 1
            BattleShipRandom1(battleshipBoard1);
            Thread.Sleep(100);
            //Apply Ships to Board 2
            BattleShipRandom2(battleshipBoard2);
            
            //Reloads the boards with the ship placements and initiates the game
            Console.Clear();
            bool won = false;

            //The game loop that lets the players keep playing until one wins
            while (won == false)
            {
                Console.WriteLine();
                Console.WriteLine("\t \t \t \t     BattleShip");

                Console.WriteLine("\t ---------------------------------------------------------------------");
                PlayerShipBoardcall(boardPlayer1, boardPlayer2);
                Console.WriteLine("\t ---------------------------------------------------------------------");
                Console.WriteLine();

                won = Player1shot(battleshipBoard2, boardPlayer2);
                Console.WriteLine("Next turn. . ."); Console.ReadLine(); Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t \t \t \t     BattleShip");

                Console.WriteLine("\t ---------------------------------------------------------------------");
                PlayerShipBoardcall(boardPlayer1, boardPlayer2);
                Console.WriteLine("\t ---------------------------------------------------------------------");
                Console.WriteLine();

                won = Player2shot(battleshipBoard1, boardPlayer1);
                Console.WriteLine("Next turn. . ."); Console.ReadLine();Console.Clear();


            }
            
            //Draws the board along with the hidden boards that show boats
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t \t \t \t     BattleShip");

            Console.WriteLine("---------------------------------------------------------------------");
            PlayerShipBoardcall(boardPlayer1, boardPlayer2);
            Console.WriteLine("---------------------------------------------------------------------");
            BattleShipBoardcall(battleshipBoard1, battleshipBoard2);
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine();

            //Checks who won the game and displays a message congratulating them before returning to main menu
            if (numberOfHitsPlayer1 >= 16)
            {
                Console.WriteLine("\t \t \t Congratulations Player 1, you have won");
            }

            if (numberOfHitsPlayer2 >= 16)
            {
                Console.WriteLine("\t \t \t Congratulations Player 2, you have won");
            }

            Console.WriteLine("\n");
            Console.WriteLine("Return to main menu . . .");
            Console.ReadLine();
        }

        /// <summary>
        /// These 2 Functions let the players type out their shot coordinates and marks the spot as hit or miss
        /// </summary>
        /// <param name="battleshipBoard2"></param>
        /// <param name="boardPlayer2"></param>
        /// <returns></returns>
        static bool Player1shot(int [,] battleshipBoard2, shots[,] boardPlayer2)
        {
            //A bool check for the upcoming while loop
            bool validx = false;
            bool validy = false;
            
                Console.WriteLine("Player 1 choose a X coordinate");
                string inputxx = (Console.ReadLine());
            
            //Loop to check validity of the user input
            while (validx == false)
            {
                //Checks to make sure the input isn't empty and that it is a number
                if (inputxx.Equals("") || inputxx.Contains(" ") || !inputxx.All(char.IsDigit))
                {
                    Console.WriteLine("Please enter an input that is valid");
                    inputxx = (Console.ReadLine());
                }

                else
                {
                    //Checks the input as an int
                    int i;
                    if (int.TryParse(inputxx, out i))
                    {
                        //Makes sure that the input is inside the board
                        if (Convert.ToInt32(inputxx) < 0 || Convert.ToInt32(inputxx) > 9)
                        {
                            Console.WriteLine("Please enter an input that is valid");
                            inputxx = (Console.ReadLine());
                        }

                        //Breaks the loop
                        else validx = true;
                        
                    }

                    else
                    {
                        Console.WriteLine("Try to enter a valid input");
                        inputxx = (Console.ReadLine());
                    }
                }
            }
            int inputx = Convert.ToInt32(inputxx);

            Console.WriteLine("player 1 choose a Y coordinate");
            String inputyy = (Console.ReadLine());

            //Loop to check validity of the user input
            while (validy == false)
            {
                //Checks to make sure the input isn't empty and that it is a number
                if (inputyy.Equals("") || inputyy.Contains(" ") || !inputyy.All(char.IsDigit))
                {
                    Console.WriteLine("Please enter an input that is valid");
                    inputyy = (Console.ReadLine());
                }

                else
                {
                    //Checks the input as an int
                    int i;
                    if (int.TryParse(inputyy, out i))
                    {
                        //Makes sure that the input is inside the board
                        if (Convert.ToInt32(inputyy) < 0 || Convert.ToInt32(inputyy) > 9)
                        {
                            Console.WriteLine("Please enter an input that is valid");
                            inputyy = (Console.ReadLine());
                        }

                        //Breaks the loop
                        else validy = true;

                    }

                    else
                    {
                        Console.WriteLine("Try to enter a valid input");
                        inputyy = (Console.ReadLine());
                    }
                }
            }
            int inputy = Convert.ToInt32(inputyy);


            //checks the hidden board for ship locations first
            if (battleshipBoard2[inputy, inputx] == (int)Ship.Ship)
            {
                //If there is a ship there it'll check if you've hit the spot before
                if (boardPlayer2[inputy, inputx] == shots.H)

                {
                    Console.WriteLine("You already hit that spot");
                }
                //Mark spot as hit and adds a hit counter(hidden)
                else
                {
                    boardPlayer2[inputy, inputx] = shots.H;
                    Console.WriteLine("It's a hit!");
                    numberOfHitsPlayer1++;
                }
            }
            //marks spot as miss
            else
            {
                boardPlayer2[inputy, inputx] = shots.M;
                Console.WriteLine("Missed!");
            }

            //Once hit has counted to 16 it'll return true and the player will win the game
            if (numberOfHitsPlayer1 >= 16)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        /// <summary>
        /// The 2nd Function of the player inputs, essentially the same as the prior function but for player2
        /// </summary>
        /// <param name="battleshipBoard1"></param>
        /// <param name="boardPlayer1"></param>
        /// <returns></returns>
        static bool Player2shot(int[,] battleshipBoard1, shots[,] boardPlayer1)
        {
            //A bool check for the upcoming while loop
            bool validx = false;
            bool validy = false;

            Console.WriteLine("Player 2 choose a X coordinate");
            string inputxx = (Console.ReadLine());

            //Loop to check validity of the user input
            while (validx == false)
            {
                //Checks to make sure the input isn't empty and that it is a number
                if (inputxx.Equals("") || inputxx.Contains(" ") || !inputxx.All(char.IsDigit))
                {
                    Console.WriteLine("Please enter an input that is valid");
                    inputxx = (Console.ReadLine());
                }

                else
                {
                    //Checks the input as an int
                    int i;
                    if (int.TryParse(inputxx, out i))
                    {
                        //Makes sure that the input is inside the board
                        if (Convert.ToInt32(inputxx) < 0 || Convert.ToInt32(inputxx) > 9)
                        {
                            Console.WriteLine("Please enter an input that is valid");
                            inputxx = (Console.ReadLine());
                        }

                        //Breaks the loop
                        else validx = true;

                    }

                    else
                    {
                        Console.WriteLine("Try to enter a valid input");
                        inputxx = (Console.ReadLine());
                    }
                }
            }
            int inputx = Convert.ToInt32(inputxx);

            Console.WriteLine("player 2 choose a Y coordinate");
            String inputyy = (Console.ReadLine());

            //Loop to check validity of the user input
            while (validy == false)
            {
                //Checks to make sure the input isn't empty and that it is a number
                if (inputyy.Equals("") || inputyy.Contains(" ") || !inputyy.All(char.IsDigit))
                {
                    Console.WriteLine("Please enter an input that is valid");
                    inputyy = (Console.ReadLine());
                }

                else
                {
                    //Checks the input as an int
                    int i;
                    if (int.TryParse(inputyy, out i))
                    {
                        //Makes sure that the input is inside the board
                        if (Convert.ToInt32(inputyy) < 0 || Convert.ToInt32(inputyy) > 9)
                        {
                            Console.WriteLine("Please enter an input that is valid");
                            inputyy = (Console.ReadLine());
                        }

                        //Breaks the loop
                        else validy = true;

                    }

                    else
                    {
                        Console.WriteLine("Try to enter a valid input");
                        inputyy = (Console.ReadLine());
                    }
                }
            }
            int inputy = Convert.ToInt32(inputyy);

            //checks the hidden board for ship locations
            if (battleshipBoard1[inputy, inputx] == (int)Ship.Ship)
            {
                //Checks to see if you've shot that position before
                if (boardPlayer1[inputy, inputx] == shots.H)

                {
                    Console.WriteLine("You already hit that spot");
                }
                //Marks the position as hit and adds a hit counter for player 2
                else
                {
                    boardPlayer1[inputy, inputx] = shots.H;
                    Console.WriteLine("It's a hit!");
                    numberOfHitsPlayer2++;
                }
            }
            //Marks the position as miss
            else
            {
                boardPlayer1[inputy, inputx] = shots.M;
                Console.WriteLine("Missed!");
            }
            //Once hit has counted to 16 it'll return true and the player will win the game
            if (numberOfHitsPlayer2 >= 16)
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        /// <summary>
        /// A Function to Update the Battleship boards, Essentially draws them out and shows all the boats.
        /// </summary>
        /// <param name="battleshipBoard1"></param>
        /// <param name="battleshipBoard2"></param>
        static void BattleShipBoardcall(int[,] battleshipBoard1, int[,] battleshipBoard2)
        {
            //This for loop draws out both Arrays along the x and y axis which are labeled i and j in the code
            //The i is used only once as it indicates each Line downwards, meanwhile the j is used twice both boards need to be next to each other horizontally
            Console.Write("\t");
            for (int i = 0; i < battleshipBoard1.GetLength(0); i++)
            {
                Console.Write(i + "  ");
            }
            Console.Write("\t \t");
            for (int i = 0; i < battleshipBoard1.GetLength(0); i++)
            {
                Console.Write(i + "  ");
            }
            Console.WriteLine("\n");

            for (int i = 0; i < battleshipBoard1.GetLength(0); i++)
            {
                Console.Write(" " + i + "  \t");
                for (int j = 0; j < battleshipBoard1.GetLength(1); j++)
                {
                    if (battleshipBoard1[i,j] == (int)Ship.Ship)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(battleshipBoard1[i, j] + "  ");
                    Console.ResetColor();
                }
                Console.Write("\t \t");

                for (int j = 0; j < battleshipBoard2.GetLength(0); j++)
                {
                    if (battleshipBoard2[i, j] == (int)Ship.Ship)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(battleshipBoard2[i, j] + "  ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }



        }

        /// <summary>
        /// This Function calls the 2 boards that the players can see and use, they do not show the position of the boats
        /// </summary>
        /// <param name="boardPlayer1"></param>
        /// <param name="boardPlayer2"></param>
        static void PlayerShipBoardcall(shots[,] boardPlayer1, shots[,]boardPlayer2)
        {
            //This for loop draws out both Arrays along the x and y axis which are labeled i and j in the code
            //The i is used only once as it indicates each Line downwards, meanwhile the j is used twice both boards need to be next to each other horizontally
            Console.Write("\t");
            for (int i = 0; i < boardPlayer1.GetLength(0); i++)
            {
                Console.Write(i + "  ");
            }
            Console.Write("\t \t");
            for (int i = 0; i < boardPlayer1.GetLength(0); i++)
            {
                Console.Write(i + "  ");
            }
            Console.WriteLine("\n");

            for (int i = 0; i < boardPlayer1.GetLength(0); i++)
            {
                Console.Write(" " + i + "  \t");
                for (int j = 0; j < boardPlayer1.GetLength(1); j++)
                {
                    if (boardPlayer1[i, j] == shots.H)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    if (boardPlayer1[i,j] == shots.M)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write(boardPlayer1[i, j] + "  ");
                    Console.ResetColor();
                }
                Console.Write("\t \t");

                for (int j = 0; j < boardPlayer2.GetLength(0); j++)
                {        
                    if (boardPlayer2[i,j] == shots.H)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    if (boardPlayer2[i, j] == shots.M)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write(boardPlayer2[i, j] + "  ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }


        /// <summary>
        /// This function sets 5 ships randomly across board 1, these ships are as follows: A 5 length ship, 4 length ship, 3 length ship and two 2 length ships.
        /// </summary>
        /// <param name="battleshipBoard1"></param>
        static void BattleShipRandom1(int[,] battleshipBoard1)
        {
            //Generates random numbers for use in the loops and then sets bools to give the loops a condition
            Random random = new Random();
            bool check4 = true;
            bool check3 = true;
            bool check2 = true;
            bool check1 = true;
            bool check = true;

            //A series of checks to find room to place a new ship
            while (check4 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard1.GetLength(0));
                int row = random.Next(battleshipBoard1.GetLength(1));

                int randomShip = battleshipBoard1[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard1[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {
                    // Checks vertically downward
                    if (column + 4 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column + 1, row] == (int)Ship.Ship || battleshipBoard1[column + 2, row] == (int)Ship.Ship || battleshipBoard1[column + 3, row] == (int)Ship.Ship || battleshipBoard1[column + 4, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 1, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 2, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 3, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 4, row] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 4 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column - 1, row] == (int)Ship.Ship || battleshipBoard1[column - 2, row] == (int)Ship.Ship || battleshipBoard1[column - 3, row] == (int)Ship.Ship || battleshipBoard1[column - 4, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 1, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 2, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 3, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 4, row] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 4 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row + 1] == (int)Ship.Ship || battleshipBoard1[column, row + 2] == (int)Ship.Ship || battleshipBoard1[column, row + 3] == (int)Ship.Ship || battleshipBoard1[column, row + 4] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 1] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 2] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 3] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 4] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }

                    // Checks Horizontally Left
                    else if (row - 4 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row - 1] == (int)Ship.Ship || battleshipBoard1[column, row - 2] == (int)Ship.Ship || battleshipBoard1[column, row - 3] == (int)Ship.Ship || battleshipBoard1[column, row - 4] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 1] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 2] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 3] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 4] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }
                }
            }



            //A series of checks to find room to place a new ship
            while (check3 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard1.GetLength(0));
                int row = random.Next(battleshipBoard1.GetLength(1));

                int randomShip = battleshipBoard1[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard1[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks Horizontally Left
                    if (row - 3 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row - 1] == (int)Ship.Ship || battleshipBoard1[column, row - 2] == (int)Ship.Ship || battleshipBoard1[column, row - 3] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 1] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 2] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 3] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 3 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row + 1] == (int)Ship.Ship || battleshipBoard1[column, row + 2] == (int)Ship.Ship || battleshipBoard1[column, row + 3] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 1] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 2] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 3] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }

                    // Checks vertically downward
                    else if (column + 3 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column + 1, row] == (int)Ship.Ship || battleshipBoard1[column + 2, row] == (int)Ship.Ship || battleshipBoard1[column + 3, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 1, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 2, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 3, row] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 3 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column - 1, row] == (int)Ship.Ship || battleshipBoard1[column - 2, row] == (int)Ship.Ship || battleshipBoard1[column - 3, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 1, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 2, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 3, row] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }
                }
            }

            //A series of checks to find room to place a new ship
            while (check2 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard1.GetLength(0));
                int row = random.Next(battleshipBoard1.GetLength(1));

                int randomShip = battleshipBoard1[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard1[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks Horizontally Left
                    if (row - 2 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row - 1] == (int)Ship.Ship || battleshipBoard1[column, row - 2] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 1] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 2] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 2 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row + 1] == (int)Ship.Ship || battleshipBoard1[column, row + 2] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 1] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 2] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }

                    // Checks vertically downward
                    else if (column + 2 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column + 1, row] == (int)Ship.Ship || battleshipBoard1[column + 2, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 1, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 2, row] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 2 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column - 1, row] == (int)Ship.Ship || battleshipBoard1[column - 2, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 1, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 2, row] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }
                }
            }


            //A series of checks to find room to place a new ship
            while (check1 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard1.GetLength(0));
                int row = random.Next(battleshipBoard1.GetLength(1));

                int randomShip = battleshipBoard1[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard1[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks vertically downward
                    if (column + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column + 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 1, row] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column - 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 1, row] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }

                    // Checks Horizontally Left
                    else if (row - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row - 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 1] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row + 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 1] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }
                }
            }


            //A series of checks to find room to place a new ship
            while (check == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard1.GetLength(0));
                int row = random.Next(battleshipBoard1.GetLength(1));

                int randomShip = battleshipBoard1[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard1[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks Horizontally Left
                    if (row - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row - 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row - 1] = (int)Ship.Ship;
                            check = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column, row + 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column, row + 1] = (int)Ship.Ship;
                            check = false;
                        }
                    }

                    // Checks vertically downward
                    else if (column + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column + 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column + 1, row] = (int)Ship.Ship;
                            check = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard1[column - 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard1[column, row] = (int)Ship.Ship;
                            battleshipBoard1[column - 1, row] = (int)Ship.Ship;
                            check = false;
                        }
                    }
                }
            }

        }


        /// <summary>
        /// This function sets 5 ships randomly across board 2, these ships are as follows: A 5 length ship, 4 length ship, 3 length ship and two 2 length ships.
        /// </summary>
        /// <param name="battleshipBoard2"></param>
        static void BattleShipRandom2(int[,] battleshipBoard2)
        {
            //Generates random numbers for use in the loops and then sets bools to give the loops a condition
            Random random = new Random();
            bool check4 = true;
            bool check3 = true;
            bool check2 = true;
            bool check1 = true;
            bool check = true;

            //A series of checks to find room to place a new ship
            while (check4 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard2.GetLength(0));
                int row = random.Next(battleshipBoard2.GetLength(1));

                int randomShip = battleshipBoard2[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard2[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {
                    // Checks vertically downward
                    if (column + 4 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column + 1, row] == (int)Ship.Ship || battleshipBoard2[column + 2, row] == (int)Ship.Ship || battleshipBoard2[column + 3, row] == (int)Ship.Ship || battleshipBoard2[column + 4, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 1, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 2, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 3, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 4, row] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 4 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column - 1, row] == (int)Ship.Ship || battleshipBoard2[column - 2, row] == (int)Ship.Ship || battleshipBoard2[column - 3, row] == (int)Ship.Ship || battleshipBoard2[column - 4, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 1, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 2, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 3, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 4, row] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 4 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row + 1] == (int)Ship.Ship || battleshipBoard2[column, row + 2] == (int)Ship.Ship || battleshipBoard2[column, row + 3] == (int)Ship.Ship || battleshipBoard2[column, row + 4] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 1] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 2] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 3] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 4] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }

                    // Checks Horizontally Left
                    else if (row - 4 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row - 1] == (int)Ship.Ship || battleshipBoard2[column, row - 2] == (int)Ship.Ship || battleshipBoard2[column, row - 3] == (int)Ship.Ship || battleshipBoard2[column, row - 4] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 1] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 2] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 3] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 4] = (int)Ship.Ship;
                            check4 = false;
                        }
                    }
                }
            }



            //A series of checks to find room to place a new ship
            while (check3 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard2.GetLength(0));
                int row = random.Next(battleshipBoard2.GetLength(1));

                int randomShip = battleshipBoard2[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard2[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks Horizontally Left
                    if (row - 3 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row - 1] == (int)Ship.Ship || battleshipBoard2[column, row - 2] == (int)Ship.Ship || battleshipBoard2[column, row - 3] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 1] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 2] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 3] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 3 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row + 1] == (int)Ship.Ship || battleshipBoard2[column, row + 2] == (int)Ship.Ship || battleshipBoard2[column, row + 3] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 1] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 2] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 3] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }

                    // Checks vertically downward
                    else if (column + 3 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column + 1, row] == (int)Ship.Ship || battleshipBoard2[column + 2, row] == (int)Ship.Ship || battleshipBoard2[column + 3, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 1, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 2, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 3, row] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 3 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column - 1, row] == (int)Ship.Ship || battleshipBoard2[column - 2, row] == (int)Ship.Ship || battleshipBoard2[column - 3, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 1, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 2, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 3, row] = (int)Ship.Ship;
                            check3 = false;
                        }
                    }
                }
            }

            //A series of checks to find room to place a new ship
            while (check2 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard2.GetLength(0));
                int row = random.Next(battleshipBoard2.GetLength(1));

                int randomShip = battleshipBoard2[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard2[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks Horizontally Left
                    if (row - 2 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row - 1] == (int)Ship.Ship || battleshipBoard2[column, row - 2] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 1] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 2] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 2 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row + 1] == (int)Ship.Ship || battleshipBoard2[column, row + 2] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 1] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 2] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }

                    // Checks vertically downward
                    else if (column + 2 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column + 1, row] == (int)Ship.Ship || battleshipBoard2[column + 2, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 1, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 2, row] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 2 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column - 1, row] == (int)Ship.Ship || battleshipBoard2[column - 2, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 1, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 2, row] = (int)Ship.Ship;
                            check2 = false;
                        }
                    }
                }
            }


            //A series of checks to find room to place a new ship
            while (check1 == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard2.GetLength(0));
                int row = random.Next(battleshipBoard2.GetLength(1));

                int randomShip = battleshipBoard2[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard2[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks vertically downward
                    if (column + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column + 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 1, row] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column - 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 1, row] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }

                    // Checks Horizontally Left
                    else if (row - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row - 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 1] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row + 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 1] = (int)Ship.Ship;
                            check1 = false;
                        }
                    }
                }
            }


            //A series of checks to find room to place a new ship
            while (check == true)
            {
                //Places a Random number into the integors for column and row
                int column = random.Next(battleshipBoard2.GetLength(0));
                int row = random.Next(battleshipBoard2.GetLength(1));

                int randomShip = battleshipBoard2[column, row];


                //Checks to see if the random position already has a ship
                if (battleshipBoard2[column, row] == (int)Ship.Ship)
                {

                }

                //A serires of Range checks to make sure the ship stays within the Arrays parameters
                else
                {

                    // Checks Horizontally Left
                    if (row - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row - 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row - 1] = (int)Ship.Ship;
                            check = false;
                        }
                    }

                    // Checks Horizonally Right
                    else if (row + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column, row + 1] == (int)Ship.Ship)
                        {

                        }
                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column, row + 1] = (int)Ship.Ship;
                            check = false;
                        }
                    }

                    // Checks vertically downward
                    else if (column + 1 <= 9)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column + 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column + 1, row] = (int)Ship.Ship;
                            check = false;
                        }
                    }

                    // Checks vertically upward
                    else if (column - 1 >= 0)
                    {
                        // Another check to make sure the ship isn't overlapping other ships
                        if (battleshipBoard2[column - 1, row] == (int)Ship.Ship)
                        {

                        }

                        else
                        {
                            battleshipBoard2[column, row] = (int)Ship.Ship;
                            battleshipBoard2[column - 1, row] = (int)Ship.Ship;
                            check = false;
                        }
                    }
                }
            }

        }

    }
}
