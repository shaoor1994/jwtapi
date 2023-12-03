using Microsoft.AspNetCore.Mvc;
using System;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
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

            return Ok(responseData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Exception: {ex.Message}");
        }
    }
}
