using System;
using System.Collections.Generic;
using System.Text;

namespace Virus_Ultimate.Data
{
    public class Round
    {
        public int RoundNumber;
        public int RoundType;
        public int Limit;

        public Round(int num, int type, int limit)
        {
            RoundNumber = num;
            RoundType = type;
            Limit = limit;
        }
    }
}
