using ClientManagementAPI.Data;
using ClientManagementAPI.DTOs;
using ClientManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
    {
        var clients = await context.Clients
            .AsNoTracking()
            .Select(c => c.ToDto())
            .ToListAsync();

        return Ok(clients);
    }

    [HttpGet("{inn:long}")]
    public async Task<ActionResult<ClientDto>> GetClient(long inn)
    {
        var client = await context.Clients.FindAsync(inn);

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> PostClient([FromBody] ClientDto clientDto)
    {
        var existingClient = await context.Clients.FindAsync(clientDto.INN);
        if (existingClient != null)
        {
            return Conflict("Клиент с таким ИНН уже существует.");
        }

        var client = new Client
        {
            INN = clientDto.INN,
            Name = clientDto.Name,
            Type = clientDto.Type
        };

        context.Clients.Add(client);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetClient), new { inn = client.INN }, client.ToDto());
    }

    [HttpPut("{inn:long}")]
    public async Task<IActionResult> PutClient(long inn, ClientDto clientDto)
    {
        var existingClient = await context.Clients.FindAsync(inn);
        if (existingClient == null)
        {
            return NotFound();
        }

        existingClient.INN = clientDto.INN;
        existingClient.Name = clientDto.Name;
        existingClient.Type = clientDto.Type;

        context.Entry(existingClient).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{inn:long}")]
    public async Task<IActionResult> DeleteClient(long inn)
    {
        var client = await context.Clients.FindAsync(inn);
        if (client == null)
        {
            return NotFound();
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync();

        return NoContent();
    }
}