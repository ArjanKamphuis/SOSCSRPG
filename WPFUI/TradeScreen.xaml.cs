using Engine.Models;
using Engine.ViewModels;
using System.Windows;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        public GameSession Session => DataContext as GameSession;

        public TradeScreen()
        {
            InitializeComponent();
        }

        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is GameItem item)
            {
                Session.CurrentPlayer.Gold += item.Price;
                Session.CurrentTrader.AddItemToInventory(item);
                Session.CurrentPlayer.RemoveItemFromInventory(item);
            }
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is GameItem item)
            {
                if (Session.CurrentPlayer.Gold >= item.Price)
                {
                    Session.CurrentPlayer.Gold -= item.Price;
                    Session.CurrentTrader.RemoveItemFromIventory(item);
                    Session.CurrentPlayer.AddItemToInventory(item);
                }
                else
                {
                    _ = MessageBox.Show("You do not have enough gold", "Something went wrong...", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
