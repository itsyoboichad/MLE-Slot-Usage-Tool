using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamSlotUsagesTool
{
    internal class Match
    {
        public int Season { get; private set; }
        public int MatchNumber { get; private set; }
        public TeamName HomeTeam { get; private set; }
        public TeamName AwayTeam { get; private set; }
        public int id { get; private set; }
        public Leagues League { get; private set; }
        public GameMode GameMode { get; private set; }
        public TeamName Team { get; private set; }
        public Slots Slot { get; private set; }
        // Get player name?

        public Match(int season, int matchNumber, TeamName homeTeam, TeamName awayTeam, int id, Leagues league, GameMode gameMode, TeamName team, Slots slot)
        {
            Season = season;
            MatchNumber = matchNumber;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            this.id = id;
            League = league;
            GameMode = gameMode;
            Team = team;
            Slot = slot;
        }

        public override string ToString()
        {
            return Season + " " + MatchNumber + " " + HomeTeam.ToString() + " " + AwayTeam.ToString() + " " + id + " " + League.ToString() + " " + GameMode.ToString() + " " + Team.ToString() + " " + Slot.ToString();
        }
    }
}
