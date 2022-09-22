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
                    //MineSweeper();
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
