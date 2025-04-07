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
                // Actualizăm scorul și numărul de meciuri
                player.Score += scoreIncrement;
                player.MatchPlayed += matchesPlayedIncrement;

                // Verifică valorile înainte de a salva
                Console.WriteLine($"Updated Player: {player.Name} | Score: {player.Score} | Matches: {player.MatchPlayed}");
                var index = Players.IndexOf(player);
                if (index >= 0)
                {
                    // Înlocuiește obiectul vechi cu cel modificat
                    Players[index] = player;
                }
                // Salvăm jucătorii actualizați
                SavePlayers();

                // Notificăm UI-ul că lista de jucători s-a schimbat
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

