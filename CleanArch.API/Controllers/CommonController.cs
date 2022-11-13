using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RPGOnline.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CommonController : ControllerBase
    {
    }
}
