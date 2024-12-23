using System.ComponentModel.DataAnnotations;

namespace Web_Programming_Proje.ViewModels
{
    public class UpdateUserViewModel{

        public long UserID { get; set; }

        [StringLength(50, ErrorMessage = "Name must be between 2 and 50 characters.", MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Surname must be between 2 and 50 characters.", MinimumLength = 3)]
        public string Surname { get; set; } = string.Empty;
        [StringLength(50, ErrorMessage = "Username must be between 2 and 50 characters.", MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>])[A-Za-z\d!@#$%^&*(),.?""{}|<>]{8,}$", 
        ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; } = string.Empty;

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>])[A-Za-z\d!@#$%^&*(),.?""{}|<>]{8,}$", 
        ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string ConfirmedPassword { get; set; } = string.Empty;

        [StringLength(60, ErrorMessage = "Adresss must be between 10 and 50 characters.", MinimumLength = 10)]
        public string Address { get; set; } = string.Empty;

    }
}