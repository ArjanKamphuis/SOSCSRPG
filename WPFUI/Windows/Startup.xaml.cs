using Engine.Models;
using Engine.Services;
using System.Windows;

namespace WPFUI.Windows
{
    public partial class Startup : Window
    {
        private readonly GameDetails _gameDetails;

        public Startup()
        {
            InitializeComponent();

            _gameDetails = GameDetailsService.ReadGameDetails();
            DataContext = _gameDetails;
        }

        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            CharacterCreation characterCreationWindow = new();
            characterCreationWindow.Show();
            Close();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
