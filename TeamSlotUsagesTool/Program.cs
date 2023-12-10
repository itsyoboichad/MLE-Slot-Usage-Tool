using System;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using TeamSlotUsagesTool;

List<Match> matches = new();
List<Player> playerList = new();

string usageString = string.Empty;
string playerDataString = string.Empty;
Console.WriteLine("Sending requests to Stonks...");
var matchFetchTask = GetMatchData();
matchFetchTask.Wait();
Console.WriteLine("Retrieved match data\nGetting player data");
var playerFetchTask = GetPlayerData();
playerFetchTask.Wait();
Console.WriteLine("Retrieved player data");



string[] usageLines = usageString.Split('\n');
Console.WriteLine("Loading matches...");

#region consuming match data
for (int i = 1; i < usageLines.Length; i++)
{
    string[] entry = usageLines[i].Split(',');
    if (entry.Length < 9)
        continue;

    int season = 0;
    if (!int.TryParse(entry[0], out season))
    {
        Console.WriteLine("Failure parsing season: " + usageLines[i]);
    }
    int matchNumber = 0;
    if (!int.TryParse(entry[1], out matchNumber))
    {
        Console.WriteLine("Failure parsing match number: " + usageLines[i]);
    }
    TeamName home;
    if (!Enum.TryParse(entry[2], out home))
    {
        Console.WriteLine("Failure parsing team name: " + usageLines[i]);
    }
    TeamName away;
    if (!Enum.TryParse(entry[3], out away))
    {
        Console.WriteLine("Failure parsing team name: " + usageLines[i]);
    }
    if (!int.TryParse(entry[4], out var id))
    {
        byte[] bytes = Encoding.ASCII.GetBytes(entry[4]);
        Console.WriteLine("Failure parsing match id: " + entry[4] + " " + GetBytes(entry[4]));
    }
    Leagues league;
    if (!Enum.TryParse(entry[5], out league))
    {
        Console.WriteLine("Failure parsing league name: " + usageLines[i]);
    }
    GameMode gameMode;
    if (!Enum.TryParse(entry[6], out gameMode))
    {
        Console.WriteLine("Failure parsing game mode: " + usageLines[i]);
    }
    TeamName team;
    if (!Enum.TryParse(entry[7], out team))
    {
        Console.WriteLine("Failure parsing team name: " + usageLines[i]);
    }
    Slots slot;
    if (!Enum.TryParse(entry[8], out slot))
    {
        Console.WriteLine("Failure parsing slot: " + usageLines[i]);
    }

    Match match = new(season, matchNumber, home, away, id, league, gameMode, team, slot);

    matches.Add(match);
}
#endregion
Console.WriteLine("Processing Player Data");
string[] playerLines = playerDataString.Split('\n');
#region Consume player data
// Need player name, salary, league, team, scrim points
for (int i = 1; i < playerLines.Length; i++)
{
    string[] entry = playerLines[i].Split(',');
    if (entry.Length < 13)
        continue;

    string playerName = entry[2];
    float salary;
    if (!float.TryParse(entry[3], out salary))
    {
        Console.WriteLine("Failure parsing player salary: " + playerLines[i]);
    }
    Leagues league;
    if (!Enum.TryParse(entry[4], out league))
    {
        Console.WriteLine("Failure parsing league name: " + playerLines[i]);
        continue;
    }
    TeamName team;
    if (!Enum.TryParse(entry[5], out team))
    {
        Console.WriteLine("Failure parsing team name: " + entry[5]);
        continue;
    }
    Slots slot;
    if (!Enum.TryParse(entry[6], out slot))
    {
        Console.WriteLine("Failure parsing slot: " + playerLines[i]);
        continue;
    }
    int scrimPoints = int.Parse(entry[10]);
    Player player = new(playerName, salary, league, team, slot, scrimPoints);
    playerList.Add(player);
}
#endregion

Console.WriteLine("\n\n\n\nWelcome to the Team Slot Usages Tool");
Console.WriteLine("------------------------------------");
Console.WriteLine("To display a teams usage, please enter the team and league you would like to view. For example: 'Jets AL'");
Console.WriteLine("To quit, enter 'quit'");



