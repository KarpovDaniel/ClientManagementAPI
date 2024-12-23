using Swashbuckle.AspNetCore.Annotations;

namespace ClientManagementAPI.DTOs;

public class FounderDto
{
    public long INN { get; set; }
    public string FullName { get; set; }

    [SwaggerSchema(ReadOnly = true)] public DateOnly DateAdded { get; init; }

    [SwaggerSchema(ReadOnly = true)] public DateOnly DateUpdated { get; init; }

    public long ClientINN { get; set; }
    public ClientDto? ClientDto { get; set; }
}