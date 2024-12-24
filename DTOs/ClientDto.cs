using Swashbuckle.AspNetCore.Annotations;

namespace ClientManagementAPI.DTOs;

public class ClientDto
{
    public long INN { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    [SwaggerSchema(ReadOnly = true)] public DateOnly DateAdded { get; init; }

    [SwaggerSchema(ReadOnly = true)] public DateOnly DateUpdated { get; init; }
}