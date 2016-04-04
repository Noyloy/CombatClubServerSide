using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public class GameSession
    {
        private static int allGameID;

        public const int TEAM_COUNT = 2;

        private int gameID;
        public DateTime gameStartTime;

        public string GameName;

        // 30minutes max game time
        public int MaxGameTime = 30;
        public Team[] Teams = new Team[TEAM_COUNT];
        public int PlayersCount { get { return Teams[0].SoldiersDict.Count() + Teams[1].SoldiersDict.Count(); } }

        public static int GetGameID()
        {
            return allGameID;
        }

        public GameSession(int GameID,string gameName, int maxGameTime = 30)
        {
            this.GameName = gameName;
            this.gameID = GameID;
            this.MaxGameTime = maxGameTime;
            for (int i = 0; i < TEAM_COUNT; i++) Teams[i] = new Team((TeamSide)i);
            allGameID++;
        }

        public void GameStart(int playerID)
        {
            Soldier soldier = getPlayer(playerID);
            if (soldier == null) return;
            soldier.IsReady = true;

            // when all ready
            if (IsAllReady()) gameStartTime = DateTime.Now;
        }
        public bool IsAllReady()
        {
            foreach (Team team in Teams)
                foreach (Soldier soldier in team.SoldiersDict.Values)
                    if (!soldier.IsReady) return false;
            return true;
        }
        private Soldier getPlayer(int playerID)
        {
            foreach (Team team in Teams)
                if (team.SoldiersDict.ContainsKey(playerID)) return team.SoldiersDict[playerID];
            return null;
        }
    }
}
