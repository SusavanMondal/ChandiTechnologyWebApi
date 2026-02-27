using ChandiTechnologyWebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ChandiTechnologyWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {

        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;

        public HotelController(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }




        [Authorize]
        [HttpGet("Search")]



        public async Task<IActionResult> Get(
            [FromQuery] string destinationCode,
            [FromQuery] string checkIn,
            [FromQuery] string checkOut,
            [FromQuery] int adults,
            [FromQuery] int children = 0)
        {
            // 1. Basic validation
            if (string.IsNullOrEmpty(destinationCode) || string.IsNullOrEmpty(checkIn) || string.IsNullOrEmpty(checkOut))
            {
                return BadRequest("Destination code, check-in, and check-out dates are required.");
            }

            try
            {
                string apiKey = _configuration["Hotelbeds:ApiKey"];
                string apiSecret = _configuration["Hotelbeds:ApiSecret"];
                string endpoint = "https://api.test.hotelbeds.com/hotel-api/1.0/hotels";

                string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                string signatureToHash = apiKey + apiSecret + timestamp;
                string xSignature = GenerateSha256Hash(signatureToHash);

                var requestPayload = new
                {
                    stay = new { checkIn = checkIn, checkOut = checkOut },
                    occupancies = new[]
                    {
                        new { rooms = 1, adults = adults, children = children }
                    },
                    destination = new { code = destinationCode }
                };

                string jsonPayload = JsonSerializer.Serialize(requestPayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Api-key", apiKey);
                    client.DefaultRequestHeaders.Add("X-Signature", xSignature);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    var response = await client.PostAsync(endpoint, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(JsonDocument.Parse(responseString));
                    }

                    return StatusCode((int)response.StatusCode, JsonDocument.Parse(responseString));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        private static string GenerateSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }






}
