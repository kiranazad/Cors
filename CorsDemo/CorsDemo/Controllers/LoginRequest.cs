using System.ComponentModel.DataAnnotations;

namespace CorsDemo.Controllers
{
    public class LoginRequest
    {
            [Required(ErrorMessage = "Username is required.")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "TenantId is required.")]
            [Range(1, long.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}")]
            public long TenantId { get; set; }
    }
}