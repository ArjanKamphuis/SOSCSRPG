using Engine.Models;
using Engine.Services;
using Engine.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows;

namespace WPFUI
{
    public partial class Startup : Window
    {
        private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";

        public Startup()
        {
            InitializeComponent();

            DataContext = GameDetailsService.ReadGameDetails();
        }

        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            CharacterCreation characterCreationWindow = new();
            characterCreationWindow.Show();
            Close();
        }

        private void LoadSavedGame_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                MainWindow mainWindow = new(SaveGameService.LoadLastSaveOrCreateNew(openFileDialog.FileName));
                mainWindow.Show();
                Close();
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
