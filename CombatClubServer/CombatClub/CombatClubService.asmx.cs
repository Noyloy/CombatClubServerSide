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
        public static Dictionary<int, GameSession> games;
        //
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
