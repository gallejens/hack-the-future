using System.Text.Json;
using System.Text;

namespace Shared
{
    public static class APICalls
    {
        private static readonly string AUTH_TOKEN = "Team 17BC43C7-5CE5-4FBF-9AE5-1D3C1C66AA87";
        private static readonly string BASE_URL = "https://exs-htf-2024.azurewebsites.net";

        public static async Task<T> Get<T>(string endpoint)
        {

            using (var client = new HttpClient())
            {
                // Add custom auth header
                client.DefaultRequestHeaders.Add("Authorization", AUTH_TOKEN);

                try
                {
                    HttpResponseMessage response = await client.GetAsync($"{BASE_URL}{endpoint}");

                    // Ensure successful status code`
                    response.EnsureSuccessStatusCode();

                    // Read and deserialize the response content
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(jsonResponse);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Request Error: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                }
            }

            Console.WriteLine("Failed to get");
            System.Environment.Exit(0);
            return default(T);
        }

        public static async Task Post<T>(string endpoint, T body)
        {
            using (var client = new HttpClient())
            {
                // Add custom auth header
                client.DefaultRequestHeaders.Add("Authorization", AUTH_TOKEN);

                try
                {
                    string jsonBody = JsonSerializer.Serialize(body);

                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{BASE_URL}{endpoint}", content);

                    // Ensure successful status code
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Request Error: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                }
            }
        }
    }
}
