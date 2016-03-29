using CombatClubServer.CombatClub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CombatClubServer
{
    /// <summary>
    /// Summary description for CombatClubService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CombatClubService : System.Web.Services.WebService
    {
        public static int MaxGameCount = 3;
        public static Dictionary<int,GameSession> games = new Dictionary<int, GameSession>();

        [WebMethod]
        public BasicResult CreateGame()
        {
            if (games.Count() >= MaxGameCount) return new BasicResult(-1,"Server Reached Max Game Count ("+MaxGameCount+")","");

            int gameID = GameSession.GetGameID();
            games.Add(gameID,new GameSession(gameID));
            return new BasicResult(1, "Server Created New Game", ""+gameID);
        }
        [WebMethod]
        public BasicResult RemoveGame(int gameID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "");
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game ("+gameID+") Does Not Exist", "");
            if (games[gameID].PlayersCount > 0) return new BasicResult(-3, "Game (" + gameID + ") Has Players", "");

            games.Remove(gameID);
            return new BasicResult(1, "Server Removed Game", "" +gameID);
        }

        [WebMethod]
        public BasicResult JoinTeam(int soldierID, int gameID,int teamID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "");
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game (" + gameID + ") Does Not Exist", "");
            if (teamID> GameSession.TEAM_COUNT) return new BasicResult(-3, "Team ID Needs to be between 0 and "+ (GameSession.TEAM_COUNT-1), "");

            Soldier soldier = new Soldier(soldierID);
            if (games[gameID].Teams[teamID].JoinTeam(soldier)) return new BasicResult(1, "Player ("+soldierID+")"+soldier.DataPlayer.Name+" Joined Game "+gameID+" Team "+teamID, "");
            else return new BasicResult(-4, "Can't Join Team", "");
        }
        [WebMethod]
        public BasicResult LeaveTeam(int soldierID, int gameID, int teamID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "");
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game (" + gameID + ") Does Not Exist", "");
            if (teamID > GameSession.TEAM_COUNT) return new BasicResult(-3, "Team ID Needs to be between 0 and " + (GameSession.TEAM_COUNT - 1), "");

            Soldier soldier = new Soldier(soldierID);
            if (games[gameID].Teams[teamID].LeaveTeam(soldier)) return new BasicResult(1, "Player (" + soldierID + ")" + soldier.DataPlayer.Name + " Left Game " + gameID+ " Team " + teamID, "");
            else return new BasicResult(-4, "Can't Leave Team", "");
        }
        [WebMethod]
        public BasicResult WaitTeamCount(int soldierID, int gameID, int teamID)
        {
            bool signaled = games[gameID].Teams[teamID].TeamCountChanged.WaitOne();
            return new BasicResult(1, "Team Count Changed", games[gameID].Teams[teamID].SoldiersDict.Count().ToString());
        }
    }
}
