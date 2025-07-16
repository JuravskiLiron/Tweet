using System.ComponentModel.DataAnnotations;

namespace Tweet.Models;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public int GenderId { get; set; }
    [Required]
    public IFormFile Avatar { get; set; }
    public string CurrentAvatarPath { get; set; }
}