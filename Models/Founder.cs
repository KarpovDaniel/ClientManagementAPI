using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClientManagementAPI.DTOs;

namespace ClientManagementAPI.Models;

public class Founder
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long INN { get; set; }

    [Required] public string FullName { get; set; }
    public DateOnly DateAdded { get; set; }
    public DateOnly DateUpdated { get; set; }

    [Required] public long ClientINN { get; set; }

    public Client Client { get; set; }

    public FounderDto ToFounderDto()
    {
        return new FounderDto
        {
            INN = INN,
            FullName = FullName,
            DateAdded = DateAdded,
            DateUpdated = DateUpdated,
            ClientINN = ClientINN,
            ClientDto = new ClientDto
            {
                INN = ClientINN,
                Name = Client.Name,
                Type = Client.Type,
                DateAdded = Client.DateAdded,
                DateUpdated = Client.DateUpdated,
                FounderNames = Client.Type == "LegalEntity" ? Client.Founders.Select(f => f.FullName).ToList() : []
            }
        };
    }
}