using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Shared.DTOs;

public class EmailDTO
{
    [Required(ErrorMessage = "El Campo de Email es Obligatorio")]
    [EmailAddress(ErrorMessage = "Debe ser un formato de correo valido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
}
