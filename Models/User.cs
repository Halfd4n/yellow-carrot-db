using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCarrotDb.Models;
public class User
{
    public int UserId { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public bool IsAdmin { get; set; } = false;
}
