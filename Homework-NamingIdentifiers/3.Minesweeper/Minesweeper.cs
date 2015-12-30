using System;
using System.Collections.Generic;

namespace _3.Minesweeper
{
    public class Minesweeper
    {   
        private static void Main()
        {
            string command = string.Empty;
            char[,] gameBoard = CreateGameBoard();
            char[,] bombs = PutTheBombs();                        
            List<Player> shampions = new List<Player>(6);
            int row = 0;
            int column = 0;
            bool isTheBeginning = true;            
            bool isGameWon = false;
            bool isABombHitted = false;
            int pointsCounter = 0;
            const int Max = 35;

            do
            {
                if (isTheBeginning)
                {
                    Console.WriteLine("Let's play minesweeper. Enter 'top' to show chart,");
                    Console.WriteLine("'restart' to start from beginning or 'exit' to quit the game.");
                    PrintGameBoard(gameBoard);
                    isTheBeginning = false;
                }

                Console.Write("Enter row and column: ");
                command = Console.ReadLine().Trim();
                if (command.Length >= 3)
                {
                    if (int.TryParse(command[0].ToString(), out row) && int.TryParse(command[2].ToString(), out column)
                        && row <= gameBoard.GetLength(0) && column <= gameBoard.GetLength(1))
                    {
                        command = "turn";
                    }
                }

                switch (command)
                {
                    case "top":
                        PrintChart(shampions);
                        break;
                    case "restart":
                        gameBoard = CreateGameBoard();
                        bombs = PutTheBombs();
                        PrintGameBoard(gameBoard);
                        isABombHitted = false;
                        isTheBeginning = false;
                        break;
                    case "exit":
                        Console.WriteLine("Bye!");
                        break;
                    case "turn":
                        ProceedNewTurn(gameBoard, bombs, row, column, ref isGameWon, ref isABombHitted, ref pointsCounter, Max);

                        break;
                    default:
                        Console.WriteLine("\nError! Invalid command!\n");
                        break;
                }

                if (isABombHitted)
                {
                    PrintGameBoard(bombs);
                    Console.Write("\nGame over! You have {0} points. " + "Enter nickname: ", pointsCounter);
                    AddPlayerToChart(shampions, pointsCounter);
                    PrintChart(shampions);
                    StartAgain(ref gameBoard, ref bombs, ref isTheBeginning, ref isGameWon, ref pointsCounter);
                }

                if (isGameWon)
                {
                    Console.WriteLine("\nYou win the game");
                    PrintGameBoard(bombs);
                    AddPlayerToChart(shampions, pointsCounter);
                    PrintChart(shampions);
                    StartAgain(ref gameBoard, ref bombs, ref isTheBeginning, ref isGameWon, ref pointsCounter);
                }
            }
            while (command != "exit");

            Console.WriteLine("Made in Bulgaria");
            Console.WriteLine("Play again!");
            Console.Read();
        }

        private static void ProceedNewTurn(char[,] gameBoard, char[,] bombs, int row, int column, ref bool isGameWon, ref bool isABombHitted, ref int pointsCounter, int Max)
        {
            if (bombs[row, column] != '*')
            {
                if (bombs[row, column] == '-')
                {
                    ShowTheNumberOfBombsNextToThisField(gameBoard, bombs, row, column);
                    pointsCounter++;
                }

                if (Max == pointsCounter)
                {
                    isGameWon = true;
                }
                else
                {
                    PrintGameBoard(gameBoard);
                }
            }
            else
            {
                isABombHitted = true;
            }
        }

        private static void StartAgain(ref char[,] gameBoard, ref char[,] bombs, ref bool isTheBeginning, ref bool isGameWon, ref int pointsCounter)
        {
            gameBoard = CreateGameBoard();
            bombs = PutTheBombs();
            pointsCounter = 0;
            isGameWon = false;
            isTheBeginning = true;
        }

        private static void AddPlayerToChart(List<Player> shampions, int pointsCounter)
        {
            string nickname = Console.ReadLine();
            Player player = new Player(nickname, pointsCounter);
            if (shampions.Count < 5)
            {
                shampions.Add(player);
            }
            else
            {
                for (int i = 0; i < shampions.Count; i++)
                {
                    if (shampions[i].Points < player.Points)
                    {
                        shampions.Insert(i, player);
                        shampions.RemoveAt(shampions.Count - 1);
                        break;
                    }
                }
            }
        }

