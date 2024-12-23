using ClientManagementAPI.Data;
using ClientManagementAPI.DTOs;
using ClientManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoundersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FoundersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FounderDto>>> GetFounders()
    {
        var founders = await _context.Founders
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
                    DateUpdated = f.Client.DateUpdated,
                    FounderNames = f.Client.Type == "LegalEntity"
                        ? f.Client.Founders.Select(f1 => f1.FullName).ToList()
                        : new List<string>()
                }
            })
            .ToListAsync();

        return Ok(founders);
    }

    [HttpGet("{inn}")]
    public async Task<ActionResult<FounderDto>> GetFounder(long inn)
    {
        var founder = await _context.Founders
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
                    DateUpdated = f.Client.DateUpdated,
                    FounderNames = f.Client.Type == "LegalEntity"
                        ? f.Client.Founders.Select(f1 => f1.FullName).ToList()
                        : new List<string>()
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
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.INN == founderDto.ClientINN);
        if (client is not { Type: "LegalEntity" })
        {
            return BadRequest("Client must be a Legal Entity.");
        }

        var founder = new Founder
        {
            INN = founderDto.INN,
            FullName = founderDto.FullName,
            DateAdded = DateOnly.FromDateTime(DateTime.Now),
            DateUpdated = DateOnly.FromDateTime(DateTime.Now),
            ClientINN = founderDto.ClientINN,
            Client = client
        };

        _context.Founders.Add(founder);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFounder), new { inn = founder.INN }, founder.ToFounderDto());
    }

    [HttpPut("{inn}")]
    public async Task<IActionResult> PutFounder(long inn, FounderDto founderDto)
    {
        var existingFounder = await _context.Founders.FirstOrDefaultAsync(f => f.INN == inn);
        if (existingFounder == null)
        {
            return NotFound();
        }

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.INN == founderDto.ClientINN);
        if (client is not { Type: "LegalEntity" })
        {
            return BadRequest("Client must be a Legal Entity.");
        }

        existingFounder.INN = founderDto.INN;
        existingFounder.FullName = founderDto.FullName;
        existingFounder.DateUpdated = DateOnly.FromDateTime(DateTime.Now);
        existingFounder.ClientINN = founderDto.ClientINN;
        existingFounder.Client = client;

        _context.Entry(existingFounder).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FounderExists(inn))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{inn}")]
    public async Task<IActionResult> DeleteFounder(long inn)
    {
        var founder = await _context.Founders.FindAsync(inn);
        if (founder == null)
        {
            return NotFound();
        }

        _context.Founders.Remove(founder);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FounderExists(long inn)
    {
        return _context.Founders.Any(e => e.INN == inn);
    }
}