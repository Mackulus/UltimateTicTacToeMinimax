using System;

namespace UltimateTicTacToeMinimax
{
    /// <summary>
    /// Stores a move.
    /// </summary>
    public class Move : IComparable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Score { get; set; }

        public Move()
        {
            X = -1;
            Y = -1;
            Score = 0;
        }
        public Move(int x, int y)
        {
            X = x;
            Y = y;
            Score = 0;
        }

        // Allow two moves to be compared
        public int CompareTo(object obj)
        {
            Move otherMove = obj as Move;
            if (otherMove != null)
                return Score.CompareTo(otherMove.Score);
            else
                return 0;
        }

        override public String ToString()
        {
            return String.Format("place_move {0} {1}", X, Y);
        }

    }
}
