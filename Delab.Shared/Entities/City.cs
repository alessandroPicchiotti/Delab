
using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class City
{
    [Key]
    public int CityId { get; set; }

    [Required(ErrorMessage = "Il campo {0} + obbligatorio")]
    [MaxLength(100, ErrorMessage = "Il campo {0} può avere amssimo {1} caratteri")]
    [Display(Name = "Citta")]
    public string Name { get; set; } =null!;
    public int StateId { get; set; }

    public State ?State { get; set; }    
}
