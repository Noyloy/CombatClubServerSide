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

        public bool IsReady = false;

        public static string GetSoldierID(string name, string pass)
        {
            CombatClubDataContext ccdc = new CombatClubDataContext();
            Player player = ccdc.Players.Where(p => p.Name == name && p.Password == pass).FirstOrDefault();
            if (player == null) return "-1";
            return player.Id + "";
        }

        public Soldier(int soldierID)
        {
            CombatClubDataContext ccdc = new CombatClubDataContext();
            DataPlayer = ccdc.Players.Where(p => p.Id == soldierID).FirstOrDefault();
            Location = new Location(0f, 0f);
            Health = new Health();
        }
    }
}