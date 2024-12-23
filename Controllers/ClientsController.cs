using ClientManagementAPI.Data;
using ClientManagementAPI.DTOs;
using ClientManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
    {
        var clients = await _context.Clients
            .AsNoTracking()
            .Select(c => new ClientDto
            {
                INN = c.INN,
                Name = c.Name,
                Type = c.Type,
                DateAdded = c.DateAdded,
                DateUpdated = c.DateUpdated,
                FounderNames = c.Type == "LegalEntity"
                    ? _context.Founders.Where(f => f.ClientINN == c.INN).Select(f => f.FullName).ToList()
                    : new List<string>()
            })
            .ToListAsync();

        return Ok(clients);
    }

    [HttpGet("{inn}")]
    public async Task<ActionResult<ClientDto>> GetClient(long inn)
    {
        var client = await _context.Clients
            .AsNoTracking()
            .Where(c => c.INN == inn)
            .Select(c => new ClientDto
            {
                INN = c.INN,
                Name = c.Name,
                Type = c.Type,
                DateAdded = c.DateAdded,
                DateUpdated = c.DateUpdated,
                FounderNames = c.Type == "LegalEntity"
                    ? _context.Founders.Where(f => f.ClientINN == c.INN).Select(f => f.FullName).ToList()
                    : new List<string>()
            })
            .FirstOrDefaultAsync();

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<Client>> PostClient([FromBody] ClientDto clientDto)
    {
        var client = new Client
        {
            INN = clientDto.INN,
            Name = clientDto.Name,
            Type = clientDto.Type,
            DateAdded = DateOnly.FromDateTime(DateTime.Now),
            DateUpdated = DateOnly.FromDateTime(DateTime.Now)
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetClient), new { inn = client.INN }, client);
    }

    [HttpPut("{inn}")]
    public async Task<IActionResult> PutClient(long inn, ClientDto clientDto)
    {
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.INN == inn);
        if (existingClient == null)
        {
            return NotFound();
        }

        existingClient.INN = clientDto.INN;
        existingClient.Name = clientDto.Name;
        existingClient.Type = clientDto.Type;
        existingClient.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

        _context.Entry(existingClient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(inn))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{inn}")]
    public async Task<IActionResult> DeleteClient(long inn)
    {
        var client = await _context.Clients.FindAsync(inn);
        if (client == null)
        {
            return NotFound();
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClientExists(long inn)
    {
        return _context.Clients.Any(e => e.INN == inn);
    }
}