using System;
using System.Collections.Generic;
using System.Text;
using Virus_Ultimate.Enums;

namespace Virus_Ultimate.Data
{
    public class Round
    {
        public int RoundNumber;
        public RoundType Type;
        public int Limit;

        public Round(int num, RoundType type, int limit)
        {
            RoundNumber = num;
            Type = type;
            Limit = limit;
        }
    }
}
