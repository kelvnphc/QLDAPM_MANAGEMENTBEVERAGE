using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;

namespace PPKBeverageManagement.Controllers
{
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyAb1nRQx1_owiauJXCEeYd22T0SROvkIEM"; // Thay bằng API key của bạn

        public GeminiController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        [Route("UseGemini")]
        public async Task<IActionResult> UseGemini(string query)
        {
            var requestContent = new JObject
            {
                ["contents"] = new JArray
                {
                    new JObject
                    {
                        ["parts"] = new JArray
                        {
                            new JObject { ["text"] = query }
                        }
                    }
                }
            }.ToString();

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}"),
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseJson = JObject.Parse(responseBody);
                var text = responseJson["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
                return Ok(text ?? "No response text found.");
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
