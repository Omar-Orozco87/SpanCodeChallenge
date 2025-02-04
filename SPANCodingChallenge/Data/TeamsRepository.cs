using SPANCodingChallenge.Enums;
using SPANCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPANCodingChallenge.Data
{
    public class TeamsRepository: ITeamsRepository
    {
        private Dictionary<string, int> Teams { get; }

        public TeamsRepository() 
        {
            Teams = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        public Dictionary<string, int> GetTeamsData() => Teams;

        /// <summary>
        /// Upsert the current Dictionary. For an exiting key input with 'points' as win it will update the existing value by adding 3.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="points"></param>
        public void UpsertTeam(Team team, PointsEnum points)
        {
            if (Teams.ContainsKey(team.Name))
            {
                var currentValue = Teams[team.Name];
                // Teams[team.Name] = currentValue + (int)points;
                UpdateTeam(team.Name, currentValue + (int)points);
            }
            else
            {
                SaveTeam(team.Name, (int)points);
            }
        }

        private void SaveTeam(string Name, int points)
        {
            Teams.Add(Name, points);
        }

        private void UpdateTeam(string Name, int points)
        {
            Teams[Name] = points;
        }
    }
}
