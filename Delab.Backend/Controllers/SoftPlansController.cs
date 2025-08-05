using Delab.AccessData.Data;
using Delab.Backend.Helpers;
using Delab.Shared.DTOs;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Delab.Backend.Controllers;

[Route("api/softplans")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
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
    public async Task<ActionResult<IEnumerable<SoftPlan>>> GetAsync([FromQuery] PaginationDTO pagination)
    {
        //var listaSof = await _dataContext.SoftPlans.ToListAsync();
        //return Ok(listaSof);
        var queryable = _dataContext.SoftPlans.AsQueryable();
        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.Name!.ToLower().Contains(pagination.Filter.ToLower()));
        }

        await HttpContext.InsertParameterPagination(queryable, pagination.RecordsNumber);
        return await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();
    }

    // GET api/<SoftPlansController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SoftPlan>> GetAsync(int id)
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
