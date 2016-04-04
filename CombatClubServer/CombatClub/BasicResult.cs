using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public class BasicResult
    {
        public int Code;
        public string Message;
        public string Value;

        public BasicResult() { }

        public BasicResult(int code,string message,string value)
        {
            this.Code = code;
            this.Message = message;
            this.Value = value;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