string input = string.Empty;
do
{
    input = Console.ReadLine() ?? string.Empty;
    input = input.Trim();
    if (input != string.Empty)
    {
        input = input.ToLower();
        char firstLetter = input[0];
        firstLetter = firstLetter.ToString().ToUpper()[0];
        input = input.Remove(0, 1);
        input = firstLetter + input;
        string[] split = input.Split(' ');
        if (split.Length != 2)
        {
            Console.WriteLine("Please enter a valid input, specifying the team name and abbreviated league");
            continue;
        }
        TeamName selectedTeam;
        if (!Enum.TryParse(split[0], out selectedTeam))
        {
            Console.WriteLine("Please enter a valid team name");
            continue;
        }
        Leagues selectedLeague;
        if (!Enum.TryParse(GetLeagueNameFromAbbreviation(split[1]), out selectedLeague))
        {
            Console.WriteLine("Please enter a valid league (FL, AL, CL, ML, PL)");
            continue;
        }
        Console.WriteLine($"\n\n\n\nTeam {selectedTeam.ToString().PadLeft(10, ' ')} usages:");
        Console.WriteLine("-----------------------");
        List<Match> filtered = FilterMatches(selectedTeam, selectedLeague);
        Dictionary<Slots, Player> players = GetPlayers(selectedTeam, selectedLeague);
        Usage[] usages = GetSlotUsageCounts(filtered);
        for (int i = 0; i < usages.Length; i++)
        {
            string doubles = usages[i].GetDoublesUsage().ToString().PadLeft(2, ' ');
            string standard = usages[i].GetStandardUsage().ToString().PadLeft(2, ' ');
            string combined = usages[i].GetCombinedUsage().ToString().PadLeft(2, ' ');
            Player player;
            string playerSlot = (players.TryGetValue((Slots)i, out player)) ? player.PlayerName : ((Slots)i).ToString();
            playerSlot = playerSlot.PadRight(20, ' ');
            string sal = player?.Salary.ToString().PadLeft(4, ' ') ?? "    ";
            string scrimPoints = player?.ScrimPoints.ToString().PadLeft(4, ' ') ?? "    ";
            Console.WriteLine($"{playerSlot} | Sal: {sal}  Scrim Points: {scrimPoints}  Doubles: {doubles}  Standard: {standard}  Combined: {combined}");
        }

        Console.WriteLine("\nTo get another teams usages, please enter the team name, or type 'quit' to exit");
    }
} while (input != "Quit");

Dictionary<Slots, Player> GetPlayers(TeamName team, Leagues leagues)
{
    Dictionary<Slots, Player> players = new();
    foreach (Player player in playerList)
    {
        if (player.Team == team && player.League == leagues)
        {
            players.Add(player.Slot, player);
        }
    }

    return players;
}

List<Match> FilterMatches(TeamName team, Leagues league)
{
    List<Match> foundMatches = new();

    foreach (Match match in matches)
    {
        if (match.Team == team && match.League == league)
        {
            foundMatches.Add(match);
        }
    }

    return foundMatches;
}

Usage[] GetSlotUsageCounts(List<Match> matches)
{
    Usage[] usage = new Usage[8];
    for (int i = 0; i < usage.Length; i++)
        usage[i] = new Usage();

    foreach (Match match in matches)
    {
        usage[(int)match.Slot].Use(match.GameMode);
    }
    return usage;
}

string GetLeagueNameFromAbbreviation(string ab)
{
    if (ab == "fl")
        return "FOUNDATION";
    if (ab == "al")
        return "ACADEMY";
    if (ab == "cl")
        return "CHAMPION";
    if (ab == "ml")
        return "MASTER";
    if (ab == "pl")
        return "PREMIER";
    return "error";
}

async Task GetPlayerData()
{
    HttpClient client = new HttpClient();
    try
    {
        using HttpResponseMessage response = await client.GetAsync("https://stonks.mlesports.dev/public/question/391c2e0b-84e5-41d3-894d-8935a68f303d.csv");
        response.EnsureSuccessStatusCode();
        playerDataString = await response.Content.ReadAsStringAsync();
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("\nException Caught!");
        Console.WriteLine("Message :{0} ", e.Message);
    }
}

async Task GetMatchData()
{
    HttpClient client = new HttpClient();
    try
    {
        using HttpResponseMessage response = await client.GetAsync("https://stonks.mlesports.dev/public/question/6490552f-9e81-4dad-8fad-798eaaa81fba.csv");
        response.EnsureSuccessStatusCode();
        usageString = await response.Content.ReadAsStringAsync();
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("\nException Caught!");
        Console.WriteLine("Message :{0} ", e.Message);
    }
}

string GetBytes(string s)
{
    byte[] bytes = Encoding.ASCII.GetBytes(s);
    StringBuilder stringBuilder = new StringBuilder();
    foreach (byte b in bytes)
    {
        stringBuilder.Append(b);
    }
    return stringBuilder.ToString();
}