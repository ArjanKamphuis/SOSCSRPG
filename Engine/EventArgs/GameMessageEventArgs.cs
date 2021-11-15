namespace Engine.EventArgs
{
    public class GameMessageEventArgs : System.EventArgs
    {
        public string Message { get; }
        public GameMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
