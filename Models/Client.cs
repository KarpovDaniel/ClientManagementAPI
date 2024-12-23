﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientManagementAPI.Models;

public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long INN { get; set; }

    [Required] public string Name { get; set; }
    [Required] public string Type { get; set; }
    public DateOnly DateAdded { get; set; }
    public DateOnly DateUpdated { get; set; }
    public List<Founder> Founders { get; set; }
}