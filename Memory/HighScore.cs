using System;
using System.Collections.Generic;
using System.Text;

namespace Memory
{
    /// <summary>
    /// Holds the high score for a specific game
    /// </summary>
    [Serializable]
    public class HighScore : IComparable<HighScore>
    {
        public string Initials; //person who played
        public int Score;       //the score they achieved

        public HighScore(string initials, int score)
        {
            Initials = initials;
            Score = score;
        }

        //sorts high scores in ascending order
        public int CompareTo(HighScore other)
        {
            if (other == null)
                return -1;
            if (Score < other.Score)
                return -1;
            if (Score > other.Score)
                return 1;
            return 0;
        }
    }
}
