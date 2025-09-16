using System.ComponentModel.DataAnnotations;

namespace Services.User.Application.DTOs;

public class UpdateUserDto
{
    [Required]
    [MinLength(2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string LastName { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    public DateTime DateOfBirth { get; set; }
    
    public bool IsActive { get; set; }
}
