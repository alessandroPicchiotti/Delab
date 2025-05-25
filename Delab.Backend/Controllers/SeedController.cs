using Delab.AccessData.Data;
using Delab.Backend.Data;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Delab.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly SeedDb seedDb;

        public SeedController(SeedDb seedDb)
        {
            this.seedDb = seedDb;
        }
        [HttpPost]
        public async Task<IActionResult> PostSeed()
        {
            await seedDb.SeedAsync();
            return Ok("init Db ok");
        }
    }
}
