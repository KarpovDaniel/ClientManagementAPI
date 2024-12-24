using ClientManagementAPI.Data;
using ClientManagementAPI.DTOs;
using ClientManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoundersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FounderDto>>> GetFounders()
    {
        var founders = await context.Founders
            .AsNoTracking()
            .Include(f => f.Client)
            .Select(f => new FounderDto
            {
                INN = f.INN,
                FullName = f.FullName,
                DateAdded = f.DateAdded,
                DateUpdated = f.DateUpdated,
                ClientINN = f.ClientINN,
                ClientDto = new ClientDto
                {
                    INN = f.Client.INN,
                    Name = f.Client.Name,
                    Type = f.Client.Type,
                    DateAdded = f.Client.DateAdded,
                    DateUpdated = f.Client.DateUpdated
                }
            })
            .ToListAsync();

        return Ok(founders);
    }

    [HttpGet("{inn:long}")]
    public async Task<ActionResult<FounderDto>> GetFounder(long inn)
    {
        var founder = await context.Founders
            .AsNoTracking()
            .Include(f => f.Client)
            .Where(f => f.INN == inn)
            .Select(f => new FounderDto
            {
                INN = f.INN,
                FullName = f.FullName,
                DateAdded = f.DateAdded,
                DateUpdated = f.DateUpdated,
                ClientINN = f.ClientINN,
                ClientDto = new ClientDto
                {
                    INN = f.Client.INN,
                    Name = f.Client.Name,
                    Type = f.Client.Type,
                    DateAdded = f.Client.DateAdded,
                    DateUpdated = f.Client.DateUpdated
                }
            })
            .FirstOrDefaultAsync();

        if (founder == null)
        {
            return NotFound();
        }

        return Ok(founder);
    }

    [HttpPost]
    public async Task<ActionResult<FounderDto>> PostFounder([FromBody] FounderDto founderDto)
    {
        var client = await context.Clients.FindAsync(founderDto.ClientINN);
        if (client is not { Type: "ЮЛ" })
        {
            return BadRequest("Клиент должен быть юридическим лицом.");
        }

        var founder = new Founder
        {
            INN = founderDto.INN,
            FullName = founderDto.FullName,
            ClientINN = founderDto.ClientINN,
            Client = client
        };

        context.Founders.Add(founder);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFounder), new { inn = founder.INN }, founder.ToFounderDto());
    }

    [HttpPut("{inn:long}")]
    public async Task<IActionResult> PutFounder(long inn, FounderDto founderDto)
    {
        var existingFounder = await context.Founders.FindAsync(inn);
        if (existingFounder == null)
        {
            return NotFound();
        }

        var client = await context.Clients.FindAsync(founderDto.ClientINN);
        if (client is not { Type: "ЮЛ" })
        {
            return BadRequest("Client must be a Legal Entity.");
        }

        existingFounder.INN = founderDto.INN;
        existingFounder.FullName = founderDto.FullName;
        existingFounder.ClientINN = founderDto.ClientINN;
        existingFounder.Client = client;

        context.Entry(existingFounder).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{inn:long}")]
    public async Task<IActionResult> DeleteFounder(long inn)
    {
        var founder = await context.Founders.FindAsync(inn);
        if (founder == null)
        {
            return NotFound();
        }

        context.Founders.Remove(founder);
        await context.SaveChangesAsync();

        return NoContent();
    }
}