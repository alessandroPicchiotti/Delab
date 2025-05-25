
namespace Delab.Shared.Entities;

using Delab.Shared.Enum;
using System.ComponentModel.DataAnnotations;

public class UserRoleDetails
{
    [Key]
    public int UserRoleDetailsId { get; set; }

    [Display(Name = "Rol User")]
    public TypeUser? UserType { get; set; }

    [Display(Name = "User Id")]
    public string? UserId { get; set; }
    //Relacion
    public User? User { get; set; }
}