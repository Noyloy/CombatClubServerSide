using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public enum TeamSide { Good, Bad }
    public class Team
    {
        public const int TEAM_CAP = 5;

        public TeamSide TeamSide;
        public Dictionary<int, Soldier> SoldiersDict = new Dictionary<int, Soldier>();
        // Gets Set Each Time Soldier Joins or Leaves
        public AutoResetEvent TeamCountChanged = new AutoResetEvent(false);

        public Team(TeamSide teamSide)
        {
            TeamSide = teamSide;
        }

        public bool JoinTeam(Soldier soldier)
        {
            if (isTeamFull()) return false;

            if (SoldiersDict.ContainsKey(soldier.DataPlayer.Id)) return true;
            SoldiersDict.Add(soldier.DataPlayer.Id, soldier);
            for (int i = 0; i < SoldiersDict.Count(); i++) TeamCountChanged.Set();
            return true;
        }

        public bool LeaveTeam(Soldier soldier)
        {
            int soldierID = soldier.DataPlayer.Id;
            if (!SoldiersDict.ContainsKey(soldierID)) return false;

            SoldiersDict.Remove(soldierID);
            for (int i = 0; i < SoldiersDict.Count(); i++) TeamCountChanged.Set();
            return true;
        }

        private bool isTeamFull()
        {
            return SoldiersDict.Count >= TEAM_CAP;
        }
    }
}
