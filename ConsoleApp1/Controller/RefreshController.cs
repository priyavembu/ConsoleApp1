using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RefreshController : ControllerBase
{
    private readonly DataRefresh _service;

    public RefreshController(DataRefresh service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult RefreshData()
    {
        try
        {
            _service.RefreshData();
            return Ok(new { message = "Data refresh started successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}