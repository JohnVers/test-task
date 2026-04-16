using Microsoft.AspNetCore.Mvc;

namespace task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            
            return Ok();
        }
    }
}
