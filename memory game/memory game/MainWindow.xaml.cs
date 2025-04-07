using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace memory_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        private string selectedImagePath;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
        }

        private void ButtonNewUser_Click(object sender, RoutedEventArgs e)
        {
            /*var newPlayer = new Player("Nou", 0, "Images/lion.jpg");
            viewModel.Players.Add(newPlayer);
            viewModel.SavePlayers();*/
            NewUserGrid.Visibility = Visibility.Visible;
        }

        private void ButtonAddUser_Click(object sender, RoutedEventArgs e)
        {
            
            string playerName = PlayerNameTextBox.Text;
            if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Please provide a name and select an avatar image.");
                return;
            }

            var newPlayer = new Player(playerName, 0, 0,selectedImagePath);
            viewModel.Players.Add(newPlayer);
            viewModel.SavePlayers();
            NewUserGrid.Visibility=Visibility.Hidden;
        }
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select Avatar Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                string imagesDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                string fileName = System.IO.Path.GetFileName(selectedFilePath);

                string savedFilePath = System.IO.Path.Combine(imagesDirectory, fileName);

                File.Copy(selectedFilePath, savedFilePath, true);

                selectedImagePath = savedFilePath;

                AvatarImage.Source = new BitmapImage(new Uri(savedFilePath));
            }
        }

        private void ButtonDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeletePlayer();

        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            // Verificăm dacă există un jucător selectat
            if (viewModel.SelectedPlayer == null)
            {
                MessageBox.Show("Please select a player before starting the game.");
                return;
            }

            // Dacă există un jucător selectat, deschidem fereastra de joc
            PlayWindow playWindow = new PlayWindow(viewModel, viewModel.SelectedPlayer); // Transmit jucătorul selectat
            playWindow.Show(); // Folosește Show() pentru a deschide fereastra
        }
    }
    }


