using SPANCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPANCodingChallenge.Logic
{
    public interface IMatchesLogic
    {
        public void SavePoints(Team team1, Team team2);

        public (Team, Team, bool error) ToTeams(string str);
    }
}
