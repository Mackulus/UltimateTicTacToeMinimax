using System;
using System.Collections.Generic;
using System.Linq;

namespace UltimateTicTacToeMinimax.Bot
{
    public class SmartBot
    {
        private const bool Debug = true;

        private Random rand = new Random();

        /// <summary>
        /// Returns the next move to make. Edit this method to make your bot smarter.
        /// Currently does only random moves.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>The column where the turn was made</returns>
        public Move GetMove(BotState state)
        {
            //var moves = state.UltimateBoard.AvailableMoves;

            //if (moves.Count > 0)
            //{
            //    // get random move from available moves
            //    return moves[rand.Next(moves.Count)]; 
            //    //return moves.First();               
            //}

            //// pass
            //return null;   

            char player;
            if (state.Field.MyId == 0)
            {
                player = 'X';
            }
            else
            {
                player = 'O';
            }
            var move = Minimax(state, player, 0);
            return move;
        }

        private Move Minimax(BotState state, char player, int level)
        {
            var gameState = state.UltimateBoard.GetGameStatus();
            if (gameState == UltimateBoard.GameStatus.OWon)
            {
                return new Move { Score = -10 };
            }
            else if (gameState == UltimateBoard.GameStatus.XWon)
            {
                return new Move { Score = 10 };
            }
            else if (gameState == UltimateBoard.GameStatus.Tie)
            {
                return new Move { Score = 0 };
            }

            if (state.UltimateBoard.Board[4, 4] == '.')
            {
                return new Move(4, 4);
            }

            if (level == 5)
            {
                var score = state.UltimateBoard.GetScore();
                return new Move { Score = score };
            }

            var moves = state.UltimateBoard.AvailableMoves;

            foreach (var move in moves)
            {
                var board = (char[,])state.UltimateBoard.Board.Clone();
                var macroboard = (char[,])state.UltimateBoard.Macroboard.Clone();
                state.UltimateBoard.MakeMove(move, player);

                Console.WriteLine(state.UltimateBoard);
                if (player == UltimateBoard.PlayerX)
                {
                    move.Score = Minimax(state, UltimateBoard.PlayerO, level + 1).Score;
                }
                else
                {
                    move.Score = Minimax(state, UltimateBoard.PlayerX, level + 1).Score;
                }

                state.UltimateBoard.Board = board;
                state.UltimateBoard.Macroboard = macroboard;
            }

            if (player == UltimateBoard.PlayerX)
            {
                return moves.Max();
            }
            else
            {
                return moves.Min();
            }
        }

        static void Main(string[] args)
        {
            BotParser parser = new BotParser(new SmartBot());
            parser.Run();
        }
    }
}