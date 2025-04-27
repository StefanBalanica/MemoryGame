using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memory_game
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Player> Players { get; set; }

        private Player selectedPlayer;
        public Player SelectedPlayer
        {
            get => selectedPlayer;
            set
            {
                selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        public MainWindowViewModel()
        {
            Players = new ObservableCollection<Player>(PlayerManager.LoadPlayers());
        }

        public void SavePlayers()
        {
            PlayerManager.SavePlayers(new List<Player>(Players));
        }
        public void UpdatePlayerStats(Player player, int scoreIncrement, int matchesPlayedIncrement)
        {
            if (player != null)
            {
                player.Score += scoreIncrement;
                player.MatchPlayed += matchesPlayedIncrement;

                Console.WriteLine($"Updated Player: {player.Name} | Score: {player.Score} | Matches: {player.MatchPlayed}");
                var index = Players.IndexOf(player);
                if (index >= 0)
                {
                    Players[index] = player;
                }
                SavePlayers();

                OnPropertyChanged(nameof(Players));
            }
        }



        public void DeletePlayer()
        {
            if (SelectedPlayer != null)
            {
                Players.Remove(SelectedPlayer);
                SavePlayers();
                SelectedPlayer = null;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

