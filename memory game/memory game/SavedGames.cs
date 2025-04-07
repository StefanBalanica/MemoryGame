using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memory_game
{
    public class SavedGames
    {

        public string PlayerName { get; set; }
        public int TimeLeft { get; set; }
        public bool IsTimerRunning { get; set; }
        public List<int> MatchedCards { get; set; }
        public List<int> SelectedCards { get; set; }
        public List<string> GameImages { get; set; }
        public List<string> GridButtonStates { get; set; }
        public int GridSizeRow { get; set; }
        public int GridSizeCol { get; set; }

        public SavedGames()
        {
            MatchedCards = new List<int>();
            SelectedCards = new List<int>();
            GameImages = new List<string>();
            GridButtonStates = new List<string>();
        }
    }
}
