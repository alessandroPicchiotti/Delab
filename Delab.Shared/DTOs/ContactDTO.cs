using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Shared.DTOs;

public class ContactDTO
{
    public string Numero { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;
}

