using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateTicTacToeMinimax
{
    public class UltimateBoard
    {
        public const int Cols = 9;
        public const int Rows = 9;

        public enum GameStatus { XWon, OWon, Tie, MovesAvailable };
        
        public const char PlayerX = 'X';
        public const char PlayerO = 'O';
        public const char Tied = 'T';
        public const char Active = 'A';
        public const char Empty = '.';

        private char[,] board;        // X, O, or .
        private char[,] macroboard;   // X, O, T, A, or .


        public char[,] Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
            }
        }

        public char[,] Macroboard
        {
            get
            {
                return macroboard;
            }
            set
            {
                macroboard = value;
            }
        }
                      

        public UltimateBoard()
        {
            board = new char[Cols, Rows];
            macroboard = new char[Cols / 3, Rows / 3];
            ClearBoard();
        }

        public void ClearBoard()
        {
            for (int x = 0; x < Cols; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    board[x, y] = Empty;
                }
            }
        }

        public void SetBoard(int col, int row, char player)
        {
            board[col, row] = player;
        }

        public void SetMacroboard(int col, int row, char status)
        {
            // Empty status might actually be tied, so check first

            if (status == Empty && IsTieMicroBoard(board, col, row))
                macroboard[col, row] = Tied;
            else
                macroboard[col, row] = status;
        }

        public void ParseFromString(string s)
        {
            int counter = 0;
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    board[x, y] = s[counter];
                    counter++;

                    if (x == 2 || x == 5)
                    {
                        // Skip |
                        counter++;
                    }
                }

                // Skip |
                counter++;
            }

            ComputeMacroboard();
        }

        public void ParseMacroboardFromString(string s)
        {
            int counter = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    macroboard[x, y] = s[counter];
                    counter++;                    
                }

                // Skip |
                counter++;
            }
        }

        private void ComputeMacroboard()
        {
            // Set macroboard based on status of board
            // IS THIS NECESSARY when macroboard string can be set directly?
        }

        public List<Move> AvailableMoves
        {
            get
            {
                var moves = new List<Move>();

                for (int y = 0; y < Rows; y++)
                {
                    for (int x = 0; x < Cols; x++)
                    {
                        if (IsActiveMicroboard(x, y) && board[x, y] == Empty)
                        {
                            moves.Add(new Move(x, y));
                        }
                    }
                }

                return moves;
            }
        }

        public bool IsActiveMicroboard(int x, int y)
        {
            return macroboard[x / 3, y / 3] == Active;
        }

        // Make a move like the actual simulator
        public void MakeMove(Move move, char player)
        {
            if (board[move.X, move.Y] != Empty)
            {
                throw new ArgumentException("Move [" + move.X + "," + move.Y + "] cannot be placed in this board");
            }

            board[move.X, move.Y] = player;

            // Set macroboard active fields back to empty
            for (int x = 0; x < Cols / 3; x++)
            {
                for (int y = 0; y < Rows / 3; y++)
                {
                    if (macroboard[x, y] == Active)
                        macroboard[x, y] = Empty;
                }
            }

            var mbX = move.X / 3;
            var mbY = move.Y / 3;

            // See if legal move and what macroboards does it make active
            if (IsWinnerMicroBoard(board, mbX, mbY, player))
            {
                macroboard[mbX, mbY] = player;
            }
            else if (IsTieMicroBoard(board, mbX, mbY))
            {
                macroboard[mbX, mbY] = Tied;
            }

            int playX = move.X % 3;
            int playY = move.Y % 3;

            // If microboard is where next player should move is empty, it becomes active
            if (macroboard[playX, playY] == Empty)
            {
                macroboard[playX, playY] = Active;
            }
            else
            {
                // All macroboards are active if move is in finished board
                for (int x = 0; x < Cols / 3; x++)
                {
                    for (int y = 0; y < Rows / 3; y++)
                    {
                        if (macroboard[x, y] == Empty)
                            macroboard[x, y] = Active;
                    }
                }
            }
        }

        public bool IsTieMicroBoard(char[,] board, int col, int row)
        {
            for (int x = col * 3; x < col * 3 + 3; x++)
            {
                for (int y = row * 3; y < row * 3 + 3; y++)
                {
                    if (board[x, y] == Empty)
                    {
                        return false;
                    }
                }
            }

            // All spots in small board are taken
            return true;
        }


        // Return true if the player has won the game
        public bool IsWinnerMicroBoard(char[,] board, int col, int row, char player)
        {
            if (col < 0 || col > 2)
                throw new ArgumentException("col must be 0-2.");
            if (row < 0 || row > 2)
                throw new ArgumentException("row must be 0-2.");

            int x = col * 3;
            int y = row * 3;

            return (board[x, y] == player && board[x + 1, y] == player && board[x + 2, y] == player) ||
                   (board[x, y + 1] == player && board[x + 1, y + 1] == player && board[x + 2, y + 1] == player) ||
                   (board[x, y + 2] == player && board[x + 1, y + 2] == player && board[x + 2, y + 2] == player) ||
                   (board[x, y] == player && board[x, y + 1] == player && board[x, y + 2] == player) ||
                   (board[x + 1, y] == player && board[x + 1, y + 1] == player && board[x + 1, y + 2] == player) ||
                   (board[x + 2, y] == player && board[x + 2, y + 1] == player && board[x + 2, y + 2] == player) ||
                   (board[x, y] == player && board[x + 1, y + 1] == player && board[x + 2, y + 2] == player) ||
                   (board[x + 2, y] == player && board[x + 1, y + 1] == player && board[x, y + 2] == player);
        }

        private bool IsWinner(char[,] macroboard, char player)
        {
            return IsWinnerMicroBoard(macroboard, 0, 0, player);
        }

        public GameStatus GetGameStatus()
        {
            if (IsWinner(macroboard, PlayerX))
                return GameStatus.XWon;
            else if (IsWinner(macroboard, PlayerO))
                return GameStatus.OWon;
            else if (AvailableMoves.Count == 0)
                return GameStatus.Tie;
            else
                return GameStatus.MovesAvailable;
        }

        public char GetPlayer(int x, int y)
        {
            return board[x, y];
        }

        // Score is number of X wins minus O wins
        public int GetScore()
        {
            int score = 0;

            for (int y = 0; y < UltimateBoard.Rows / 3; y++)
            {
                for (int x = 0; x < UltimateBoard.Cols / 3; x++)
                {
                    if (IsWinnerMicroBoard(board, x, y, PlayerX))
                        score++;
                    else if (IsWinnerMicroBoard(board, x, y, PlayerO))
                        score--;
                }
            }

            return score;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Macroboard:\n");
            for (int y = 0; y < UltimateBoard.Rows / 3; y++)
            {
                for (int x = 0; x < UltimateBoard.Cols / 3; x++)
                {
                    sb.Append(macroboard[x, y]);                    
                }
                sb.Append("\n");
            }

            sb.Append("\n-----------\n");

            for (int y = 0; y < UltimateBoard.Rows; y++)
            {
                for (int x = 0; x < UltimateBoard.Cols; x++)
                {
                    sb.Append(GetPlayer(x, y));

                    if ((x + 1) % 3 == 0)
                        sb.Append("|");
                }

                if ((y + 1) % 3 == 0)
                    sb.Append("\n-----------\n");
                else
                    sb.Append("\n");
            }

            return sb.ToString();
        }
        
    }
}
