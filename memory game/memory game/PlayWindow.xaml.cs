using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace memory_game
{
    /// <summary>
    /// Interaction logic for PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window
    {
        private MainWindowViewModel _viewModel;
        private readonly Player currentPlayer;
        private List<string> imagePaths;
        private List<Button> gameButtons;
        private List<int> selectedCards;
        private int gridSizeRow = 4, gridSizeCol=4 ;
        private List<int> matchedCards = new List<int>();
        public int gameTime;
        public string selectedCategory;
        public Player CurrentPlayer => currentPlayer;

        private DispatcherTimer _timer;
        private int _timeLeft;
        private bool isTimerRunning = false;

        public PlayWindow(MainWindowViewModel viewModel, Player SelectedPlayer)
        {
            InitializeComponent();
            _viewModel = viewModel;
            currentPlayer = SelectedPlayer;
            imagePaths = new List<string>();
            gameButtons = new List<Button>();
            selectedCards = new List<int>();
            this.DataContext = _viewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); 
            _timer.Tick += Timer_Tick;
        
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeLeft--; 

            if (_timeLeft <= 0 || ceckWin())
            {
                _timer.Stop();
                if (ceckWin())
                {
                    _viewModel.UpdatePlayerStats(currentPlayer, 1, 1);
                    MessageBox.Show($"Ai castigat! Jocul s-a încheiat!");
                }
                else
                {
                    _viewModel.UpdatePlayerStats(currentPlayer, 0, 1);
                    MessageBox.Show("Timpul a expirat! Jocul s-a încheiat.");
                }
                EndGame(); 
            }

            TimerLabel.Content = TimeSpan.FromSeconds(_timeLeft).ToString(@"mm\:ss");
        }

        private void StartGame(int _gameTimeInSeconds)
        {
            PlayerNameTimerGrid.Visibility = Visibility.Visible;
            Dificulty.Visibility = Visibility.Collapsed;
            _timeLeft = _gameTimeInSeconds;
            gameTime= _gameTimeInSeconds;
            _timer.Start();
            isTimerRunning = true;


            LoadImages((gridSizeRow*gridSizeCol)/2);
        }

        private void EndGame()
        {
            foreach (var button in gameButtons)
            {
                button.IsEnabled = false;
            }

            imagePaths.Clear();
            gameButtons.Clear();
            selectedCards.Clear();
            matchedCards.Clear();
        }



        

        private void Category_Click(object sender, RoutedEventArgs e)
          {
            StatisticGrid.Visibility = Visibility.Hidden;
            Category1MenuItem.Background = null;
            Category2MenuItem.Background = null;
            Category3MenuItem.Background = null;

             var selectedMenuItem = sender as MenuItem;
            if (selectedMenuItem != null)
            {
                selectedMenuItem.Background = new SolidColorBrush(Colors.LightBlue);
                selectedCategory = selectedMenuItem.Header.ToString();
            }
}

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {

            StatisticGrid.Visibility = Visibility.Hidden;
            Dificulty.Visibility = Visibility.Visible;

        }

        private void OpenGameMenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            StatisticGrid.Visibility = Visibility.Hidden;
            var menuItem = sender as MenuItem;

            if (menuItem == null)
                return;

            menuItem.Items.Clear(); // Curățăm orice era înainte

            string folderPath = "SavedGames";
            if (!Directory.Exists(folderPath))
                return;

            string[] files = Directory.GetFiles(folderPath, "*.json");
            var playerFiles = files.Where(f => System.IO.Path.GetFileName(f).StartsWith(currentPlayer.Name)).ToList();

            if (playerFiles.Count == 0)
            {
                MenuItem emptyItem = new MenuItem
                {
                    Header = "Nicio salvare disponibilă",
                    IsEnabled = false
                };
                menuItem.Items.Add(emptyItem);
                return;
            }

            foreach (var file in playerFiles)
            {
                string fileName = System.IO.Path.GetFileName(file);
                MenuItem saveItem = new MenuItem
                {
                    Header = fileName,
                    Tag = file
                };
                saveItem.Click += LoadSavedGame_Click;
                menuItem.Items.Add(saveItem);
            }
        }
        private void LoadSavedGame_Click(object sender, RoutedEventArgs e)
        {
            StatisticGrid.Visibility = Visibility.Hidden;
            if (sender is MenuItem item && item.Tag is string filePath)
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    SavedGames gameState = JsonConvert.DeserializeObject<SavedGames>(json);

                    currentPlayer.Name = gameState.PlayerName;
                    _timeLeft = gameState.TimeLeft;
                    isTimerRunning = gameState.IsTimerRunning;
                    matchedCards = new List<int>(gameState.MatchedCards);
                    selectedCards = new List<int>(gameState.SelectedCards);
                    gridSizeRow = gameState.GridSizeRow;
                    gridSizeCol = gameState.GridSizeCol;

                    CreateGameBoard(gameState.GameImages);

                    if (gameButtons.Count != gameState.GridButtonStates.Count)
                    {
                        MessageBox.Show("Fișierul de salvare este corupt sau incompatibil.");
                        return;
                    }

                    for (int i = 0; i < gameButtons.Count; i++)
                    {
                        var content = gameState.GridButtonStates[i];
                        if (!string.IsNullOrEmpty(content))
                        {
                            try
                            {
                                gameButtons[i].Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(content))
                                };
                            }
                            catch
                            {
                                gameButtons[i].Content = null;
                            }
                        }

                        if (matchedCards.Contains(i))
                        {
                            var imagePath = gameButtons[i].Tag?.ToString();
                            if (!string.IsNullOrEmpty(imagePath))
                            {
                                gameButtons[i].Content = new Image
                                {
                                    Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute)),
                                    Stretch = Stretch.UniformToFill
                                };
                            }

                            gameButtons[i].IsEnabled = false;
                        }
                    }

                    PlayerNameTimerGrid.Visibility = Visibility.Visible;

                    ButtonPause.Content = isTimerRunning ? "Pause" : "Start";
                    if (isTimerRunning)
                        _timer.Start();
                    else
                    {
                        _timer.Stop();
                        foreach (var button in gameButtons)
                            button.IsEnabled = false;
                    }

                    TimerLabel.Content = TimeSpan.FromSeconds(_timeLeft).ToString(@"mm\:ss");

                    MessageBox.Show($"Jocul {System.IO.Path.GetFileName(filePath)} a fost încărcat cu succes!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la deschiderea jocului: " + ex.Message);
                }
            }
        }


        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            StatisticGrid.Visibility = Visibility.Hidden;
            string playerName = currentPlayer.Name; 
            string gridSize = $"{gridSizeRow}x{gridSizeCol}"; 
            string difficulty = ""; 

            if (gameTime == 300) difficulty = "Easy";
            else if (gameTime == 180) difficulty = "Medium";
            else if (gameTime == 60) difficulty = "Hard";

            string saveFileName = $"{playerName}_{difficulty}_{gridSize}.json";
            string saveFilePath = System.IO.Path.Combine("SavedGames", saveFileName);

            SavedGames gameState = new SavedGames
            {
                PlayerName = playerName,
                TimeLeft = _timeLeft,
                IsTimerRunning = isTimerRunning,
                MatchedCards = new List<int>(matchedCards),
                SelectedCards = new List<int>(selectedCards),
                GameImages = gameButtons.Select(btn => btn.Tag?.ToString() ?? "").ToList(),
                GridButtonStates = new List<string>(),
                GridSizeRow = gridSizeRow,
                GridSizeCol = gridSizeCol
            };

            foreach (var button in gameButtons)
            {
                var buttonState = button.Content == null ? "" : button.Content.ToString(); 
                gameState.GridButtonStates.Add(buttonState);
            }

            Directory.CreateDirectory("SavedGames");

            string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);

            MessageBox.Show($"Jocul a fost salvat cu succes ca: {saveFileName}");
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticGrid.Visibility = Visibility.Visible;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void StandardGame_Click(object sender, RoutedEventArgs e)
        {
            gridSizeRow=4;
            gridSizeCol=4;
        }

        private void CustomGame_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem != null)
            {
                string headerText = menuItem.Header.ToString();

                gridSizeRow=headerText[0] -'0';
                gridSizeCol=headerText[2] -'0';

            }

        }
      private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Student: Balanica Stefan \nEmail: stefanbalanica22@yahoo.com\nGrupa: 10LF331\nSpecializarea: Informatica Aplicata");
        }







        private void LoadImages(int nr)
        {
            string imagesPath= @"AnimalImages";
            switch (selectedCategory)
            {
                case "Category 1":
                    imagesPath = @"AnimalImages";
                    break;
                case "Category 2":
                     imagesPath = @"FruitImages";
                    break;
                case "Category 3":
                     imagesPath = @"ToysImages";
                    break;
            }
            var imageFiles = Directory.GetFiles(imagesPath, "*.jpg").ToList();

            if (imageFiles.Count < nr)
            {
                MessageBox.Show("Nu sunt suficiente imagini în folder.");
                return;
            }

            imagePaths.Clear();
            imagePaths.AddRange(imageFiles.Take(nr)); 

            var gameImages = imagePaths.Concat(imagePaths).ToList();

            Random rand = new Random();
            gameImages = gameImages.OrderBy(x => rand.Next()).ToList();

            CreateGameBoard(gameImages);
        }

        private void CreateGameBoard(List<string> gameImages)
        {
            GameGrid.Children.Clear(); 
            gameButtons.Clear();

            for (int row = 0; row < gridSizeRow; row++)
            {
                for (int col = 0; col < gridSizeCol; col++)
                {
                    var button = new Button
                    {
                        Width = 80,
                        Height = 80,
                        Margin = new System.Windows.Thickness(5),
                        Tag = gameImages[row * gridSizeCol + col] 
                    };

                    button.Click += Card_Click;
                    gameButtons.Add(button);
                    GameGrid.Children.Add(button);

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                }
            }
        }

        private bool ceckWin()
        {
            foreach (var button in gameButtons)
            {
                if (!matchedCards.Contains(gameButtons.IndexOf(button)))
                {
                    return false;
                }
            }
            return true;
        }
        private void ResetAllCards()
        {
            foreach (var button in gameButtons)
            {
                if (!matchedCards.Contains(gameButtons.IndexOf(button)))
                {
                    button.Content = null; 
                }
            }
        }
        private void Card_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string imagePath = button?.Tag.ToString();
            if (button.Content != null)
                return;
            string absoluteImagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), imagePath);

            if(selectedCards.Count == 0)
            {
                ResetAllCards();
            }
            try
            {
                button.Content = new Image
                {
                    Source = new BitmapImage(new Uri(absoluteImagePath)) 
                };
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show($"Eroare la încărcarea imaginii: {ex.Message}");
            }
            selectedCards.Add(gameButtons.IndexOf(button));

            if (selectedCards.Count == 2)
            {
                CheckMatch();
            }
        }
        private void CheckMatch()
        {
            int firstCardIndex = selectedCards[0];
            int secondCardIndex = selectedCards[1];
            if (gameButtons[firstCardIndex].Tag.ToString() == gameButtons[secondCardIndex].Tag.ToString())
            {
                matchedCards.Add(firstCardIndex);
                matchedCards.Add(secondCardIndex);
            }
      
            selectedCards.Clear();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            if(isTimerRunning == false)
            {
                ButtonPause.Content="Pause";
                _timer.Start();
                isTimerRunning = true;
                foreach (var button in gameButtons)
                {
                    button.IsEnabled = true;
                }
            }
            else
            {
                ButtonPause.Content="Start";
                _timer.Stop();
                isTimerRunning = false;
                foreach (var button in gameButtons)
                {
                    button.IsEnabled = false;
                }
            }
        }

        private void ButtonDificulty_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button; 
            if (button != null)
            {
                var buttonContent = button.Content.ToString();
              switch (buttonContent)
               {
                    case "Easy":
                        StartGame(300); 
                        break;
                    case "Medium":
                        StartGame(180);
                        break;
                    case "Hard":
                        StartGame(60);
                        break;
                }
            }
        }


    }
}
