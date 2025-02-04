using SPANCodingChallenge.Data;
using SPANCodingChallenge.Logic;
using SPANCodingChallenge.Models;
using System;

namespace SPANCodingChallenge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SPAN coding challenge.\n");
            Console.WriteLine("Program that calcultes the ranking table for a league.\n");
            Console.WriteLine("You can start capturing the results of the matches. \n");

            Console.WriteLine("Once you are done capturing de data, please type 'dashboard'.\n");
            var finishedCapturig = false;
            var teamsRepo = new TeamsRepository();
            var matchesLogic = new MatchesLogic(teamsRepo);

            while (!finishedCapturig)
            {

                Console.WriteLine("Enter a match result.\n");
                string capturedLine = Console.ReadLine();

                if (matchesLogic.IsDashboardCommand(capturedLine))
                {
                    finishedCapturig = true;
                }
                else
                {
                    (Team team1, Team team2, bool error) = matchesLogic.ToTeams(capturedLine);
                    if (error)
                    {
                        Console.WriteLine("There was an error capturing the data, please try again.\n");
                    }
                    else
                    {
                        matchesLogic.SavePoints(team1, team2);
                    }
                }
            }

            var dashBoard = matchesLogic.OrderDashboard();
            foreach (var team in dashBoard) 
            {
                Console.WriteLine($"{team.Key}, {team.Value} pts\n"); 
            }
        }   
    }
}
