using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public class EnemyMark
    {
        private static int EnemyMarkCount;
        public int ID;
        public Location Location;

        public EnemyMark() { Location = new Location(); }
        public EnemyMark(Location location)
        {
            EnemyMarkCount++;
            ID = EnemyMarkCount;
            this.Location = location;
        }
        
    }
}
