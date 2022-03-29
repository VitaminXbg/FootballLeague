using FootballLeague.Data;
using FootballLeague.Domain;
using System;
using System.Linq;

namespace ConsoleFiddleApp
{
    class Program
    {
        private static FootballLeagueContext context = new();
        static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            var count = GetTeamCount();
            Console.WriteLine($"Count BEFORE created: {count}");
            AddTeam("Levski");
            count = GetTeamCount();
            Console.WriteLine($"Count AFTER created: {count}");

            var teamBeforeUpdate = GetTeam("Levski");
            Console.WriteLine($"Score for team {teamBeforeUpdate.Name} BEFORE update: {teamBeforeUpdate.Score}");
            var teamToUpdate = new Team() { Name = "Levski", Score = 10 };
            UpdateTeam(teamToUpdate);

            var teamAfterUpdate = GetTeam("Levski");
            Console.WriteLine($"Score for team {teamAfterUpdate.Name} AFTER update: {teamAfterUpdate.Score}");

            Console.ReadLine();
        }

        public static void AddTeam(string teamName)
        {
            if (!context.Teams.Any(t => t.Name == teamName))
            {
                var team = new Team { Name = teamName };
                context.Teams.Add(team);
                context.SaveChanges();
            }
        }

        public static Team GetTeam(int id)
        {
            var team = context.Teams.Find(id);
            return team;
        }

        public static Team GetTeam(string name)
        {
            var team = context.Teams.First(t => t.Name == name);
            return team;
        }

        public static void UpdateTeam(Team team)
        {
            var teamToUpdate = GetTeam(team.Name);
            teamToUpdate.Score = team.Score;
            context.Teams.Update(teamToUpdate);
            context.SaveChanges();
        }

        public static void RemoveTeam(string teamName)
        {
            var teamToRemove = context.Teams.First(t => t.Name == teamName);
            context.Teams.Remove(teamToRemove);
            context.SaveChanges();
        }

        public static void RemoveTeam(int teamId)
        {
            var teamToRemove = context.Teams.Find(teamId);
            context.Teams.Remove(teamToRemove);
            context.SaveChanges();
        }

        public static int GetTeamCount()
        {
            return context.Teams.AsEnumerable().Count();
        }

        public static void AddMatch(Match match)
        {
            var homeTeam = context.Teams.Find(match.HomeTeam.Id);
            var awayTeam = context.Teams.Find(match.AwayTeam.Id);

            if(match.HomeTeamGoals > match.AwayTeamGoals)
            {
                homeTeam.Score += 3;
            }
            else if(match.HomeTeamGoals < match.AwayTeamGoals)
            {
                awayTeam.Score += 3;
            }
            else
            {
                homeTeam.Score++;
                awayTeam.Score++;
            }

            context.Teams.UpdateRange(homeTeam, awayTeam);
            context.Matches.Add(match);
            context.SaveChanges();
        }
    }
}
