using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace ClientManagementAPI.DTOs;

public class ClientDto
{
    public long INN { get; init; }
    public string Name { get; init; }
    [MaxLength(2)] public string Type { get; init; }

    [SwaggerSchema(ReadOnly = true)] public DateOnly DateAdded { get; init; }

    [SwaggerSchema(ReadOnly = true)] public DateOnly DateUpdated { get; init; }
}