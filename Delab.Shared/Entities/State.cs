
using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class State
{
    [Key]
    public int StateId { get; set; }

    [Required(ErrorMessage = "Il campo {0} + obbligatorio")]
    [MaxLength(100, ErrorMessage = "Il campo {0} può avere amssimo {1} caratteri")]
    [Display(Name = "Dipartimento/Stato/Regioni")]
    public string Name { get; set; } = null!;
    public int CountryId { get; set; }
    public Country ?Country { get; set; }

    public ICollection<City> ?Cities { get; set; }   
}