        private static void PrintChart(List<Player> chart)
        {
            chart.Sort((Player r1, Player r2) => r2.Name.CompareTo(r1.Name));
            chart.Sort((Player r1, Player r2) => r2.Points.CompareTo(r1.Points));
            Console.WriteLine("\nTop players:");
            if (chart.Count > 0)
            {
                for (int i = 0; i < chart.Count; i++)
                {
                    Console.WriteLine("{0}. {1} --> {2} uncovered positions", i + 1, chart[i].Name, chart[i].Points);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Empty chart!\n");
            }
        }

        private static void ShowTheNumberOfBombsNextToThisField(char[,] gameBoard, char[,] bombs, int row, int col)
        {
            char numberOfBombs = GetTheNumberOfBombsNextToThisField(bombs, row, col);
            bombs[row, col] = numberOfBombs;
            gameBoard[row, col] = numberOfBombs;
        }

        private static void PrintGameBoard(char[,] board)
        {
            int row = board.GetLength(0);
            int col = board.GetLength(1);
            Console.WriteLine("\n    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");
            for (int i = 0; i < row; i++)
            {
                Console.Write("{0} | ", i);
                for (int j = 0; j < col; j++)
                {
                    Console.Write(string.Format("{0} ", board[i, j]));
                }

                Console.Write("|");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------\n");
        }

        private static char[,] CreateGameBoard()
        {
            int boardRows = 5;
            int boardColumns = 10;
            char[,] board = new char[boardRows, boardColumns];
            for (int i = 0; i < boardRows; i++)
            {
                for (int j = 0; j < boardColumns; j++)
                {
                    board[i, j] = '?';
                }
            }

            return board;
        }

        private static char[,] PutTheBombs()
        {
            int rows = 5;
            int columns = 10;
            char[,] gameBoard = new char[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    gameBoard[i, j] = '-';
                }
            }

            List<int> bombs = new List<int>();
            while (bombs.Count < 15)
            {
                Random random = new Random();
                int randomNumber = random.Next(50);
                if (!bombs.Contains(randomNumber))
                {
                    bombs.Add(randomNumber);
                }
            }

            foreach (int bomb in bombs)
            {
                int col = bomb / columns;
                int row = bomb % columns;
                if (row == 0 && bomb != 0)
                {
                    col--;
                    row = columns;
                }
                else
                {
                    row++;
                }

                gameBoard[col, row - 1] = '*';
            }

            return gameBoard;
        }

        // This method is unused
        //private static void smetki(char[,] pole)
        //{
        //    int kol = pole.GetLength(0);
        //    int red = pole.GetLength(1);

        //    for (int i = 0; i < kol; i++)
        //    {
        //        for (int j = 0; j < red; j++)
        //        {
        //            if (pole[i, j] != '*')
        //            {
        //                char kolkoo = GetTheNumberOfBombsNextToThisField(pole, i, j);
        //                pole[i, j] = kolkoo;
        //            }
        //        }
        //    }
        //}

        private static char GetTheNumberOfBombsNextToThisField(char[,] gameBoard, int row, int col)
        {
            int count = 0;
            int rows = gameBoard.GetLength(0);
            int cols = gameBoard.GetLength(1);

            if (row - 1 >= 0)
            {
                if (gameBoard[row - 1, col] == '*')
                {
                    count++;
                }
            }

            if (row + 1 < rows)
            {
                if (gameBoard[row + 1, col] == '*')
                {
                    count++;
                }
            }

            if (col - 1 >= 0)
            {
                if (gameBoard[row, col - 1] == '*')
                {
                    count++;
                }
            }

            if (col + 1 < cols)
            {
                if (gameBoard[row, col + 1] == '*')
                {
                    count++;
                }
            }

            if ((row - 1 >= 0) && (col - 1 >= 0))
            {
                if (gameBoard[row - 1, col - 1] == '*')
                {
                    count++;
                }
            }

            if ((row - 1 >= 0) && (col + 1 < cols))
            {
                if (gameBoard[row - 1, col + 1] == '*')
                {
                    count++;
                }
            }

            if ((row + 1 < rows) && (col - 1 >= 0))
            {
                if (gameBoard[row + 1, col - 1] == '*')
                {
                    count++;
                }
            }

            if ((row + 1 < rows) && (col + 1 < cols))
            {
                if (gameBoard[row + 1, col + 1] == '*')
                {
                    count++;
                }
            }

            return char.Parse(count.ToString());
        }
    }
}