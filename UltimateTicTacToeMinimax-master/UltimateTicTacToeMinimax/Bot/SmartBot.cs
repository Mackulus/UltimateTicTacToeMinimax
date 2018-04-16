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
            var moves = state.UltimateBoard.AvailableMoves;

            if (moves.Count > 0)
            {
                // get random move from available moves
                return moves[rand.Next(moves.Count)]; 
                //return moves.First();               
            }

            // pass
            return null;       
        }
                
        static void Main(string[] args)
        {
            BotParser parser = new BotParser(new SmartBot());
            parser.Run();
        }
    }
}
