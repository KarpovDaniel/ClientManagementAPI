namespace ClientManagementAPI.DTOs;

public class ClientDto
{
    public long INN { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateOnly DateAdded { get; set; }
    public DateOnly DateUpdated { get; set; }
    public List<string> FounderNames { get; set; }
}