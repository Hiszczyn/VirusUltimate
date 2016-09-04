using System;
using System.Collections.Generic;
using System.Text;

namespace Virus_Ultimate.Data
{
    public class Board
    {
        public List<Square> Squares;
        public int Infected;
        public Round RoundObject;
        public int DoneMoves;
        public int Outcluded;
        public int Time;
        public int BestMove;
    }
}
