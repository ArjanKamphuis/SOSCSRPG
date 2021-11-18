using Engine.Services;
using System.Windows;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string exceptionMessageTest = $"An exception occurred: {e.Exception.Message}\r\n\r\nat: {e.Exception.StackTrace}";
            LoggingService.Log(e.Exception);
            _ = MessageBox.Show(exceptionMessageTest, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
