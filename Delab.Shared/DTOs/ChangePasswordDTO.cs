using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.DTOs;

public class ChangePasswordDTO
{

    [Required(ErrorMessage = "Questo campo è obbligatorio")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "La password minimo di {2} e massimo di {1}")]
    [Display(Name = "Password attuale")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "Questo campo è obbligatorio")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "La password deve avere un minimo di {2} e un massimo di {1}")]
    [Display(Name = "Nuova password")]
    public string NewPassword { get; set; } = null!;

    [Compare("NewPassword", ErrorMessage = "Le password non coincidono")]
    [Required(ErrorMessage = "Questo campo è obbligatorio")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "La password minima {2} e massima di {1}")]
    [Display(Name = "Conferma password")]
    public string Confirm { get; set; } = null!;
}
