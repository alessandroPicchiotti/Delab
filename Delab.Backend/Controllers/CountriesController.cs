
using Delab.AccessData.Data;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Controllers;

[Route("api/countries")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly DataContext _context;

    public CountriesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        var list = await _context.Countries.OrderBy(x => x.Name).ToListAsync();

        return Ok(list);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        var contry = await _context.Countries.FindAsync(id);

        return Ok(contry);
    }
    [HttpPost]
    public async Task<IActionResult> PostCountry([FromBody]Country model)
    {
        _context.Countries.Add(model);
        await  _context.SaveChangesAsync(); 
        return Ok();

    }
}
