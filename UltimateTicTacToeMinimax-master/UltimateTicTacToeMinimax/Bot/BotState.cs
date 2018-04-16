using System;
using System.Collections.Generic;

namespace UltimateTicTacToeMinimax.Bot
{
    /// <summary>
    ///  This class stores all settings of the game and the information about the current state of the game.
    ///  When calling this in BotStarter.doMove(), you can trust that this state has been update to current 
    ///  game state (because updates are sent before action request).
    /// </summary>
    public class BotState
    {
        public int RoundNumber { get; set; }
        public int Timebank { get; set; }
        public int TimePerMove { get; set; }
        public int MaxTimebank { get; set; }        
        public string MyName { get; set; }    // 0 or 1
        public int MoveNumber { get; set; }                
        public Dictionary<string, Player> Players { get; set; }
        public Field Field { get; set; }
        public UltimateBoard UltimateBoard { get; set; }

        public BotState()
        {
            UltimateBoard = new UltimateBoard();
            Field = new Field(UltimateBoard);
            Players = new Dictionary<string, Player>();
        }
    }
}
