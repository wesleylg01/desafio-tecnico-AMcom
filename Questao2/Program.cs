using System.Text.Json;
using System.Text.Json.Serialization;

public class Program
{
    public static async Task Main(string[] args)
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string teamName, int year)    
    {
        int totalGoals = 0;

        totalGoals += await getGoals(teamName, year, "team1");
        totalGoals += await getGoals(teamName, year, "team2");

        return totalGoals;
    }
    public static async Task<int> getGoals(string teamName, int year, string teamPosition)
    {
        using HttpClient client = new HttpClient();

        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = 1;

        do
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamPosition}={Uri.EscapeDataString(teamName)}&page={currentPage}";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Houve um erro na requisição: {response.StatusCode}");
                return 0;
            }

            string content = await response.Content.ReadAsStringAsync();

            FootballApiResponse? apiResponse = JsonSerializer.Deserialize<FootballApiResponse>(content);
            if (apiResponse == null || apiResponse.Data == null)
            {
                Console.WriteLine("Falha ao desserializar: conteúdo da API é nulo ou inválido.");
                return 0;
            }

            foreach (var match in apiResponse.Data)
            {
                int goals = int.Parse(teamPosition == "team1" ? match.Team1Goals : match.Team2Goals);
                totalGoals += goals;
            }

            totalPages = apiResponse.TotalPages;
            currentPage++;

        } while (currentPage <= totalPages);

        return totalGoals;
    }
}
public class FootballApiResponse
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("data")]
    public List<FootballMatch> Data { get; set; }
}

public class FootballMatch
{
    [JsonPropertyName("competition")]
    public string Competition { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("round")]
    public string Round { get; set; }

    [JsonPropertyName("team1")]
    public string Team1 { get; set; }

    [JsonPropertyName("team2")]
    public string Team2 { get; set; }

    [JsonPropertyName("team1goals")]
    public string Team1Goals { get; set; }

    [JsonPropertyName("team2goals")]
    public string Team2Goals { get; set; }
}