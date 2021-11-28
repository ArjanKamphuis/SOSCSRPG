using Engine.EventArgs;
using Engine.Models;
using Engine.Services;
using Engine.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WPFUI.Windows;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";

        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Dictionary<Key, Action> _userInputActions = new();

        private GameSession _gameSession;

        public MainWindow(Player player, int xLocation = 0, int yLocation = 0)
            : this(new GameSession(player, xLocation, yLocation))
        {
        }

        public MainWindow(GameSession gameSession)
            : this()
        {
            SetActiveGameSessionTo(gameSession);
        }

        private MainWindow()
        {
            InitializeComponent();
            InitializeUserInputActions();
        }

        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }
        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }
        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }

        private void OnClick_AttackMonster(object sender, RoutedEventArgs e)
        {
            _gameSession.AttackCurrentMonster();
        }

        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();
        }

        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            if (_gameSession.HasTrader)
            {
                TradeScreen tradeScreen = new() { Owner = this, DataContext = _gameSession };
                _ = tradeScreen.ShowDialog();
            }
        }

        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            _gameSession.CrafItemUsing(((FrameworkElement)sender).DataContext as Recipe);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            _userInputActions.GetValueOrDefault(e.Key)?.Invoke();
        }

        private void InitializeUserInputActions()
        {
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());

            _userInputActions.Add(Key.Z, () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());

            _userInputActions.Add(Key.I, () => SetTabFocusTo("InventoryTabItem"));
            _userInputActions.Add(Key.Q, () => SetTabFocusTo("QuestsTabItem"));
            _userInputActions.Add(Key.R, () => SetTabFocusTo("RecipesTabItem"));

            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));
        }

        private void SetTabFocusTo(string tabName)
        {
            foreach (object item in PlayerDataTabControl.Items)
            {
                if (item is TabItem tabItem && tabItem.Name == tabName)
                {
                    tabItem.IsSelected = true;
                    return;
                }
            }
        }

        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;
            
            _gameSession?.Dispose();
            _gameSession = gameSession;
            DataContext = _gameSession;

            gameMessages.Document.Blocks.Clear();

            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }

        private void SaveGame()
        {
            SaveFileDialog saveFileDialog = new()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveGameService.Save(_gameSession, saveFileDialog.FileName);
            }
        }

        private void AskToSaveGame()
        {
            YesNoWindow message = new("Save Game", "Do you want to save your game?") { Owner = GetWindow(this) };
            _ = message.ShowDialog();
            if (message.ClickedYes)
            {
                SaveGame();
            }
        }

        private void ClearGameSessionDataAndClose()
        {
            _gameSession?.Dispose();
            _gameSession = null;
            Close();
        }

        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            gameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            gameMessages.ScrollToEnd();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AskToSaveGame();
        }

        private void OnClick_StartNewGame(object sender, RoutedEventArgs e)
        {
            Startup startup = new();
            startup.Show();
            ClearGameSessionDataAndClose();
        }

        private void OnClick_SaveGame(object sender, RoutedEventArgs e)
        {
            SaveGame();
        }

        private void OnClick_Exit(object sender, RoutedEventArgs e)
        {
            ClearGameSessionDataAndClose();
        }
    }
}
