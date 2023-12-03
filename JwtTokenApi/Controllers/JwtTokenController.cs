// Install necessary NuGet packages: Microsoft.AspNetCore.Mvc.NewtonsoftJson and System.IdentityModel.Tokens.Jwt
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace JwtTokenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokenController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Replace these values with your actual authentication credentials and endpoint
                string apiUrl = "https://your-api.com/resource";
                string username = "sample_user";
                string password = "sample_password";

                // Validate user credentials
                if (ValidateCredentials(username, password))
                {
                    // Generate a secure random key
                    string secretKey = GenerateSecureRandomKey();

                    // Generate a JWT token
                    string jwtToken = GenerateJwtToken(username, secretKey);

                    // Use the token to make a GET request
                    string response = await MakeGetRequest(apiUrl, jwtToken);

                    // Return the response content
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid credentials.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception: {ex.Message}");
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            // Simulate a simple credential validation logic
            // Replace this with your actual user validation logic
            return username == "sample_user" && password == "sample_password";
        }

        private string GenerateJwtToken(string username, string secretKey)
        {
            // Set token expiration time (adjust as needed)
            DateTime expires = DateTime.UtcNow.AddHours(1);

            // Create security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            // Create JWT token
            var token = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            // Serialize token to a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> MakeGetRequest(string apiUrl, string jwtToken)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Add the JWT token to the request headers
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and return the response content
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception($"Request failed. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP Request Exception: {ex.Message}");
                }
            }
        }

        private string GenerateSecureRandomKey()
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] keyBytes = new byte[32]; // Adjust the size of the key as needed (32 bytes for HMAC-SHA256)
                rng.GetBytes(keyBytes);
                return Convert.ToBase64String(keyBytes);
            }
        }
    }
}
