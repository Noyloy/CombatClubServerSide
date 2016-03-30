using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatClubServer.CombatClub
{
    public class Health
    {
        public int Percentage = 100;
        public bool IsAlive = true;

        public Health()
        {
            Percentage = 100;
            IsAlive = true;
        }
        public Health(int hp)
        {
            IsAlive = (hp > 0);
            Percentage = hp;
        }

        public void Kill()
        {
            Percentage = 0;
            IsAlive = false;
        }

        public void Revive(int toPercentage)
        {
            Percentage = toPercentage;
            IsAlive = true;
        }
    }
}
