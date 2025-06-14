﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Shared.Entities;
//TODO : ciao
public class User : IdentityUser
{
    [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    [Display(Name = "Nombres")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo no puede ser mayor a {1} de largo")]
    [Display(Name = "Apellidos")]
    public string LastName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "El campo no puede ser mayor a {0} de largo")]
    public string? FullName { get; set; }

    [Required(ErrorMessage = "El {0} es Obligatorio")]
    [MaxLength(50, ErrorMessage = "El {0} no puede tener mas de {1} Caracteres.")]
    [Display(Name = "Posizione Lavorativa")]
    public string JobPosition { get; set; } = null!;

    //Identificacion de Origenes y Role del Usuario
    [Display(Name = "Origen")]
    public string? UserFrom { get; set; }

    [Display(Name = "Foto")]
    public string? PhotoUser { get; set; }

    [Display(Name = "Attivo")]
    public bool Active { get; set; }

    [NotMapped]
    public string? Pass { get; set; }

    public int? CorporationId { get; set; }

    ////Relaciones
    public Corporation? Corporation { get; set; }

    public ICollection<UserRoleDetails>? UserRoleDetails { get; set; }
}
