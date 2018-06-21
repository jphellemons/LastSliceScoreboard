using System;
using System.Collections.Generic;
using System.Text;

namespace LastSliceScoreboard
{
    public class Score
    {
        public string Initials { get; set; }
        public int ChallengesCompleted { get; set; }

        public override string ToString()
        {
            return $"{Initials} completed {ChallengesCompleted} challenges";
        }
    }
}
