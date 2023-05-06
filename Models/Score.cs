using System;

namespace MemoryGame.Models
{
    public class Score
    {
        public DateTime Date { get; set; }

        public string? PlayerName { get; set; }

        public TimeSpan TimeScore { get; set; }
    }
}
