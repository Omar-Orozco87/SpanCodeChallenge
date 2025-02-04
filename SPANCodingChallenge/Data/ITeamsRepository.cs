using SPANCodingChallenge.Enums;
using SPANCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace SPANCodingChallenge.Data
{
    public interface ITeamsRepository
    {
        /// <summary>
        /// Retrieves the current Teams dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int>  GetTeamsData();

        /// <summary>
        /// Updates or Inserts a new element to the teams Dictionary
        /// </summary>
        /// <param name="team"></param>
        /// <param name="points"></param>
        public void UpsertTeam(Team team, PointsEnum points);
    }
}
