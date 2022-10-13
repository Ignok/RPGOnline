using Microsoft.AspNetCore.Mvc;
using RPGOnline.Infrastructure.Models;

namespace RPGOnline.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        
        private readonly RPGOnlineDbContext _dbContext;

        public SamplesController(RPGOnlineDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        [HttpGet]
        public IActionResult GetSamples()
        {
            var result = _dbContext.Samples.ToList();
            if (result.Any()) return Ok(result);
            
            return BadRequest("Brak sampli w bazie danych");

            //return Ok(Mediator.Send(new GetSampleQuery
            //{
            //    JustSomeSampleThing = "PT"
            //}));
        }
        
    }

}
