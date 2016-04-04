using CombatClubServer.CombatClub;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public string PassConfirm(string name, string pass)
        {
            return Soldier.GetSoldierID(name,pass);
        }

        [WebMethod]
        public string CreateGame(string gameName)
        {
            if (games.Count() >= MaxGameCount) return new BasicResult(-1,"Server Reached Max Game Count ("+MaxGameCount+")","").ToJSON();

            int gameID = GameSession.GetGameID();
            games.Add(gameID,new GameSession(gameID,gameName));
            return new BasicResult(1, "Server Created New Game", ""+gameID).ToJSON();
        }
        [WebMethod]
        public string RemoveGame(int gameID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "").ToJSON();
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game ("+gameID+") Does Not Exist", "").ToJSON();
            if (games[gameID].PlayersCount > 0) return new BasicResult(-3, "Game (" + gameID + ") Has Players", "").ToJSON();

            games.Remove(gameID);
            return new BasicResult(1, "Server Removed Game", "" +gameID).ToJSON();
        }
        [WebMethod]
        public string GetGames()
        {
            return JsonConvert.SerializeObject(games.Values.Select(game => 
            new {   id = game.GameID,
                    name = game.GameName,
                    startTime = game.gameStartTime,
                    gameTime = game.MaxGameTime,
                    players0 = game.Teams[0].SoldiersDict.Count,
                    players1 = game.Teams[1].SoldiersDict.Count,
                    tickets0 = game.Teams[0].NumberTickets,
                    tickets1 = game.Teams[1].NumberTickets,
                    playersCap = Team.TEAM_CAP,
                    ticketsCap = Team.TICKETS_CAP }).ToList());
        }
        [WebMethod]
        public string StartGame(int gameID,int soldierID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "").ToJSON();
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game (" + gameID + ") Does Not Exist", "").ToJSON();

            games[gameID].GameStart(soldierID);
            return new BasicResult(1, "Joined The game", gameID+ "" + soldierID).ToJSON();
        }
        [WebMethod]
        public string IsGameReady(int gameID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "").ToJSON();
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game (" + gameID + ") Does Not Exist", "").ToJSON();

            return JsonConvert.SerializeObject(games[gameID].IsAllReady());
        }
        [WebMethod]
        public string GetGameStatus(int gameID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "").ToJSON();
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game (" + gameID + ") Does Not Exist", "").ToJSON();
            return JsonConvert.SerializeObject(games[gameID].Teams.Select(team => new { id = team.TeamSide, count = team.NumberTickets }).ToList());
        }

        [WebMethod]
        public string JoinTeam(int soldierID, int gameID,int teamID)
        {
            BasicResult res = validateDicts(soldierID, gameID, teamID);
            if (res.Code < 0) return res.ToJSON();

            Soldier soldier = new Soldier(soldierID);
            if (games[gameID].Teams[teamID].JoinTeam(soldier)) return new BasicResult(1, "Player ("+soldierID+")"+soldier.DataPlayer.Name+" Joined Game "+gameID+" Team "+teamID, "").ToJSON();
            else return new BasicResult(-4, "Can't Join Team", "").ToJSON();
        }
        [WebMethod]
        public string LeaveTeam(int soldierID, int gameID, int teamID)
        {
            BasicResult res = validateDicts(soldierID, gameID, teamID);
            if (res.Code < 0) return res.ToJSON();

            Soldier soldier = new Soldier(soldierID);
            if (games[gameID].Teams[teamID].LeaveTeam(soldier)) return new BasicResult(1, "Player (" + soldierID + ")" + soldier.DataPlayer.Name + " Left Game " + gameID+ " Team " + teamID, "").ToJSON();
            else return new BasicResult(-4, "Can't Leave Team", "").ToJSON();
        }
        [WebMethod]
        public string GetTeamStatus(int soldierID, int gameID, int teamID)
        {
            return JsonConvert.SerializeObject(games[gameID].Teams[teamID].SoldiersDict.Values.Select(soldier =>
                new ClientSoldier
                {
                    id = soldier.DataPlayer.Id,
                    name = soldier.DataPlayer.Name,
                    health = soldier.Health,
                    location = soldier.Location
                }).ToList());
        }
        [WebMethod]
        public string GetTeamEnemyMarkStatus(int soldierID, int gameID, int teamID)
        {
            return JsonConvert.SerializeObject(games[gameID].Teams[teamID].EnemyDict.Values.ToList());
        }

        [WebMethod]
        public string UpdateSoldier(int soldierID, int gameID, int teamID, int hp, float lat, float lon, string locName)
        {
            BasicResult res = validateDicts(soldierID, gameID, teamID);
            if (res.Code < 0) return res.ToJSON();
            if (!games[gameID].Teams[teamID].SoldiersDict.ContainsKey(soldierID)) return new BasicResult(-4, "Player (" + soldierID + ") Isn't In Game " + gameID + " Team " + teamID,"").ToJSON();

            Health tmpHeal = new Health(hp);
            Location tmpLoc = new Location(lat, lon, locName);

            // soldier died and wasn't dead already
            if (games[gameID].Teams[teamID].SoldiersDict[soldierID].Health.IsAlive && !tmpHeal.IsAlive)
            {
                if(games[gameID].Teams[teamID].NumberTickets>=0) games[gameID].Teams[teamID].NumberTickets--;
            }

            games[gameID].Teams[teamID].SoldiersDict[soldierID].Health = tmpHeal;
            games[gameID].Teams[teamID].SoldiersDict[soldierID].Location = tmpLoc;

            return new BasicResult(1, "Soldier Updated", "").ToJSON();
        }
        [WebMethod]
        public string MarkEnemy(int soldierID, int gameID, int teamID, float lat, float lon, string locName)
        {
            BasicResult res = validateDicts(soldierID, gameID, teamID);
            if (res.Code < 0) return res.ToJSON();
            if (!games[gameID].Teams[teamID].SoldiersDict.ContainsKey(soldierID)) return new BasicResult(-4, "Player (" + soldierID + ") Isn't In Game " + gameID + " Team " + teamID, "").ToJSON();
            Location tmpLoc = new Location(lat, lon, locName);

            EnemyMark tmpEMark = games[gameID].Teams[teamID].MarkEnemy(tmpLoc);

            return new BasicResult(1, "Enemy Marked", ""+tmpEMark.ID).ToJSON();
        }
        [WebMethod]
        public string UnmarkEnemy(int soldierID, int gameID, int teamID, int enemyID)
        {
            BasicResult res = validateDicts(soldierID, gameID, teamID);
            if (res.Code < 0) return res.ToJSON();
            if (!games[gameID].Teams[teamID].SoldiersDict.ContainsKey(soldierID)) return new BasicResult(-4, "Player (" + soldierID + ") Isn't In Game " + gameID + " Team " + teamID, "").ToJSON();
            if (!games[gameID].Teams[teamID].EnemyDict.ContainsKey(enemyID)) return new BasicResult(-5, "Enemy (" + soldierID + ") Isn't Marked", "").ToJSON();

            if (!games[gameID].Teams[teamID].UnmarkEnemy(enemyID)) return new BasicResult(-6, "Can't Unmark Enemy (" + enemyID + ")", "").ToJSON();

            return new BasicResult(1, "Enemy Unmarked", "" + enemyID).ToJSON();
        }

        private BasicResult validateDicts(int soldierID, int gameID, int teamID)
        {
            if (games.Count() == 0) return new BasicResult(-1, "No Games On Server", "");
            if (!games.ContainsKey(gameID)) return new BasicResult(-2, "Game (" + gameID + ") Does Not Exist", "");
            if (teamID > GameSession.TEAM_COUNT) return new BasicResult(-3, "Team ID Needs to be between 0 and " + (GameSession.TEAM_COUNT - 1), "");

            return new BasicResult(1, "Valid", "");
        }
    }
}
