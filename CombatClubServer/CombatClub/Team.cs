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
        public Dictionary<int, EnemyMark> EnemyDict = new Dictionary<int, EnemyMark>();

        public Team(TeamSide teamSide)
        {
            TeamSide = teamSide;
        }

        public bool JoinTeam(Soldier soldier)
        {
            if (isTeamFull()) return false;

            if (SoldiersDict.ContainsKey(soldier.DataPlayer.Id)) return true;
            SoldiersDict.Add(soldier.DataPlayer.Id, soldier);
            return true;
        }

        public bool LeaveTeam(Soldier soldier)
        {
            int soldierID = soldier.DataPlayer.Id;
            if (!SoldiersDict.ContainsKey(soldierID)) return false;

            SoldiersDict.Remove(soldierID);
            return true;
        }

        public EnemyMark MarkEnemy(Location location)
        {
            EnemyMark tmp_em = new EnemyMark(location);
            EnemyDict.Add(tmp_em.ID, tmp_em);
            return tmp_em;
        }
        public bool UnmarkEnemy(int enemyMarkID)
        {
            if (!EnemyDict.ContainsKey(enemyMarkID)) return false;
            EnemyDict.Remove(enemyMarkID);
            return true;
        }

        private bool isTeamFull()
        {
            return SoldiersDict.Count >= TEAM_CAP;
        }
    }
}
