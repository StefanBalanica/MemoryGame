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

