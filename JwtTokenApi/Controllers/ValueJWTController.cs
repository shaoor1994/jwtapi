using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class ValueJWTController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            // Hardcoded response for demonstration purposes
            var responseData = new
            {
                id = 1,
                name = "Sample Data",
                description = "This is a sample response for an HTTP GET request."
            };

            // Generate a JWT token
            string jwtToken = GenerateJwtToken("sample_user");

            // Include the token in the response
            var responseWithToken = new
            {
                data = responseData,
                token = jwtToken
            };

            return Ok(responseWithToken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Exception: {ex.Message}");
        }
    }

    private string GenerateJwtToken(string username)
    {
        // Replace this placeholder with a secure method for key generation in a production environment
        string secretKey = "your_secure_random_key";

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
}
