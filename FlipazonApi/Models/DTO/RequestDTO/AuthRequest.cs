using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlipazonApi.Models.DTO.RequestDTO
{
    public class AuthRequest
    {
        /// <summary>
        /// Email is required with right format.
        /// </summary>
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Password is required and has a limit between 8 to 64 characters.
        /// </summary>
        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(maximumLength:20, MinimumLength = 6, ErrorMessage = "Password must be atleast 6 digits.")]
        public string Password { get; set; } = null!;
    }
}
