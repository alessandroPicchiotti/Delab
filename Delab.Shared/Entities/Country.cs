
using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }

    [Required(ErrorMessage ="Il campo {0} + obbligatorio")]
    [MaxLength(100,ErrorMessage =   "Il campo {0} può avere amssimo {1} caratteri")]
    [Display(Name="Paese/Nazioni")]
    public string Name { get; set; } =null!;
    [MaxLength(10, ErrorMessage = "Il campo {0} può avere amssimo {1} caratteri")]
    [Display(Name = "Cod Phone")]
    public string ?CodPhone { get; set; } 
    public ICollection<State> ?States { get; set; }
}
