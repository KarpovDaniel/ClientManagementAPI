using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClientManagementAPI.DTOs;

namespace ClientManagementAPI.Models;

public class Client
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long INN { get; set; }

    public string Name { get; set; }

    [MaxLength(2)] public string Type { get; set; }

    public DateOnly DateAdded { get; init; }

    public DateOnly DateUpdated { get; init; }

    public List<Founder> Founders { get; set; }

    public ClientDto ToDto()
    {
        return new ClientDto
        {
            INN = INN,
            Name = Name,
            Type = Type,
            DateAdded = DateAdded,
            DateUpdated = DateUpdated
        };
    }
}