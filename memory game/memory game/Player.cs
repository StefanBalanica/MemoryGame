using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memory_game
{
    public class Player
    {
        public string Name { get; set; }
        public int MatchPlayed { get; set; }
        public int Score { get; set; }
        public string AvatarPath { get; set; }

        public Player(string name, int score = 0, int matchPlayed=0, string avatarPath = "")
        {
            Name = name;
            Score = score;
            MatchPlayed = matchPlayed;
            AvatarPath = avatarPath;
        }

        public void ResetScore()
        {
            Score = 0;
        }

        public override string ToString()
        {
            return $"Player: {Name}, Score: {Score}, Avatar: {AvatarPath}";
        }
    }
}

