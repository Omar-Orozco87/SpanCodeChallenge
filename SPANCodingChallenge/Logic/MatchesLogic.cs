using SPANCodingChallenge.Data;
using SPANCodingChallenge.Enums;
using SPANCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SPANCodingChallenge.Logic
{
    public class MatchesLogic: IMatchesLogic
    {
        public const string DASHBOARD_COMMAND = "dashboard";

        public ITeamsRepository teamsRepository;

        public MatchesLogic(ITeamsRepository repo) 
        {
            teamsRepository = repo;
        }

        /// <summary>
        /// Apply the points rules by adding or updating accordinlgy to reflect the match results
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void SavePoints(Team team1, Team team2)
        {
            if(team1.Score == team2.Score)
            {
                teamsRepository.UpsertTeam(team1, PointsEnum.DRAW);
                teamsRepository.UpsertTeam(team2, PointsEnum.DRAW);
                return;
            }

            if(team1.Score > team2.Score)
            {
                teamsRepository.UpsertTeam(team1, PointsEnum.WIN);
                teamsRepository.UpsertTeam(team2, PointsEnum.LOSE);
                return;
            }

            teamsRepository.UpsertTeam(team1, PointsEnum.LOSE);
            teamsRepository.UpsertTeam(team2, PointsEnum.WIN);
        }     

        /// <summary>
        /// Sort the current teams/points by descending
        /// </summary>
        /// <returns></returns>
        public IOrderedEnumerable<KeyValuePair<string, int>> OrderDashboard()
        {
            var result = teamsRepository.GetTeamsData()
                .OrderByDescending(pair => pair.Value);
            return result;
        }

        /// <summary>
        /// Verifies if the input contains the 'dashboard' key word
        /// </summary>
        /// <param name="lineInput"></param>
        /// <returns></returns>
        public bool IsDashboardCommand(string lineInput)
        {
            return lineInput.Contains(DASHBOARD_COMMAND, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Parse an input string to a 'match result' (team1 2 , team2 3) form. Also returns a boolean that indicates if there was any error while parsing the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public (Team, Team, bool error) ToTeams(string str)
        {
            var strings = str.Split(',');

            (Team team1, bool error1) = ParseToTeam(strings[0].Trim());
            (Team team2, bool error2) = ParseToTeam(strings[1].Trim());


            return (team1, team2, error1 || error2);

        }

        /// <summary>
        /// Converts an input string to object of type Team; Name, Score. Also returns a boolean that indicates if there was any erroe while parsing the string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private (Team, bool) ParseToTeam(string input)
        {
            int separator = 0;
            bool error = false;
            var team = new Team();
            try
            {
                for (int index = input.Length - 1; index > 0; index--)
                {
                    if ((int)input[index] == 32)
                    {
                        separator = index;
                        break;
                    }
                }
                team.Name = input.Substring(0, separator);
                team.Score = int.Parse(input.Substring(separator));
            }
            catch (Exception)
            {
                error = true;
            }

            return (team, error);
        }
    }
}
