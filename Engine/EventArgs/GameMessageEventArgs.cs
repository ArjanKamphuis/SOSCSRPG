using System;
using System.Collections.Generic;
using System.Text;

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
