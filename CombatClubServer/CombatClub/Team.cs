using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public enum TeamSide { Good, Bad}
    public class Team
    {
        private const int TEAM_CAP = 5;

        public TeamSide TeamSide;
        public Dictionary<int, Soldier> SoldiersDict = new Dictionary<int, Soldier>();

        public Team(TeamSide teamSide)
        {
            TeamSide = teamSide;
        }

        public bool JoinTeam(Soldier soldier)
        {
            if (isTeamFull()) return false;

            SoldiersDict.Add(soldier.DataPlayer.Id, soldier);
            return true;
        }

        private bool isTeamFull()
        {
            return SoldiersDict.Count >= TEAM_CAP;
        }
    }
}
