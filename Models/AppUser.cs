using EntityFrameworkCore.EncryptColumn.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCarrotDb.Models;
public class AppUser
{
    [Key]
    public int UserId { get; set; }
    public required string Username { get; set; }
    [EncryptColumn]
    public required string Password { get; set; }
    public bool IsAdmin { get; set; } = false;
}
