using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;

[HttpGet]
public async Task<IActionResult> Get()
{
    try
    {
        // Replace these values with your actual authentication credentials and endpoint
        string apiUrl = "https://api.example.com/data";
        string username = "your_real_username";
        string password = "your_real_password";

        // Validate user credentials
        if (ValidateCredentials(username, password))
        {
            // Replace this placeholder with a secure method for key generation in a production environment
            string secretKey = "your_secure_random_key";

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
        issuer: "your_real_issuer",
        audience: "your_real_audience",
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
