namespace ClientManagementAPI.DTOs;

public class FounderDto
{
    public long INN { get; set; }
    public string FullName { get; set; }
    public DateOnly DateAdded { get; set; }
    public DateOnly DateUpdated { get; set; }
    public long ClientINN { get; set; }
    public ClientDto? ClientDto { get; set; }
}