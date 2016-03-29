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

        // 1hr max game time
        public int MaxGameTime = 60;
        public Team[] Teams = new Team[TEAM_COUNT];
        public int PlayersCount { get { return Teams[0].SoldiersDict.Count() + Teams[1].SoldiersDict.Count(); } }

        public static int GetGameID()
        {
            return allGameID;
        }

        public GameSession(int GameID, int maxGameTime = 60)
        {
            this.gameID = GameID;
            this.MaxGameTime = maxGameTime;
            for (int i = 0; i < TEAM_COUNT; i++) Teams[i] = new Team((TeamSide)i);
            allGameID++;
        }

        
    }
}
