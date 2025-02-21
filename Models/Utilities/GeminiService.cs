using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateContent";
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"];
    }

    public async Task<string> GetAIResponseAsync(string userMessage)
    {
        int maxRetries = 5;
        int delayMilliseconds = 2000; 

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                    new { parts = new[] { new { text = userMessage } } }
                }
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{ApiUrl}?key={_apiKey}", jsonContent);

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
                    return jsonResponse.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                }
                if ((int)response.StatusCode == 503 && attempt < maxRetries)
                {
                    await Task.Delay(delayMilliseconds);
                    delayMilliseconds *= 2;
                    continue;
                }

                return $"Error: {response.StatusCode} - {responseString}";
            }
            catch (Exception ex)
            {
                return $"Error: Unable to connect to AI service. Exception: {ex.Message}";
            }
        }
        return "Service is currently unavailable. Please try again later.";
    }

}
