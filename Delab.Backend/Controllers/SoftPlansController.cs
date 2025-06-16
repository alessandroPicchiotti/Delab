using Delab.AccessData.Data;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Delab.Backend.Controllers;

[Route("api/softplans")]
[ApiController]
public class SoftPlansController : ControllerBase
{
    private readonly DataContext _dataContext;

    public SoftPlansController(DataContext dataContext)
    {
        this._dataContext = dataContext;
    }
    // GET: api/<SoftPlansController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SoftPlan>>>  Get()
    {
        var listaSof = await _dataContext.SoftPlans.ToListAsync();

        return Ok(listaSof);
    }

    // GET api/<SoftPlansController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SoftPlan>> Get(int id)
    {
        var softPlan = await _dataContext.SoftPlans.FindAsync(id);
        if(softPlan == null) {  
            return BadRequest($"Id not Exist; {id}");
        }

        return Ok(softPlan);
    }

    // POST api/<SoftPlansController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<SoftPlansController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<SoftPlansController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
