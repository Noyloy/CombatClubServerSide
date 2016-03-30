using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public class Location
    {
        public float Lat = 0f;
        public float Long = 0f;
        public String Name;

        public Location() { }
        public Location(float lat, float lon, String name = "")
        {
            Lat = lat;
            Long = lon;
            Name = name;
        }
    }
}
