using System.ComponentModel.DataAnnotations;

namespace AuthService.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name Is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User Name Is Required")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "A user has to belong to an organisation.")]
        public string OrganisationId { get; set; }
    }
}
