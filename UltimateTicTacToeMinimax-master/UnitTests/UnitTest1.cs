using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateTicTacToeMinimax;
using UltimateTicTacToeMinimax.Bot;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        const string PlayerX = "1";
        const string PlayerO = "0";

        [TestMethod]
        public void Test_Field_IsActiveMicroboard()
        {
            var board = new UltimateBoard();
            var field = new Field(board);
            field.ParseFromString(".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.");
            field.ParseMacroboardFromString("-1,-1,-1,-1,-1,-1,-1,-1,-1");

            var moves = board.AvailableMoves;
            Assert.IsTrue(moves.Count == UltimateBoard.Rows * UltimateBoard.Cols);
        }

        [TestMethod]
        public void Test_Board_IsActiveMicroboard()
        {
            var board = new UltimateBoard();
            var macroboard = "A..|...|...";

            board.ParseMacroboardFromString(macroboard);

            for (int y = 0; y < Field.Rows; y++)
            {
                for (int x = 0; x < Field.Cols; x++)
                {
                    if (x >= 0 && x <= 2 && y >= 0 && y <= 2)
                    {
                        Assert.IsTrue(board.IsActiveMicroboard(x, y));
                    }
                    else
                    {
                        Assert.IsFalse(board.IsActiveMicroboard(x, y), 
                            "Should be false for microboard [" + x + "," + y + "]");
                    }
                }
            }
        }

        [TestMethod]
        public void Test_Board_IsWinnerMicroBoard()
        {
            var boardString = "XOX|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);
            var results = "TXO|XO?|XOX";

            int count = 0;
            for (int y = 0; y < UltimateBoard.Rows / 3; y++)
            {
                for (int x = 0; x < UltimateBoard.Cols / 3; x++)
                {
                    string help = "[" + x + "," + y + "] winner should be is " + results[count];

                    if (results[count] == 'T' || results[count] == '?')
                    {
                        // No winner if tied or undecided
                        Assert.IsFalse(board.IsWinnerMicroBoard(board.Board, x, y, UltimateBoard.PlayerX), help);
                        Assert.IsFalse(board.IsWinnerMicroBoard(board.Board, x, y, UltimateBoard.PlayerO), help);
                    }
                    else if (results[count] == 'X')
                    {
                        Assert.IsTrue(board.IsWinnerMicroBoard(board.Board, x, y, UltimateBoard.PlayerX), help);
                        Assert.IsFalse(board.IsWinnerMicroBoard(board.Board, x, y, UltimateBoard.PlayerO), help);
                    }
                    else if (results[count] == 'O')
                    {
                        Assert.IsFalse(board.IsWinnerMicroBoard(board.Board, x, y, UltimateBoard.PlayerX), help);
                        Assert.IsTrue(board.IsWinnerMicroBoard(board.Board, x, y, UltimateBoard.PlayerO), help);
                    }
                    count++;
                }

                // Skip vertical bar
                count++;
            }
        }

        
        [TestMethod]
        public void Text_Board_IsTieMicroBoard()
        {
            var boardString = "XOX|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);

            for (int y = 0; y < Field.Rows / 3; y++)
            {
                for (int x = 0; x < Field.Cols / 3; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        Assert.IsTrue(board.IsTieMicroBoard(board.Board, x, y), 
                            "[" + x + "," + y + "]");
                    }
                    else
                    {
                        Assert.IsFalse(board.IsTieMicroBoard(board.Board, x, y),
                            "[" + x + "," + y + "]");
                    }
                }
            }           
        }

        
        [TestMethod]
        public void Test_Board_MakeMove_PlayerXWins()
        {
            // Make winning move in upper-left microboard
            var boardString = "X.X|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);
            
            // Win the microboard
            var move = new Move(1, 0);
            board.MakeMove(move, UltimateBoard.PlayerX);

            // X won upper-left corner of macroboard
            Assert.IsTrue(board.Macroboard[0, 0] == UltimateBoard.PlayerX);        
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Move should not be allowed in non-empty position")]
        public void Test_Board_MakeMove_OccupiedPosition()
        {
            var boardString = "XOX|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);

            // This position is occupied
            var move = new Move(1, 0);
            board.MakeMove(move, UltimateBoard.PlayerX);
        }

        [TestMethod]
        public void Test_Board_MakeMove_TieMove()
        {
            var boardString = "X.X|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);

            // Tie the upper-left corner of microboard
            var move = new Move(1, 0);
            board.MakeMove(move, UltimateBoard.PlayerO);
            Assert.IsTrue(board.Macroboard[0, 0] == UltimateBoard.Tied);
        }

        [TestMethod]
        public void Test_Board_GameStatus_MoreMovesAvailable()
        {
            var boardString = "XOX|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var macroboard = "TXO|XOA|XOX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);
            board.ParseMacroboardFromString(macroboard);

            // More moves available in center-right of microboard
            Assert.IsTrue(board.GetGameStatus() == UltimateBoard.GameStatus.MovesAvailable);
        }

        [TestMethod]
        public void Test_Board_GameStatus_Tie()
        {
            var boardString = "XOX|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OOX|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var macroboard = "TXO|XOX|XOX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);
            board.ParseMacroboardFromString(macroboard);

            // Tie
            Assert.IsTrue(board.GetGameStatus() == UltimateBoard.GameStatus.Tie);
        }

        [TestMethod]
        public void Test_Board_GameStatus_XWon()
        {
            var boardString = "XXX|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var macroboard = "XXO|XOT|XOX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);
            board.ParseMacroboardFromString(macroboard);

            // Tie
            Assert.IsTrue(board.GetGameStatus() == UltimateBoard.GameStatus.XWon);
        }

        [TestMethod]
        public void Test_Board_GameStatus_OWon()
        {
            var boardString = "XXX|OOO|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var macroboard = "TOO|XOT|XOX";
            var board = new UltimateBoard();
            board.ParseFromString(boardString);
            board.ParseMacroboardFromString(macroboard);

            // Tie
            Assert.IsTrue(board.GetGameStatus() == UltimateBoard.GameStatus.OWon);
        }

        [TestMethod]
        public void Minimax_Xwin()
        {
            var boardString = "X.X|XXX|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var macroboard = "AXO|XOA|XOX";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 0;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            // X should be placed in top-left microboard so X wins macroboard on vert left
            Move move = bot.GetMove(state);

            // More moves available in center-right of microboard
            Assert.IsTrue(move.X == 1 && move.Y == 0, "Actual move is " + move);
        }

        [TestMethod]
        public void Minimax_Owin()
        {
            var boardString = "X.O|OO.|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|XOO|XXX|OX.|.XX|.O.|O..|OO.|O..|OOO|.OX|XXX|...|...|..O|XXO|XXX";
            var macroboard = "AXO|XO.|XOX";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 0;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            // X should be placed in top-left microboard since it's the only spot
            // open, then O should win top-middle microboard to win macroboard vert middle
            Move move = bot.GetMove(state);

            // More moves available in center-right of microboard
            Assert.IsTrue(move.X == 1 && move.Y == 0, "Actual move is " + move);
        }

        [TestMethod]
        public void Minimax_Xwin2()
        {
            var boardString = "OOO|OO.|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|.OO|XXX|OX.|OXX|.O.|O..|OXO|XOO|XXX|O.O|XX.|...|OX.|O.X|XXO|XXO";
            var macroboard = "OOX|TO.|AX.";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 0;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            Console.Write(state.UltimateBoard);

            // X should be placed in [2,7] to win [0,2] microboard
            Move move = bot.GetMove(state);

            // More moves available in center-right of microboard
            Assert.IsTrue(move.X == 2 && move.Y == 7, "Actual move is " + move);
        }

        [TestMethod]
        public void Minimax_XLose()
        {
            var boardString = "OOO|OO.|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|.OO|XXX|OX.|OXX|.O.|O..|OXO|XOO|XXX|O.O|X.X|...|OXX|O.X|XXO|XXO";
            var macroboard = "OOX|TO.|AX.";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 0;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            Console.Write(state.UltimateBoard);

            // X can't win, so [1,7] is chosen first
            Move move = bot.GetMove(state);

            // X loses
            Assert.IsTrue(move.X == 1 && move.Y == 7 && move.Score == -10, 
                "Actual move is " + move + " and score = " + move.Score);
        }

        [TestMethod]
        public void Minimax_XLose2()
        {
            var boardString = "OOO|OO.|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|.OO|XXX|OX.|OXX|.O.|O..|OXO|XOO|XXX|O.O|X.X|...|OXX|O.X|XXO|XXO";
            var macroboard = "OOX|TOA|.X.";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 0;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            Console.Write(state.UltimateBoard);

            // X can't win, so [1,7] is chosen first
            Move move = bot.GetMove(state);

            // X loses
            Assert.IsTrue(move.X == 6 && move.Y == 3 && move.Score == -10,
                "Actual move is " + move + " and score = " + move.Score);
        }

        [TestMethod]
        public void Minimax_Tie()
        {
            var boardString = "OOO|OO.|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|.OO|XXX|OX.|OXX|.O.|O..|OXO|XOO|XXX|O.O|X.X|...|OXX|O.X|XXO|XXO";
            var macroboard = "OOX|TO.|.XA";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 0;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            Console.Write(state.UltimateBoard);

            Move move = bot.GetMove(state);

            // Tie
            Assert.IsTrue(move.X == 7 && move.Y == 6 && move.Score == 0,
                "Actual move is " + move + " and score = " + move.Score);
        }

        [TestMethod]
        public void Minimax_XWin()
        {
            var boardString = "OOO|OO.|.XX|OXX|O..|OOO|OXO|..O|.OO|OXX|OOO|.OO|XXX|OX.|OXX|.O.|O..|OXO|XOO|XXX|O.O|X.X|...|OXX|O.X|XXO|XXO";
            var macroboard = "OOX|TOX|AXA";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 0;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            Console.Write(state.UltimateBoard);

            Move move = bot.GetMove(state);

            // X wins
            Assert.IsTrue(move.X == 7 && move.Y == 6 && move.Score == 10,
                "Actual move is " + move + " and score = " + move.Score);
        }

        [TestMethod]
        public void GetMove_FirstMoveBlankBoard()
        {
            var boardString = "...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...|...";
            var macroboard = "AAA|AAA|AAA";

            var bot = new SmartBot();
            var state = new BotState();

            state.Field.MyId = 1;
            state.UltimateBoard.ParseFromString(boardString);
            state.UltimateBoard.ParseMacroboardFromString(macroboard);

            Console.Write(state.UltimateBoard);

            Move move = bot.GetMove(state);

            // X wins
            Assert.IsTrue(move.X == 7 && move.Y == 6 && move.Score == 10,
                "Actual move is " + move + " and score = " + move.Score);
        }
    }

   
}
