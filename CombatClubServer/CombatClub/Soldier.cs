using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CombatClubServer.CombatClub
{
    public class Soldier
    {
        public Player DataPlayer;
        public Location Location;
        public Health Health;

        public Soldier(int soldierID)
        {
            CombatClubDataContext ccdc = new CombatClubDataContext();
            DataPlayer = ccdc.Players.Where(p => p.Id == soldierID).FirstOrDefault();
        }
    }
}