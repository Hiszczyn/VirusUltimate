using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virus_Ultimate.Data;
using Windows.UI;

namespace Virus_Ultimate.Services
{
    public class BoardService
    {
        public Board _board;
        #region Maps
        public readonly Dictionary<int, string> _squareName = new Dictionary<int, string>()
        {
            [0] = "A",
            [1] = "B",
            [2] = "C",
            [3] = "D",
            [4] = "E",
            [5] = "F",
            [6] = "G",
            [7] = "H",
            [8] = "I",
            [9] = "J",
            [10] = "K",
            [11] = "L",
            [12] = "M",
            [13] = "N",
            [14] = "O",
            [15] = "P",
            [16] = "R",
            [17] = "S",
            [18] = "T",
            [19] = "U"
        };

        private Round[] _rounds = {
                new Round(1,0,45),
                new Round(2,0,42),
                new Round(3,0,40),
                new Round(4,0,39),
                new Round(5,0,38),
            };

        public readonly Dictionary<int, Color> _squareColor = new Dictionary<int, Color>()
        {
            [0] = Colors.Blue,
            [1] = Colors.Red,
            [2] = Colors.Green,
            [3] = Colors.Yellow,
            [4] = Colors.Purple,
            [5] = Colors.LightGray
        };

        public readonly Dictionary<Color,int> _squareColorNumber = new Dictionary<Color, int>()
        {
            [Colors.Blue] = 0,
            [Colors.Red] = 1,
            [Colors.Green] = 2,
            [Colors.Yellow] = 3,
            [Colors.Purple] = 4,
            [Colors.LightGray] =5
        };
        #endregion

        public BoardService()
        {
            _board = setNewBoard();
        }

        private Board setNewBoard()
        {
            Board board = new Board();
            board.Squares = RandomSquares();
            board.DoneMoves = 0;
            return board;
        }

        private List<Square> RandomSquares()
        {
            List<Square> squares = new List<Square>();
            Square sq;
            Random random = new Random();
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                {
                    sq = new Square();
                    sq.Column = i;
                    sq.Row = j;
                    sq.Color = random.Next(0, 6);
                    sq.Status = 1;
                    squares.Add(sq);
                }
            return squares;
        }

        private bool checkEverythingOnce(int nrOfColor)
        {
            bool anyChange = false;
            List<Square> neighbours = _board.Squares.Where(s => s.Status > 0 && s.Color == nrOfColor).ToList<Square>();
            foreach (var squareToChange in _board.Squares.Where(s => s.Status > 0 && s.Color == nrOfColor))
            {
                if (haveInfectedNeighbour(squareToChange.Column, squareToChange.Row))
                {
                    squareToChange.Status = 0;
                    squareToChange.Color = nrOfColor;
                    anyChange = true;
                }
            }
            return anyChange;
        }

        private bool haveInfectedNeighbour(int col, int row)
        {
            List<Square> neighbours = new List<Square>();
            if (col < 19 &&
                _board.Squares.Where(s => s.Status == 0 && s.Column == col + 1 && s.Row == row).ToList().Any())
                return true;
            if (col > 0 &&
                _board.Squares.Where(s => s.Status == 0 && s.Column == col - 1 && s.Row == row).ToList().Any())
                return true;
            if (row < 19 &&
               _board.Squares.Where(s => s.Status == 0 && s.Column == col && s.Row == row + 1).ToList().Any())
                return true;
            if (row > 0 &&
               _board.Squares.Where(s => s.Status == 0 && s.Column == col && s.Row == row - 1).ToList().Any())
                return true;
            return false;
        }

        private void changeInfectedColors(int changeColor)
        {
            foreach (var infected in _board.Squares.Where(s => s.Status == 0).ToList())
            {
                infected.Color = changeColor;
            }
        }

        private void setPatient0()
        {
            switch (_board.RoundObject.RoundType)
            {
                case 0:
                    _board.Squares.Where(s => s.Column == 0 && s.Row == 0).ToList().First().Status = 0;
                    _board.Outcluded = 0;
                    break;
                default:
                    break;
            }
        }

        private void updateBoardStatistics()
        {
            _board.Infected = _board.Squares.Where(s => s.Status == 0).Count();
            _board.DoneMoves++;
        }

        private void countBoardResults()
        {
            int newInfectedNr = _board.Squares.Where
                (s => s.Status == 0).Count();

            if (newInfectedNr - _board.Infected > _board.BestMove)
                _board.BestMove = newInfectedNr - _board.Infected;
            _board.Infected = newInfectedNr;
        }

        public void play(int nrOfColor)
        {
            while (checkEverythingOnce(nrOfColor)) { }
            changeInfectedColors(nrOfColor);
            countBoardResults();
            updateBoardStatistics();

        }

        public void resetBoard(int round)
        {
            _board = setNewBoard();
            _board.RoundObject = _rounds[round - 1];
            setPatient0();
        }

        public bool hasWon()
        {
            return (_board.Infected == 400 - _board.Outcluded);
        }

        public bool hasLost()
        {
            return (!(_board.DoneMoves < _board.RoundObject.Limit));
        }

    }
}
