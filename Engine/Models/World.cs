using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>();

        internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            _locations.Add(new Location
            {
                XCoordinate = xCoordinate,
                YCoordinate = yCoordinate,
                Name = name,
                Description = description,
                ImageName = $"pack://application:,,,/Engine;component/Images/Locations/{imageName}"
            });
        }

        public Location LocationAt(int xCoordinate, int yCoordinate)
        {
            return _locations.SingleOrDefault(loc => loc.XCoordinate == xCoordinate && loc.YCoordinate == yCoordinate);
        }
    }
}
