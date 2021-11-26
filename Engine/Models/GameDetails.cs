using System.Collections.Generic;

namespace Engine.Models
{
    public class GameDetails
    {
        public string Name { get; }
        public string Version { get; }
        public List<PlayerAttribute> PlayerAttributes { get; } = new();

        public GameDetails(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}
