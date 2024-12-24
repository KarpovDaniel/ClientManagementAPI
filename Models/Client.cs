using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientManagementAPI.Models;

public class Client
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long INN { get; set; }

    public string Name { get; set; }
    public string Type { get; set; }
    public DateOnly DateAdded { get; init; }
    public DateOnly DateUpdated { get; init; }
    public List<Founder> Founders { get; set; }
}