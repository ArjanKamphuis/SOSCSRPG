using Engine.ViewModels;
using System.Windows;

namespace WPFUI
{
    public partial class CharacterCreation : Window
    {
        private readonly CharacterCreationViewModel _viewModel = new();

        public CharacterCreation()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void Race_OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _viewModel.ApplyAttributeModifiers();
        }

        private void RandomPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            _viewModel.RollNewCharacter();
        }

        private void UseThisPlayer_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(_viewModel.GetPlayer());
            mainWindow.Show();
            Close();
        }
    }
}
