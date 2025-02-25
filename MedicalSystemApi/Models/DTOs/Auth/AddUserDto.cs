using MedicalSystemApi.Models.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

public class AddUserDto : RequestRegisterDto
{
    [Required(ErrorMessage = "Role Name is required")]
    public string Role { get; set; }
}
