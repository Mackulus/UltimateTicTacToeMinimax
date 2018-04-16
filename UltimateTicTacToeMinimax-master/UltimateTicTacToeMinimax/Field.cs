using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateTicTacToeMinimax
{
    /// <summary>
    /// Handles everything that has to do with the field, such as storing 
    /// the current state and performing calculations on the field.
    /// </summary>
    public class Field
    {
        public const String EmptyField = ".";
        public const String PlayerField = "0";
        public const String OpponentField = "1";
        public const String AvailableField = "-1";

        public const int Cols = 9;
        public const int Rows = 9;

        public int MyId { get; set; }
        public int OpponentId { get; set; }
        
        private string[,] board;
        private string[,] macroboard;
        private UltimateBoard ultimateBoard;        

        public Field(UltimateBoard ultimateBoard)
        {
            board = new string[Cols, Rows];
            macroboard = new string[Cols / 3, Rows / 3];
            this.ultimateBoard = ultimateBoard;
            ClearBoard();
        }

        /// <summary>
        /// Initialize field containing board from comma separated string
        /// </summary>
        /// <param name="s"></param>
        public void ParseFromString(String s)
        {
            s = s.Replace(";", ",");
            String[] r = s.Split(',');
            int counter = 0;
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    board[x, y] = r[counter];
                    counter++;

                    if (board[x, y] == PlayerField)
                        ultimateBoard.SetBoard(x, y, UltimateBoard.PlayerO);
                    else if (board[x, y] == OpponentField)
                        ultimateBoard.SetBoard(x, y, UltimateBoard.PlayerX);
                    else if (board[x, y] == EmptyField)
                        ultimateBoard.SetBoard(x, y, UltimateBoard.Empty);
                }
            }
            
        }

        /// <summary>
        /// Initialize macroboard from comma separated string
        /// </summary>
        /// <param name="s"></param>
        public void ParseMacroboardFromString(String s)
        {
            String[] r = s.Split(',');
            int counter = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    macroboard[x, y] = r[counter];
                    counter++;

                    if (macroboard[x, y] == PlayerField)
                        ultimateBoard.SetMacroboard(x, y, UltimateBoard.PlayerO);
                    else if (macroboard[x, y] == OpponentField)
                        ultimateBoard.SetMacroboard(x, y, UltimateBoard.PlayerX);
                    else if (macroboard[x, y] == EmptyField)
                        ultimateBoard.SetMacroboard(x, y, UltimateBoard.Empty);
                    else if (macroboard[x, y] == AvailableField)
                        ultimateBoard.SetMacroboard(x, y, UltimateBoard.Active);
                }
            }
        }

        public void ClearBoard()
        {
            for (int x = 0; x < Cols; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    board[x, y] = EmptyField;
                }
            }
        }
        
        public bool IsActiveMicroboard(int x, int y)
        {
            return macroboard[x / 3, y / 3] == AvailableField;
        }
        

        /// <summary>
        /// Creates comma separated String with player ids for the microboards.
        /// </summary>
        /// <returns>String with player names for every cell, or 'empty' when cell is empty.</returns>
        override public String ToString()
        {
            var r = new StringBuilder("");
            int counter = 0;
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    if (counter > 0)
                    {
                        r.Append(",");
                    }
                    r.Append(board[x, y]);
                    counter++;
                }
            }
            return r.ToString();
        }    


        /// <summary>
        /// Checks whether the field is full
        /// </summary>
        /// <returns>Returns true when field is full, otherwise returns false</returns>
        public bool IsFull()
        {
            for (int x = 0; x < Cols; x++)
                for (int y = 0; y < Rows; y++)
                    if (board[x, y] == EmptyField)
                        return false; // At least one cell is not filled

            // All cells are filled
            return true;
        }
        
        public bool IsEmpty()
        {
            for (int x = 0; x < Cols; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (board[x, y] != EmptyField)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
                 
    }
}
