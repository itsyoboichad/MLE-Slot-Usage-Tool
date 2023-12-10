using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamSlotUsagesTool
{
    internal class Player
    {
        public string PlayerName { get; private set; }
        public float Salary {  get; private set; }
        public Leagues League { get; private set; }
        public TeamName Team { get; private set; }
        public Slots Slot { get; private set; }
        public int ScrimPoints { get; private set; }
        public Player(string playerName, float salary, Leagues league, TeamName team, Slots slot, int scrimPoints)
        {
            PlayerName = playerName;
            Salary = salary;
            League = league;
            Team = team;
            Slot = slot;
            ScrimPoints = scrimPoints;
        }
    }
}
