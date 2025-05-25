using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Delab.Shared.DTOs;

public class LoginDTO
{
    [Display(Name = "User")]
    [Required(ErrorMessage = " {0} is required")]
    public string Email { get; set; } = null!;

    [Display(Name = "Clave")]
    [Required(ErrorMessage = "{0} is required")]
    [MinLength(6, ErrorMessage = "{0} it must be at least 6 Caracters ")]
    public string Password { get; set; } = null!;
}
