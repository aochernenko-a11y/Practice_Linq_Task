using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace Practice_Linq
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path = @"../../../data/results_2010.json";

            List<FootballGame> games = ReadFromFileJson(path);

            int testCount = games.Count();
            Console.WriteLine($"Test value = {testCount}.");

            Query1(games);
            Query2(games);
            Query3(games);
            Query4(games);
            Query5(games);
            Query6(games);
            Query7(games);
            Query8(games);
            Query9(games);
            Query10(games);
            Query11(games);
            Query12(games);
            Query13(games);
            Query14(games);
            Query15(games);
        }

        static List<FootballGame> ReadFromFileJson(string path)
        {
            using var reader = new StreamReader(path);
            string jsondata = reader.ReadToEnd();

            List<FootballGame> games = JsonConvert.DeserializeObject<List<FootballGame>>(jsondata)!;
            return games;
        }

        static void PrintGame(FootballGame g)
        {
            Console.WriteLine(
                $"{g.Date:yyyy-MM-dd} | {g.Home_team} {g.Home_score}:{g.Away_score} {g.Away_team} | " +
                $"{g.Tournament} | {g.City}, {g.Country} | Neutral={g.Neutral}"
            );
        }

        static string GetHomeResult(FootballGame g)
        {
            if (g.Home_score > g.Away_score) return "Win";
            if (g.Home_score < g.Away_score) return "Loss";
            return "Draw";
        }

        static bool IsTeamWinner(FootballGame g, string team)
        {
            return
                (g.Home_team == team && g.Home_score > g.Away_score) ||
                (g.Away_team == team && g.Away_score > g.Home_score);
        }

        static void Query1(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Country == "Ukraine")
                    .Where(g => g.Date.Year == 2012)
                    .OrderBy(g => g.Date)
                    .ThenBy(g => g.City)
                    .ThenBy(g => g.Home_team);

            Console.WriteLine("\n======================== QUERY 1 ========================");
            foreach (var g in selectedGames)
                PrintGame(g);
        }

        static void Query2(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Tournament == "Friendly")
                    .Where(g => g.Date.Year >= 2020)
                    .Where(g => g.Home_team == "Italy" || g.Away_team == "Italy")
                    .OrderBy(g => g.Date);

            Console.WriteLine("\n======================== QUERY 2 ========================");
            foreach (var g in selectedGames)
                PrintGame(g);
        }

        static void Query3(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Home_team == "France")
                    .Where(g => g.Date.Year == 2021)
                    .Where(g => g.Home_score == g.Away_score)
                    .OrderBy(g => g.Date);

            Console.WriteLine("\n======================== QUERY 3 ========================");
            foreach (var g in selectedGames)
                PrintGame(g);
        }

        static void Query4(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Away_team == "Germany")
                    .Where(g => g.Date.Year >= 2018 && g.Date.Year <= 2020)
                    .Where(g => g.Away_score < g.Home_score)
                    .OrderBy(g => g.Date);

            Console.WriteLine("\n======================== QUERY 4 ========================");
            foreach (var g in selectedGames)
                PrintGame(g);
        }

        static void Query5(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Tournament.Contains("UEFA Euro", StringComparison.OrdinalIgnoreCase))
                    .Where(g => g.Tournament.Contains("qualif", StringComparison.OrdinalIgnoreCase))
                    .Where(g => g.City == "Kyiv" || g.City == "Kharkiv")
                    .Where(g => IsTeamWinner(g, "Ukraine"))
                    .OrderBy(g => g.Date);

            Console.WriteLine("\n======================== QUERY 5 ========================");
            foreach (var g in selectedGames)
                PrintGame(g);
        }

        static void Query6(List<FootballGame> games)
        {
            int lastWorldCupYear =
                games
                    .Where(g => g.Tournament == "FIFA World Cup")
                    .Max(g => g.Date.Year);

            var selectedGames =
                games
                    .Where(g => g.Tournament == "FIFA World Cup")
                    .Where(g => g.Date.Year == lastWorldCupYear)
                    .OrderByDescending(g => g.Date)
                    .ThenByDescending(g => g.Home_team)
                    .Take(8)
                    .OrderBy(g => g.Date);

            Console.WriteLine("\n======================== QUERY 6 ========================");
            Console.WriteLine($"Last FIFA World Cup year = {lastWorldCupYear}");
            foreach (var g in selectedGames)
                PrintGame(g);
        }

        static void Query7(List<FootballGame> games)
        {
            var firstWin =
                games
                    .Where(g => g.Date.Year == 2023)
                    .Where(g => IsTeamWinner(g, "Ukraine"))
                    .OrderBy(g => g.Date)
                    .FirstOrDefault();

            Console.WriteLine("\n======================== QUERY 7 ========================");
            if (firstWin == null)
            {
                Console.WriteLine("No matches found.");
                return;
            }

            PrintGame(firstWin);
        }

        static void Query8(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Tournament == "UEFA Euro")
                    .Where(g => g.Date.Year == 2012)
                    .Where(g => g.Country == "Ukraine")
                    .OrderBy(g => g.Date)
                    .Select(g => new
                    {
                        MatchYear = g.Date.Year,
                        Team1 = g.Home_team,
                        Team2 = g.Away_team,
                        Goals = g.Home_score + g.Away_score
                    });

            Console.WriteLine("\n======================== QUERY 8 ========================");
            foreach (var x in selectedGames)
                Console.WriteLine($"{x.MatchYear} | {x.Team1} vs {x.Team2} | Goals={x.Goals}");
        }

        static void Query9(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Tournament == "UEFA Nations League")
                    .Where(g => g.Date.Year == 2023)
                    .OrderBy(g => g.Date)
                    .Select(g => new
                    {
                        MatchYear = g.Date.Year,
                        Game = $"{g.Home_team} - {g.Away_team}",
                        Result = GetHomeResult(g)
                    });

            Console.WriteLine("\n======================== QUERY 9 ========================");
            foreach (var x in selectedGames)
                Console.WriteLine($"{x.MatchYear} | {x.Game} | {x.Result}");
        }

        static void Query10(List<FootballGame> games)
        {
            var selectedGames =
                games
                    .Where(g => g.Tournament == "Gold Cup")
                    .Where(g => g.Date.Year == 2023)
                    .Where(g => g.Date.Month == 7)
                    .OrderBy(g => g.Date)
                    .ThenBy(g => g.City)
                    .Skip(4)
                    .Take(6);

            Console.WriteLine("\n======================== QUERY 10 ========================");
            foreach (var g in selectedGames)
                PrintGame(g);
        }

        static void Query11(List<FootballGame> games)
        {
            var selectedCountries =
                games
                    .Where(g => g.Date.Year == 2020)
                    .Select(g => g.Country)
                    .Distinct()
                    .OrderBy(c => c)
                    .Take(10);

            Console.WriteLine("\n======================== QUERY 11 ========================");
            foreach (var c in selectedCountries)
                Console.WriteLine(c);
        }

        static void Query12(List<FootballGame> games)
        {
            var selectedTournaments =
                games
                    .Where(g => g.Date.Year >= 2020)
                    .GroupBy(g => g.Tournament)
                    .Select(gr => new
                    {
                        Tournament = gr.Key,
                        Count = gr.Count()
                    })
                    .Where(x => x.Count > 200)
                    .OrderByDescending(x => x.Count)
                    .ThenBy(x => x.Tournament);

            Console.WriteLine("\n======================== QUERY 12 ========================");
            foreach (var x in selectedTournaments)
                Console.WriteLine($"{x.Tournament} | Count={x.Count}");
        }

        static void Query13(List<FootballGame> games)
        {
            var topCountries =
                games
                    .Where(g => g.Neutral)
                    .GroupBy(g => g.Country)
                    .Select(gr => new
                    {
                        Country = gr.Key,
                        Count = gr.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .ThenBy(x => x.Country)
                    .Take(3);

            Console.WriteLine("\n======================== QUERY 13 ========================");
            foreach (var x in topCountries)
                Console.WriteLine($"{x.Country} | Count={x.Count}");
        }

        static void Query14(List<FootballGame> games)
        {
            var topTournaments =
                games
                    .GroupBy(g => g.Tournament)
                    .Select(gr => new
                    {
                        Tournament = gr.Key,
                        AvgGoals =
                            gr.Average(g => g.Home_score + g.Away_score)
                    })
                    .OrderByDescending(x => x.AvgGoals)
                    .ThenBy(x => x.Tournament)
                    .Take(5);

            Console.WriteLine("\n======================== QUERY 14 ========================");
            foreach (var x in topTournaments)
                Console.WriteLine($"{x.Tournament} | AvgGoals={x.AvgGoals:F2}");
        }

        static void Query15(List<FootballGame> games)
        {
            var oneGameTeams =
                games
                    .SelectMany(g => new[] { g.Home_team, g.Away_team })
                    .GroupBy(t => t)
                    .Select(gr => new
                    {
                        Team = gr.Key,
                        Count = gr.Count()
                    })
                    .Where(x => x.Count == 1)
                    .OrderBy(x => x.Team);

            Console.WriteLine("\n======================== QUERY 15 ========================");
            foreach (var x in oneGameTeams)
                Console.WriteLine($"{x.Team} | Count={x.Count}");
        }
    }
}
