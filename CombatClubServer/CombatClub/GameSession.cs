using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public class GameSession
    {
        private const int TEAM_COUNT = 2;
        private int gameID;

        // 1hr max game time
        public int MaxGameTime = 60;
        public Team[] Teams = new Team[TEAM_COUNT];

        public GameSession(int GameID, int maxGameTime = 60)
        {
            this.gameID = GameID;
            this.MaxGameTime = maxGameTime;
        }
    }
}
